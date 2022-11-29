using UnityEngine;
using Thuleanx.Utils;

namespace Thuleanx.PortalKnight.Mapping {
	public class OfficialTransitioner : MonoBehaviour {
		[SerializeField] SceneReference nxtScene;

		public void Go() => GameObject.FindObjectOfType<FadeTransitioner>()?.Transition(nxtScene.SceneName);
	}
}