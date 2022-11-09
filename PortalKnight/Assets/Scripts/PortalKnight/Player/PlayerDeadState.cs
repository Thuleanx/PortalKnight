using UnityEngine;
using System.Collections;

using Thuleanx.AI.FSM;
using Thuleanx.Utils;

namespace Thuleanx.PortalKnight {
	public partial class Player {
		public class PlayerDeadState : GenericDeadState<Player> {
			public override IEnumerator Coroutine(Player player) {
				yield return new WaitForSeconds(3);
				App.instance.RequestReload();
			}
		}
	}
}