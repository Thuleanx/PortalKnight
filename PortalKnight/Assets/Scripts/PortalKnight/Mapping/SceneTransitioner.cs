using UnityEngine;
using Thuleanx.Utils;
using Thuleanx.PortalKnight.Interactions;
using NaughtyAttributes;

namespace Thuleanx.PortalKnight.Mapping {
	public abstract class SceneTransitioner : MonoBehaviour {
		public bool transitioning {get; protected set;}
		public bool Transition(SceneReference TargetScene, Interactible Triggerer = null) => Transition(TargetScene.SceneName, Triggerer);
		public abstract bool Transition(string sceneName, Interactible Triggerer = null);
	}
}