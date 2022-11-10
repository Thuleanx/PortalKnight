using UnityEngine;

namespace Thuleanx.PortalKnight {
	public partial class Player {
		[RequireComponent(typeof(Player))]
		public class PlayerInputSystem : MonoBehaviour {
			public Player Player {get; private set; }


		}
	}
}