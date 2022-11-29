using UnityEngine;
using DG.Tweening;

namespace Thuleanx.PortalKnight.UI {
	[RequireComponent(typeof(CanvasGroup))]
	public class FadeOnEnable : MonoBehaviour {
		public CanvasGroup canvasGroup {get; private set; }
		
		[SerializeField] float duration;
		[SerializeField] Ease ease = Ease.Linear;
		Tween tween;

		void Awake() {
			canvasGroup = GetComponent<CanvasGroup>();
		}

		void OnEnable() {
			canvasGroup.alpha = 0;
			tween = canvasGroup.DOFade(1, duration).SetEase(ease);
		}
	}
}