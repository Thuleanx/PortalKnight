using System;
using UnityEngine;
using Thuleanx.AI.FSM;

namespace Thuleanx.PortalKnight {
	[RequireComponent(typeof(ShadowEnemy))]
	public class ShadowEnemyStateMachine : StateMachine<ShadowEnemy> {
		// no code is needed here, really
		// needed because Unity is fussy

		private void Awake() {
			ConstructMachine(GetComponent<ShadowEnemy>(), 
				Enum.GetNames(typeof(ShadowEnemy.State)).Length, 
				(int) ShadowEnemy.State.Aggro, false);

			AssignState((int) ShadowEnemy.State.Aggro, new ShadowEnemy.ShadowAggroState());
			AssignState((int) ShadowEnemy.State.Attack, new ShadowEnemy.ShadowAttackState());
		}

		private void Start() {
			Init();
		}
	}
}