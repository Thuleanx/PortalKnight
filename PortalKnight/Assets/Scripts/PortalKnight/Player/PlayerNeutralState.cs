using UnityEngine;

using Thuleanx.AI.FSM;
using Thuleanx.Utils;

namespace Thuleanx.PortalKnight {
	public partial class Player {
		public class PlayerNeutralState : State<Player> {
			bool OnDash() => stateMachine.TrySetState((int) Player.State.Dash);
			bool OnAttack() => stateMachine.TrySetState((int) Player.State.Attack);

			public override void Begin(Player agent) {
				agent.ActionHandler[(int) ActionType.Dash] = OnDash;
				agent.ActionHandler[(int) ActionType.Attack] = OnAttack;
			}

			public override void End(Player agent) {
				agent.ActionHandler[(int) ActionType.Dash] = null;
				agent.ActionHandler[(int) ActionType.Attack] = null;
			}

			public override int Update(Player player) {
				Vector3 desiredVelocity = player.Velocity;

				if (player.movement.sqrMagnitude >= 0.01f)
					desiredVelocity = player.InputMovementToWorldDir(player.movement) * player.speed;
				else desiredVelocity = Vector3.zero;

				player.Velocity = Mathx.Damp(Vector3.Lerp, player.Velocity, desiredVelocity, 
					(player.Velocity.sqrMagnitude > desiredVelocity.sqrMagnitude) ? player.deccelerationAlpha : player.accelerationAlpha, Time.deltaTime);

				if (player.Velocity != Vector3.zero) {
					// turn to face velocity
					player.transform.rotation = Quaternion.LookRotation(player.Velocity, Vector3.up);
				}

				return -1;
			}
		}
	}
}