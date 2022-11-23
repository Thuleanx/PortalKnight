using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;

namespace Thuleanx.PortalKnight.UI {
	public class UIShine : MonoBehaviour {
		[SerializeField, Required] RectTransform shineWipe;
		// [SerializeField, Required] RectTransform shineCircle;
		[SerializeField, Range(0,1)] float shineWipeDuration;
		// [SerializeField, Range(0,1)] float shineCircleDuration;
		[SerializeField, Range(0,3)] float shineRestDuration;
		[SerializeField] float shineWipeDistance;

		Sequence shineSequence;

		void OnEnable() {
			shineSequence = DOTween.Sequence();
			shineSequence.AppendCallback(() => {
				// shineCircle.localScale = Vector3.zero;
				shineWipe.anchoredPosition = Vector3.right * -shineWipeDistance;
			});
			shineSequence.Append(shineWipe.DOAnchorPos(Vector3.right * shineWipeDistance, shineWipeDuration));
			// shineSequence.AppendCallback(() => shineCircle.localScale = Vector3.one);
			// shineSequence.Append(shineCircle.DOScale(Vector3.right, shineCircleDuration));
			shineSequence.AppendInterval(shineRestDuration);
			shineSequence.SetLoops(-1);
		}

		void OnDisable() {
			shineSequence?.Kill();
		}
	}
}