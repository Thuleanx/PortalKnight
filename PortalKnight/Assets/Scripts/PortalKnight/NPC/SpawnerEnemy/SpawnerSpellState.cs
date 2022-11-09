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
				Vector3 spawnPos = spawner.transform.position;

				while (true) {
					Vector3 desiredSpawnPos = spawner.transform.position + spawner.offset + Random.insideUnitSphere * spawner.range;
					NavMeshHit hit;
					if (NavMesh.SamplePosition(desiredSpawnPos, out hit, spawner.range, NavMesh.AllAreas)) {
						spawnPos = hit.position;
						break;
					} else yield return new WaitForSeconds(0.5f); // keep waiting if not hitting a point
				}

				// vfx for spawning here
				yield return new WaitForSeconds(spawner.spellDuration);

				// spawn
				ShadowEnemy enemy = spawner.SpawnEnemy(spawnPos);

				finished = true;
			}
		}
	}
}