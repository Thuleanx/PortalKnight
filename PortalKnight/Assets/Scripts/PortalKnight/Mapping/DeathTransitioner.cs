using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using NaughtyAttributes;
using DG.Tweening;

using Thuleanx.PortalKnight.Interactions;
using Thuleanx.Utils;

namespace Thuleanx.PortalKnight.Mapping {
	public class DeathTransitioner : SceneTransitioner {
		[SerializeField] Image BlockoutImage;

		[SerializeField, MinMaxSlider(0, 5)] Vector2 fadeOutDuration;
		[SerializeField, Range(0, 5)] float focusWait;
		[SerializeField, Range(0, 5)] float fadeInDuration;
		[SerializeField] float maxRange;
		[SerializeField] float minRange;
		[SerializeField] float focusRange;
		[SerializeField] Ease easeFocus;
		[SerializeField] Ease easeFadeIn;
		[SerializeField] Ease easeOut;
		Tween tween;

		public override bool Transition(string sceneName, Interactible Triggerer = null) {
			if (transitioning) return false;
			transitioning = true;
			ResetAll();
			StartCoroutine(iTransition(sceneName));
			return true;
		}

		public bool TriggerFadeIn() {
			if (transitioning) return false;
			transitioning = true;
			ResetAll();
			StartCoroutine(iFadein());
			return true;
		}


		IEnumerator iTransition(string sceneName) {
			SetEnabbed(true);
			tween = DOVirtual.Float(maxRange, focusRange, fadeOutDuration.x, (x) => SetRange(x)).SetEase(easeFocus);
			yield return new WaitForSeconds(fadeOutDuration.x + focusWait);
			tween?.Kill();
			tween = DOVirtual.Float(focusRange, minRange, fadeOutDuration.y - fadeOutDuration.x, (x) => SetRange(x)).SetEase(easeOut);
			yield return new WaitForSeconds(fadeOutDuration.y - fadeOutDuration.x);
			App.instance.RequestLoad(sceneName);
			transitioning = false;	
		}

		IEnumerator iFadein() {
			SetEnabbed(true);
			tween = DOVirtual.Float(minRange, maxRange, fadeInDuration, (x) => SetRange(x)).SetEase(easeFadeIn);
			yield return new WaitForSeconds(fadeInDuration);
			transitioning = false;
			SetEnabbed(false);
		}

		void Start() {
			TriggerFadeIn();
		}

		void ResetAll() {
			StopAllCoroutines(); // if fading in, well stop
			tween?.Kill();
		}

		void SetRange(float value) => BlockoutImage.material.SetFloat("_Radius", value);
		void SetEnabbed(bool value) => BlockoutImage.material.SetFloat("_Enabled", value ? 1 : 0);
	}
}