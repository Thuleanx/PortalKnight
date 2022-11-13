using System;
using UnityEngine;
using Thuleanx.AI.FSM;

namespace Thuleanx.PortalKnight {
	[RequireComponent(typeof(ShadowEnemy))]
	public class ShadowEnemyStateMachine : StateMachine<ShadowEnemy> {
		public override void Construct() {
			ConstructMachine(GetComponent<ShadowEnemy>(), 
				Enum.GetNames(typeof(ShadowEnemy.State)).Length, 
				(int) ShadowEnemy.State.Aggro, false);

			AssignState((int) ShadowEnemy.State.Aggro, new ShadowEnemy.ShadowAggroState());
			AssignState((int) ShadowEnemy.State.Attack, new ShadowEnemy.ShadowAttackState());
			AssignState((int) ShadowEnemy.State.Dead, new ShadowEnemy.ShadowDeathState());
			AssignState((int) ShadowEnemy.State.Special, new ShadowEnemy.ShadowShootState());
		}
	}
}