using UnityEngine;
using UnityEngine.AI;
using Thuleanx.AI.FSM;
using Thuleanx.Utils;
using System.Collections;

namespace Thuleanx.PortalKnight {
	public partial class SpawnerEnemy {
		public class SpawnerDeadState : GenericDeadState<SpawnerEnemy> {
			public override IEnumerator Coroutine(SpawnerEnemy spawner) {
				yield return new WaitForSeconds(3);
				spawner.OnPermaDeath?.Invoke();
			}
		}
	}
}