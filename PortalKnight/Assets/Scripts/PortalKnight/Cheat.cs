#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

using Thuleanx.PortalKnight.UI;

namespace Thuleanx.PortalKnight {
	public static class Cheat {
		[MenuItem("Thuleanx/Player/HealFull")]
		static void HealToFull() {
			Player player = GameObject.FindObjectOfType<Player>();
			player.Status.Health = player.Status.MaxHealth;
		}

	}
}
#endif