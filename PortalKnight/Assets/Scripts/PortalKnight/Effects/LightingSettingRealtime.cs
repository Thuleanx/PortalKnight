using UnityEngine;

namespace Thuleanx.PortalKnight.Effects {
	[ExecuteAlways]
	public class LightingSettingRealtime : MonoBehaviour {
		[SerializeField, ColorUsage(true, true)] Color lightColor;
		void LateUpdate() {
			RenderSettings.ambientLight = lightColor;
		}
	}
}