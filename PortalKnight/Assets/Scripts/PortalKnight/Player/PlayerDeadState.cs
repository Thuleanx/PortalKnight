using UnityEngine;
using System.Collections;

using Thuleanx.AI.FSM;
using Thuleanx.Utils;
using Thuleanx.PortalKnight.Mapping;
using Thuleanx.PortalKnight.Dialogue;

namespace Thuleanx.PortalKnight {
	public partial class Player {
		public class PlayerDeadState : GenericDeadState<Player> {
			public override void Begin(Player player) {
				base.Begin(player);
				FindObjectOfType<VariableStorage>()?.IncrementDeath();
			}
			public override int Update(Player player) {
				player.ResetMovements();
				return base.Update(player);
			}
			public override IEnumerator Coroutine(Player player) {
				FindObjectOfType<DeathTransitioner>().Transition(App.instance.GetActiveScene());
				yield return null;
			}

		}
	}
}