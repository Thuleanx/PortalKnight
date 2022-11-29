using UnityEngine;
using Yarn.Unity;

namespace Thuleanx.PortalKnight.UI {
	public class PlayerUICanvas : MonoBehaviour {
		[SerializeField] GameObject UIHub;

		[YarnCommand("enable")]
		void yarn_Enable() => UIHub.SetActive(true);
	}
}