using System.Collections;
using Thuleanx.AI.FSM;
using Thuleanx.Combat3D;
using Thuleanx.Utils;
using UnityEngine;

namespace Thuleanx.PortalKnight {
	public partial class ShadowEnemy {
		public class ShadowSpawnState : State<ShadowEnemy> {
			bool finished;
			public override void Begin(ShadowEnemy monster) {
				finished = false;
			}

			public override void End(ShadowEnemy monster) {
				monster.bodyHitbox.startCheckingCollision();
			}

			public override int Transition(ShadowEnemy monster) {
				if (finished) return (int) ShadowEnemy.State.Aggro;
				return -1;
			}

			public override IEnumerator Coroutine(ShadowEnemy monster) {
				yield return monster.iWaitForTrigger();
				finished = true;
				monster.Anim.SetTrigger(monster.neutralTrigger);
			}
		}
	}
}