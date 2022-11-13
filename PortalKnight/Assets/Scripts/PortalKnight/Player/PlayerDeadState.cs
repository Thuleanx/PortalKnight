using UnityEngine;
using System.Collections;

using Thuleanx.AI.FSM;
using Thuleanx.Utils;
using Thuleanx.PortalKnight.Mapping;

namespace Thuleanx.PortalKnight {
	public partial class Player {
		public class PlayerDeadState : GenericDeadState<Player> {
			public override IEnumerator Coroutine(Player player) {
				// umm you wanna tell this 
				FindObjectOfType<DeathTransitioner>().Transition(App.instance.GetActiveScene());
				yield return null;
			}
		}
	}
}