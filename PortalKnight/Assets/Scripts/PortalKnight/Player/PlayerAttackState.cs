using UnityEngine;
using System.Collections;

using DG.Tweening;

using Thuleanx.AI.FSM;
using Thuleanx.Combat3D;
using Thuleanx.Utils;

namespace Thuleanx.PortalKnight {
	public partial class Player {
		public class PlayerAttackState : State<Player>, iHitGenerator3D {
			Timer 	onCooldown;
			Vector3 attackDirection;
			Tween 	dragTween;

			public override bool CanEnter() => !onCooldown;

			public override void Begin(Player player) {
				player.Drag = player.attackDrag;
				attackDirection = player.lastNonZeroMovement;
				if (attackDirection.sqrMagnitude > 0) attackDirection = attackDirection.normalized;
				player.attackHitbox.HitGenerator = this;
			}

			public override void End(Player player) {
				if (player.attackHitbox.HitGenerator == this) 
					player.attackHitbox.HitGenerator = null;

				player.Drag = 0;
				player.attackHitbox.stopCheckingCollision();
				onCooldown = player.attackCooldown;
				dragTween.Kill();
			}

			public override IEnumerator Coroutine(Player player) {
				dragTween = DOVirtual.Float(player.attackDrag, 0, player.attackDuration, (x) => player.Drag = x);

				player.attackHitbox.startCheckingCollision();

				Timer waiting = player.attackDuration;
				while (waiting) {
					if (player.movement != Vector2.zero) {
						Vector3 inputDir = player.InputMovementToWorldDir(player.movement).normalized;
						player.Velocity = inputDir * player.Velocity.magnitude;
					}
					yield return null;
				}

				player.attackHitbox.stopCheckingCollision();

				onCooldown = player.attackCooldown;
				dragTween.Kill(); // gotta kill to ensure no side erffects after exiting the state

				stateMachine.SetState((int) Player.State.Neutral);
			}

			public Hit3D GenerateHit(Hitbox3D hitbox, Hurtbox3D hurtbox) {
				// assumes hitbox is parented 
				Player player = hitbox.GetComponentInParent<Player>();
				return new Hit3D(player.attackDamage, player.attackKnockback, player.transform.forward);
			}
		}
	}
}