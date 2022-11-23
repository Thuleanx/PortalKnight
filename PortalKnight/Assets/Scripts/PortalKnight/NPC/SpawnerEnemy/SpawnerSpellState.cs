using UnityEngine;
using UnityEngine.AI;
using Thuleanx.AI.FSM;
using Thuleanx.Utils;
using System.Collections;

namespace Thuleanx.PortalKnight {
	public partial class SpawnerEnemy {

		public class SpawnerSpellState : State<SpawnerEnemy> {
			Timer onCooldown;
			bool finished = false;

			public override bool CanEnter(SpawnerEnemy spawner) => spawner.enemies.Count < spawner.maxShadowEnemies && !onCooldown;

			public override void Begin(SpawnerEnemy spawner) {
				finished = false;
			}

			public override int Transition(SpawnerEnemy spawner) {
				if (finished) return (int) State.Neutral;
				return -1;
			}

			public override IEnumerator Coroutine(SpawnerEnemy spawner) {
				yield return spawner.iSpawnEnemyInRange();
				// vfx for spawning here
				yield return new WaitForSeconds(spawner.spellDuration);

				finished = true;
			}
		}
	}
}