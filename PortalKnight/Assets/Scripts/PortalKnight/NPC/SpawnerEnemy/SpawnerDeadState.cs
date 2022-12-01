using UnityEngine;
using UnityEngine.AI;
using Thuleanx.AI.FSM;
using Thuleanx.Utils;
using System.Collections;

namespace Thuleanx.PortalKnight {
	public partial class SpawnerEnemy {
		public class SpawnerDeadState : GenericDeadState<SpawnerEnemy> {
			public override IEnumerator Coroutine(SpawnerEnemy spawner) {
				foreach (var collider in spawner.GetComponentsInChildren<Collider>())
					collider.enabled = false;
				yield return new WaitForSeconds(1);
				spawner.OnPermaDeath?.Invoke();
				yield return new WaitForSeconds(2);
				spawner.gameObject.SetActive(false);
			}
		}
	}
}