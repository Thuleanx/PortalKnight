using System.Collections;
using Thuleanx.AI.FSM;
using Thuleanx.Combat3D;
using Thuleanx.Utils;
using UnityEngine;

namespace Thuleanx.PortalKnight {
	public partial class ShadowEnemy {
		public class ShadowAttackState : State<ShadowEnemy>, iHitGenerator3D {
			Timer onCooldown;
			bool attackFinished;

			public override bool CanEnter(ShadowEnemy monster) => !onCooldown && InAttackRange(monster);

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
				monster.Drag = monster.deccelerationAlpha;

				monster.Anim.SetTrigger(monster.attackWindupTrigger);
				yield return iWaitWhileFacingPlayer(monster, monster.attackWindupTime);
				monster.Anim.SetTrigger(monster.attackTrigger);
				yield return monster.iWaitForTrigger();
				yield return monster.iWaitForTrigger();
				// yield return iWaitWhileFacingPlayer(monster, monster.attackRecovery);
				yield return new WaitForSeconds(monster.attackRecovery);
				onCooldown = monster.attackCooldown;
				monster.Drag = 0;
				attackFinished = true;
				monster.Anim.SetTrigger(monster.neutralTrigger);
			}

			IEnumerator iWaitWhileFacingPlayer(ShadowEnemy monster, float time) {
				Timer waiting = time;
				while (waiting) {
					// face player
					Vector3 facingDir = monster.player.transform.position - monster.transform.position;
					facingDir.y = 0;
					monster.TurnToFace(facingDir, monster.attackTurnSpeed);
					yield return null;
				}
			}

			bool InAttackRange(ShadowEnemy monster) 
				=> (monster.transform.position - monster.player.transform.position).sqrMagnitude <= monster.attackRange * monster.attackRange;

			public Hit3D GenerateHit(Hitbox3D hitbox, Hurtbox3D hurtbox) {
				ShadowEnemy monster = hitbox.GetComponentInParent<ShadowEnemy>(); // kinda inefficient
				return new Hit3D(monster.attackDamage, monster.attackKnockback, monster.transform.forward, hurtbox.transform.position);
			}
		}
	}
}