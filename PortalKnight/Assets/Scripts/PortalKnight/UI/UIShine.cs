using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;

namespace Thuleanx.PortalKnight.UI {
	public class UIShine : MonoBehaviour {
		[SerializeField, Required] RectTransform shineWipe;
		[SerializeField, Required] RectTransform shineCircle;
		[SerializeField, Range(0,1)] float shineWipeDuration;
		[SerializeField, Range(0,1)] float shineCircleDuration;
		[SerializeField] float shineDuration;
		[SerializeField] float shineWipeDistance;

		Sequence shineSequence;

		void OnEnable() {
			// shineSequence = DOTween.Sequence();
			// shineSequence.AppendCallback(() => {
			// 	shineCircle.localScale = Vector3.zero;
			// });
			// shineSequence.Append(shineWipe.anchoredPosition(Vector3.right * shineWipeDistance));
			// shineSequence.AppendCallback(() => shineCircle.localScale = Vector3.one);
			// shineSequence.Append(shineCircle.DOScale(Vector3.zero, shineCircleDuration));
			// shineSequence.SetLoops(-1);
		}

		void OnDisable() {
			shineSequence?.Kill();
		}
	}
}