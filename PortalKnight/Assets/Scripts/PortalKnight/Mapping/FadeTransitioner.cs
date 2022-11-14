using Thuleanx.PortalKnight.Interactions;
using UnityEngine;
using System.Collections;
using DG.Tweening;

using NaughtyAttributes;

namespace Thuleanx.PortalKnight.Mapping {
	public class FadeTransitioner : SceneTransitioner {
		[SerializeField] float fadeOutDuration;
		[SerializeField] float fadeInDuration;
		[SerializeField] CanvasGroup canvasGroup;

		[Button]
		public void __TestTransition() {
			Transition("scn_official_end");
		}

		public override bool Transition(string sceneName, Interactible Triggerer = null) {
			if (transitioning) return false;
			transitioning = true;
			StartCoroutine(iTransition(sceneName));
			return true;
		}

		IEnumerator iTransition(string sceneName) {
			transitioning = false;

			canvasGroup.DOFade(1, fadeOutDuration);
			yield return new WaitForSeconds(1);

			App.instance.RequestLoad(sceneName);

			canvasGroup.DOFade(1, fadeInDuration);
			yield return new WaitForSeconds(1);
		}

	}
}