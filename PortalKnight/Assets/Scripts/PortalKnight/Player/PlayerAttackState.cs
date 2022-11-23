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
			bool queueAttack;
			bool canQueueAttack;
			bool canDash;

			bool OnAttack(Player player) {
				if (queueAttack || !canQueueAttack) return false;
				queueAttack = true;
				return true;
			}

			bool OnDash(Player player) { return canDash && player.StateMachine.TrySetState((int) Player.State.Dash); }

			public override bool CanEnter(Player player) => !onCooldown;

			public override void Begin(Player player) {
				player.Input.ActionHandler[(int) ActionType.Attack] = OnAttack;
				player.Input.ActionHandler[(int) ActionType.Dash] = OnDash;

				canQueueAttack = false;
				queueAttack = false;
				canDash = false;

				attackDirection = player.Input.mousePosWSFlat - player.transform.position;
				attackDirection.y = 0;
				if (attackDirection.sqrMagnitude > 0) attackDirection = attackDirection.normalized;

				player.attackHitbox.HitGenerator = this;
				player.attackHitbox.OnHit.AddListener(OnHit);
			}

			public override void End(Player player) {
				player.attackHitbox.HitGenerator = null;
				player.attackHitbox.OnHit.RemoveListener(OnHit);

				player.Input.ActionHandler[(int) ActionType.Attack] = null;
				player.Input.ActionHandler[(int) ActionType.Dash] = null;
				player.Drag = 0;
				player.attackHitbox.stopCheckingCollision();
				onCooldown = player.attackCooldown;
				dragTween.Kill();
			}

			public override IEnumerator Coroutine(Player player) {
				player.Drag = player.attackDrag;
				player.Anim.SetTrigger(player.attackTrigger);

				yield return iWaitForAttack(player, true);
				canQueueAttack = true;
				canDash = true;
				yield return iWaitForAttack(player, false);

				if (queueAttack) {
					// second attack, if queued
					attackDirection = player.Input.mousePosWSFlat - player.transform.position;
					canDash = false;
					queueAttack = false;
					canQueueAttack = false;
					player.Anim.SetTrigger(player.attack2Trigger);
					yield return iWaitForAttack(player, true);
					canDash = true;
					yield return iWaitForAttack(player, true);
				}

				onCooldown = player.attackCooldown;
				player.Anim.SetTrigger(player.neutralTrigger);
				player.StateMachine.SetState((int) Player.State.Neutral);
			}

			IEnumerator iWaitForAttack(Player player, bool ignoreQueueAttack) {
				player.StartAnimationWait();
				while (player.WaitingForTrigger && (!queueAttack || ignoreQueueAttack)) {
					player.TurnToFace(attackDirection, player.attackTurnSpeed);
					// move slightly
					// if (player.Input.movement != Vector2.zero) {
					// 	Vector3 inputDir = player.Input.MovementToWorldDir(player.Input.movement).normalized;
					// 	player.Velocity = inputDir * player.Velocity.magnitude;
					// }
					yield return null;
				}
			}

			void OnHit(Hit3D hit) {
				// bad code smell >_<
				Player player = stateMachine.GetComponent<Player>();
				player.Mana += player.manaOnHit;
			}

			public Hit3D GenerateHit(Hitbox3D hitbox, Hurtbox3D hurtbox) {
				// assumes hitbox is parented 
				Player player = hitbox.GetComponentInParent<Player>();
				return new Hit3D(player.attackDamage, player.attackKnockback, attackDirection, hurtbox.transform.position);
			}
		}
	}
}