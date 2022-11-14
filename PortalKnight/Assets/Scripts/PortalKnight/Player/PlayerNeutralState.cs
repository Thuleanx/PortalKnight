using UnityEngine;

using Thuleanx.AI.FSM;
using Thuleanx.Utils;

namespace Thuleanx.PortalKnight {
	public partial class Player {
		public class PlayerNeutralState : State<Player> {
			bool OnDash(Player player) => player.StateMachine.TrySetState((int) Player.State.Dash);
			bool OnAttack(Player player) => player.StateMachine.TrySetState((int) Player.State.Attack);
			bool OnSpecial(Player player) => player.StateMachine.TrySetState((int) Player.State.Special);

			public override void Begin(Player agent) {
				agent.Input.ActionHandler[(int) ActionType.Dash] = OnDash;
				agent.Input.ActionHandler[(int) ActionType.Attack] = OnAttack;
				agent.Input.ActionHandler[(int) ActionType.Special] = OnSpecial;
				// agent.Input.ActionHandler[(int) ActionType.Shoot] = OnShoot;
			}

			public override void End(Player agent) {
				// unbind event handlers
				agent.Input.ActionHandler[(int) ActionType.Dash] = null;
				agent.Input.ActionHandler[(int) ActionType.Attack] = null;
				// agent.Input.ActionHandler[(int) ActionType.Shoot] = null;
				agent.Input.ActionHandler[(int) ActionType.Special] = null;
			}

			public override int Update(Player player) {
				Vector3 desiredVelocity = player.Velocity;

				if (player.Input.movement.sqrMagnitude >= 0.01f)
					desiredVelocity = player.Input.MovementToWorldDir(player.Input.movement) * player.speed;
				else desiredVelocity = Vector3.zero;

				player.Velocity = Mathx.Damp(Vector3.Lerp, player.Velocity, desiredVelocity, 
					(player.Velocity.sqrMagnitude > desiredVelocity.sqrMagnitude) ? player.deccelerationAlpha : player.accelerationAlpha, Time.deltaTime);

				if (player.Velocity != Vector3.zero) 
					player.TurnToFace(player.Velocity, player.turnSpeed);

				return -1;
			}

			bool OnShoot(Player player) {
				SpawnManaOrb(player);
				return true;
			}

			// you can shoot mana orbs in this state. Not sure where they spawn out of
			void SpawnManaOrb(Player player) {
				GameObject obj = player.manaOrbPool.Borrow(player.gameObject.scene);
				obj.transform.position = player.manaOrbFiringSource.transform.position;
				Vector3 targetPos = player.Input.mousePosWS;

				Puppet closestTarget = null;
				foreach (var target in GameObject.FindGameObjectsWithTag("Enemy")) {
					Puppet pup = target.GetComponent<Puppet>();
					if (pup && !pup.IsDead && (closestTarget == null || (target.transform.position - targetPos).sqrMagnitude < 
						(closestTarget.transform.position - targetPos).sqrMagnitude))
							closestTarget = pup;
				}
				if (closestTarget && (closestTarget.transform.position - targetPos).sqrMagnitude > player.manaOrbMouseRange * player.manaOrbMouseRange)
					closestTarget = null;
				if (closestTarget == null) {
					Vector2 randomCircle = Random.insideUnitCircle;
					targetPos = player.transform.position + 
						player.manaOrbMouseRange * new Vector3(randomCircle.x, 0, randomCircle.y);
				}

				ManaOrb orb = obj.GetComponent<ManaOrb>();
				orb.Initialize(player.manaOrbFiringSource.transform.forward, closestTarget, Vector3.up * 0.75f, targetPos, player.manaOrbDamage);
			}
		}
	}
}