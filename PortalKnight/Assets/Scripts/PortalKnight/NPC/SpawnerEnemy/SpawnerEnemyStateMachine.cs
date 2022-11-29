using System;
using UnityEngine;
using Thuleanx.AI.FSM;

namespace Thuleanx.PortalKnight {
	[RequireComponent(typeof(SpawnerEnemy))]
	public class SpawnerEnemyStateMachine : StateMachine<SpawnerEnemy> {
		public override void Construct() {
			ConstructMachine(GetComponent<SpawnerEnemy>(), 
				Enum.GetNames(typeof(SpawnerEnemy.State)).Length, 
				(int) SpawnerEnemy.State.Neutral, false);

			AssignState((int) SpawnerEnemy.State.Neutral, new SpawnerEnemy.SpawnerNeutralState());
			AssignState((int) SpawnerEnemy.State.Spell, new SpawnerEnemy.SpawnerSpellState());
			AssignState((int) SpawnerEnemy.State.Dead, new SpawnerEnemy.SpawnerDeadState());
		}

	}
}