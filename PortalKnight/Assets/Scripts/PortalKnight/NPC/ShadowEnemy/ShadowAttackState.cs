using UnityEngine;
using Thuleanx.AI.FSM;
using Thuleanx.Utils;
using System.Collections;

using Thuleanx.Combat3D;

namespace Thuleanx.PortalKnight {
	public partial class ShadowEnemy {
		public class ShadowAttackState : State<ShadowEnemy>, iHitGenerator3D {
			Timer onCooldown;
			bool attackFinished;

			public override void Begin(ShadowEnemy monster) {
				monster.meleeHitbox.HitGenerator = this;
				attackFinished = false;
			}

			public override void End(ShadowEnemy monster) {
				monster.Drag = 0;
				monster.meleeHitbox.stopCheckingCollision();
				monster.meleeHitbox.HitGenerator = null;
			}

			public override int Transition(ShadowEnemy monster) {
				if (attackFinished) return (int) ShadowEnemy.State.Aggro;
				return -1;
			}

			public override IEnumerator Coroutine(ShadowEnemy monster) {
				Timer waiting = monster.attackWindupTime;

				monster.Drag = monster.deccelerationAlpha;
				while (waiting) {
					// face player
					Vector3 facingDir = monster.player.transform.position - monster.transform.position;
					facingDir.y = 0;
					monster.TurnToFace(facingDir);
					yield return null;
				}
				monster.Drag = 0;

				monster.meleeHitbox.startCheckingCollision();
				waiting = monster.attackDuration;
				while (waiting) {
					yield return null;
				}

				monster.meleeHitbox.stopCheckingCollision();
				onCooldown = monster.attackCooldown;

				attackFinished = true;
			}

			public Hit3D GenerateHit(Hitbox3D hitbox, Hurtbox3D hurtbox) {
				ShadowEnemy monster = hitbox.GetComponentInParent<ShadowEnemy>(); // kinda inefficient
				return new Hit3D(monster.attackDamage, monster.attackKnockback, monster.transform.forward, hurtbox.transform.position);
			}
		}
	}
}