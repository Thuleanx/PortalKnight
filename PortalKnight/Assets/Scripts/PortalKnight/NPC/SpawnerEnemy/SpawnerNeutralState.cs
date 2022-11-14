using UnityEngine;
using UnityEngine.AI;
using Thuleanx.AI.FSM;
using Thuleanx.Utils;
using System.Collections;

namespace Thuleanx.PortalKnight {
	public partial class SpawnerEnemy {
		public class SpawnerNeutralState : State<SpawnerEnemy> {
			public override int Transition(SpawnerEnemy spawner) {
				if (spawner.playerInRange && spawner.StateMachine.CanEnter((int) State.Spell)) 
					return (int) State.Spell;
				return -1;
			}
		}
	}
}