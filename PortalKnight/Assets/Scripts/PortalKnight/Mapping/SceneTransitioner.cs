using UnityEngine;
using Thuleanx.Utils;

namespace Thuleanx.PortalKnight.Mapping {
	public abstract class SceneTransitioner : MonoBehaviour {
		public abstract bool Transition(SceneReference TargetScene);
	}
}