using UnityEngine;
using Thuleanx.Utils;
using Thuleanx.PortalKnight.Interactions;

namespace Thuleanx.PortalKnight.Mapping {
	public abstract class SceneTransitioner : MonoBehaviour {
		public bool transitioning;
		public abstract bool Transition(SceneReference TargetScene, Interactible Triggerer = null);
	}
}