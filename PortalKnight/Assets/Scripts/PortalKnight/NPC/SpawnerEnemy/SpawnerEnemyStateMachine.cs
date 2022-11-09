using System;
using UnityEngine;
using Thuleanx.AI.FSM;

namespace Thuleanx.PortalKnight {
	[RequireComponent(typeof(SpawnerEnemy))]
	public class SpawnerEnemyStateMachine : StateMachine<SpawnerEnemy> {
		// no code is needed here, really
		// needed because Unity is fussy

		private void Awake() {
			ConstructMachine(GetComponent<SpawnerEnemy>(), 
				Enum.GetNames(typeof(SpawnerEnemy.State)).Length, 
				(int) SpawnerEnemy.State.Neutral, false);

			AssignState((int) SpawnerEnemy.State.Neutral, new SpawnerEnemy.SpawnerNeutralState());
			AssignState((int) SpawnerEnemy.State.Spell, new SpawnerEnemy.SpawnerSpellState());
		}

		private void Start() {
			Init();
		}
	}
}