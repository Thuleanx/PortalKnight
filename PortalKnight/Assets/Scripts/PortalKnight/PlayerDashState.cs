using UnityEngine;

using Thuleanx.AI.FSM;
using System.Collections;

namespace Thuleanx.PortalKnight {
	public class PlayerDashState : State<Player> {
		Vector2 DashDirection;

		public override void Begin(Player agent) {
			DashDirection = agent.LastNonZeroMovement;
			agent.Velocity = DashDirection;
		}

		public override void End(Player agent) {
			agent.Drag = 0;
		}

		public override IEnumerator Coroutine(Player agent) {
			yield return new WaitForSeconds(2);
		}
	}
}