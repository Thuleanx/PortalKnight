using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Thuleanx.Utils;

namespace Thuleanx.PortalKnight.Mapping {
	public class PortalTransitioner : SceneTransitioner {
		bool transitioning = false;

		public override bool Transition(SceneReference TargetScene) {
			if (transitioning) return false;
			transitioning = true;
			StartCoroutine(iTransition(TargetScene));
			return true;
		}

		IEnumerator iTransition(SceneReference TargetScene) {
			yield return null;
			App.instance.RequestLoad(TargetScene.SceneName);
			transitioning = false;
		}
	}
}