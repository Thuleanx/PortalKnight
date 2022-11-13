using UnityEngine;
using Thuleanx.AI.FSM;
using Thuleanx.Utils;
using System.Collections;
using DG.Tweening;

using Thuleanx.Combat3D;

namespace Thuleanx.PortalKnight {
	public partial class ShadowEnemy {
		public class ShadowShootState : State<ShadowEnemy> {
			Timer onCooldown;
			bool finished;
			Tween dragging;

			public override bool CanEnter(ShadowEnemy monster) => !onCooldown && InSpecialRange(monster);

			public override void Begin(ShadowEnemy monster) {
				finished = false;
			}

			public override void End(ShadowEnemy monster) {
				dragging?.Kill();
			}

			public override int Transition(ShadowEnemy monster) {
				if (finished) return (int) ShadowEnemy.State.Aggro;
				return -1;
			}

			public override IEnumerator Coroutine(ShadowEnemy monster) {
				dragging?.Kill();
				monster.Drag = monster.deccelerationAlpha;
				// monster.StartAnimationWait();
				
				monster.Anim.SetTrigger(monster.specialTrigger);
				yield return monster.iWaitForTrigger();

				// actual special
				for (int i = 0; i < monster.specialCount; i++) {
					float phi = Mathx.RandomRange(monster.specialEmissionPhi.x, monster.specialEmissionPhi.y) * Mathf.Deg2Rad;
					float theta = 360 * i / monster.specialCount * Mathf.Deg2Rad;
					float r = monster.specialEmissionDistance;

					Vector3 spawnOffset = Calc.ToSpherical(r, phi, theta);
					Vector3 startSpeed = spawnOffset.normalized * monster.specialSpeed;

					ShadowProjectile projectile = monster.specialBulletPool.BorrowTyped<ShadowProjectile>(monster.gameObject.scene, monster.gameObject.transform.position + spawnOffset);
					projectile.Initialize(startSpeed);
				}

				yield return monster.iWaitForTrigger();
				yield return new WaitForSeconds(monster.specialRecovery);

				onCooldown = Mathx.RandomRange(monster.specialCooldown.x, monster.specialCooldown.y);

				monster.Anim.SetTrigger(monster.neutralTrigger);
				monster.Drag = 0;
				finished = true;
			}

			bool InSpecialRange(ShadowEnemy monster) 
				=> (monster.transform.position - monster.player.transform.position).sqrMagnitude <= monster.specialRange * monster.specialRange;
		}
	}
}