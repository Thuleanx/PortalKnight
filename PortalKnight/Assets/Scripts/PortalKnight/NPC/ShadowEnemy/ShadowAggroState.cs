using UnityEngine;
using Thuleanx.AI.FSM;
using Thuleanx.Utils;

namespace Thuleanx.PortalKnight {
	public partial class ShadowEnemy {
		// walk towards player, update destination every few frames
		// once in range for melee, switch to attack state
		// if attack is on cooldown, doesn't do anything

		public class ShadowAggroState : State<ShadowEnemy> {
			float timeLastNavMeshUpdate;

			public override void Begin(ShadowEnemy monster) {
				UpdateNavMeshTarget(monster);
			}

			public override int Transition(ShadowEnemy monster) {
				if (InAttackRange(monster) && stateMachine.CanEnter((int) State.Attack)) 
					return (int) State.Attack;
				return -1;
			}

			public override int Update(ShadowEnemy monster) {
				// every x frames, update destination
				if (Time.time - timeLastNavMeshUpdate >= monster.navMeshUpdateInterval)
					UpdateNavMeshTarget(monster);

				Vector3 desiredVelocity = monster.NavAgent.desiredVelocity;

				monster.Velocity = Mathx.Damp(Vector3.Lerp, monster.Velocity, desiredVelocity, 
					(desiredVelocity.sqrMagnitude >= monster.Velocity.sqrMagnitude ? 
						monster.accelerationAlpha : monster.deccelerationAlpha), Time.deltaTime);
				monster.TurnToFace(monster.Velocity);

				return -1;
			}

			bool InAttackRange(ShadowEnemy monster) 
				=> (monster.transform.position - monster.player.transform.position).sqrMagnitude <= monster.attackRange * monster.attackRange;

			void UpdateNavMeshTarget(ShadowEnemy monster) {
				monster.NavAgent.SetDestination(monster.player.transform.position);
				timeLastNavMeshUpdate = Time.time;
			}
		}
	}
}