using System.Collections;
using UnityEngine;
using Thuleanx.AI.FSM;
using Thuleanx.Utils;

namespace Thuleanx.PortalKnight {
	public partial class ShadowEnemy {
		// walk towards player, update destination every few frames
		// once in range for melee, switch to attack state
		// if attack is on cooldown, doesn't do anything

		public class ShadowDeathState : GenericDeadState<ShadowEnemy> {
			public override IEnumerator Coroutine(ShadowEnemy monster) {
				monster.Drag = monster.deccelerationAlpha;
				monster.Anim.SetTrigger(monster.deathTrigger);
				yield return monster.iWaitForTrigger();
				monster.gameObject.SetActive(false);
			}
		}
	}
}