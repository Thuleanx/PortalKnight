using UnityEngine;

using Thuleanx.AI.FSM;
using Thuleanx.Utils;

namespace Thuleanx.PortalKnight {
	public class PlayerNeutralState : State<Player> {
		public override int Update(Player agent) {
			Vector3 inputDir = new Vector3(
				agent.Movement.x, 0, agent.Movement.y
			).normalized;

			Vector3 desiredVelocity = agent.Velocity;
			if (inputDir.sqrMagnitude >= 0.01f) {
				Vector3 moveDir = Quaternion.Euler(
					0, Camera.main.transform.eulerAngles.y, 0f
				) * inputDir;
				desiredVelocity = moveDir * agent.Speed;
			} else desiredVelocity = Vector3.zero;

			// no acceleration

			// noAcceleration: {
			// 	agent.Velocity = desiredVelocity;
			// }

			agent.Velocity = Mathx.Damp(Vector3.Lerp, agent.Velocity, desiredVelocity, 
				(agent.Velocity.sqrMagnitude > desiredVelocity.sqrMagnitude) ? agent.DeccelerationAlpha : agent.AccelerationAlpha, Time.deltaTime);

			return -1;
		}
	}
}