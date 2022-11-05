using UnityEngine;

using Thuleanx.AI.FSM;
using Thuleanx.Utils;

namespace Thuleanx.PortalKnight {
	public partial class Player {
		public class PlayerNeutralState : State<Player> {
			bool OnDash() => stateMachine.TrySetState((int) Player.State.Dash);

			public override void Begin(Player agent) {
				agent.ActionHandler[(int) ActionType.Dash] = OnDash;
			}

			public override void End(Player agent) {
				agent.ActionHandler[(int) ActionType.Dash] = null;
			}

			public override int Update(Player player) {
				Vector3 desiredVelocity = player.Velocity;

				if (player.movement.sqrMagnitude >= 0.01f) {
					desiredVelocity = player.InputMovementToWorldDir(player.movement) * player.speed;
				} else desiredVelocity = Vector3.zero;

				// no acceleration

				// noAcceleration: {
				// 	agent.Velocity = desiredVelocity;
				// }

				player.Velocity = Mathx.Damp(Vector3.Lerp, player.Velocity, desiredVelocity, 
					(player.Velocity.sqrMagnitude > desiredVelocity.sqrMagnitude) ? player.deccelerationAlpha : player.accelerationAlpha, Time.deltaTime);

				return -1;
			}
		}
	}
}