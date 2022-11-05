using System;
using UnityEngine;
using NaughtyAttributes;
using Thuleanx.AI.FSM;

namespace Thuleanx.PortalKnight {
	[RequireComponent(typeof(Player))]
	public class PlayerStateMachine : StateMachine<Player> {
		// no code is needed here, really
		// needed because Unity is fussy

		private void Awake() {
			ConstructMachine(GetComponent<Player>(), Enum.GetNames(typeof(Player.State)).Length, (int) Player.State.Neutral, false);

			AssignState((int) Player.State.Neutral, new Player.PlayerNeutralState());
			AssignState((int) Player.State.Dash, new Player.PlayerDashState());

			Init();
		}
	}
}