using UnityEngine;
using UnityEngine.AI;
using System.Collections;

using DG.Tweening;

using Thuleanx.AI.FSM;
using Thuleanx.Combat3D;
using Thuleanx.Utils;

namespace Thuleanx.PortalKnight {
	public partial class Player {
		public class PlayerNovaState : State<Player> {
			bool finished = false;

			public override bool CanEnter(Player player) => player.Mana >= 1;

			public override void Begin(Player player) {
				player.Drag = player.deccelerationAlpha;
				player.Mana--;
				finished = false;
			}

			public override void End(Player player) {
				player.Drag = 0;
			}

			public override int Transition(Player agent) => finished ? (int) Player.State.Neutral : -1;

			public override IEnumerator Coroutine(Player player) {
				Vector3 targetPos = player.Input.mousePosWS;
				// TODO: change this to waiting for the animation instead
				yield return new WaitForSeconds(player.novaDelay);
				SpawnNova(player, targetPos);
				yield return new WaitForSeconds(player.novaRecovery);
				finished = true;
			}

			void SpawnNova(Player player, Vector3 targetPos) {
				Nova nova = player.novaPool.BorrowTyped<Nova>(player.gameObject.scene);
				nova.Initialize(player);

				Puppet closestTarget = null;
				foreach (var target in GameObject.FindGameObjectsWithTag("Enemy")) {
					Puppet pup = target.GetComponent<Puppet>();
					if (ShouldSwap(closestTarget, pup, targetPos))
						closestTarget = pup;
				}
				if (closestTarget && (closestTarget.transform.position - targetPos).sqrMagnitude > player.novaTrackingRange) 
					closestTarget = null;

				if (closestTarget == null) nova.transform.position = targetPos;
				else nova.transform.position = closestTarget.transform.position;
			}

			bool ShouldSwap(Puppet a, Puppet b, Vector3 pos) {
				if (b.IsDead) return false;
				if (!a) return true;
				Alive aa = a.GetComponent<Alive>(), bb = b.GetComponent<Alive>();
				if (aa is SpawnerEnemy && bb is ShadowEnemy) return true;
				if (aa is ShadowEnemy && bb is SpawnerEnemy) return false;
				return (Vector3.Distance(aa.transform.position, pos) > Vector3.Distance(bb.transform.position, pos));
			}
		}
	}
}