using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using NaughtyAttributes;
using DG.Tweening;

namespace Thuleanx.PortalKnight.UI {
	[RequireComponent(typeof(Image))]
	[RequireComponent(typeof(RectTransform))]
	public class HealthKnotch : MonoBehaviour {
		public bool Filled {get; private set; }
		[field:SerializeField] public Image Image {get; private set;}
		public RectTransform rectTransform {get; private set; }
		[Space, SerializeField] UnityEvent OnFill;
		[Space, SerializeField] UnityEvent OnEmpty;

		[SerializeField, Required] Sprite filledSprite;
		[SerializeField, Required] Sprite emptySprite;

		[Header("Shake on Health Gain")]
		[SerializeField, Range(0,1)] float shakeDurationGained = 0.2f;
		[SerializeField, Range(0,100)] float shakeStrengthGained = 25;
		[SerializeField, Range(0,100)] int shakeFrequencyGained = 30;

		[Header("Shake on Health Lost")]
		[SerializeField, Range(0,1)] float shakeDurationLost = 0.2f;
		[SerializeField, Range(0,100)] float shakeStrengthLost = 25;
		[SerializeField, Range(0,300)] int shakeFrequencyLost = 30;

		void Awake() {
			if (!Image) Image = GetComponent<Image>();
			rectTransform = GetComponent<RectTransform>();
		}

		void OnEnable() => Filled = true;

		void onFill() {
			OnFill?.Invoke();
			Image.sprite = filledSprite;
		}

		void onEmpty() {
			OnEmpty?.Invoke();
			Image.sprite = emptySprite;
			rectTransform.DOShakeAnchorPos(shakeDurationGained, shakeStrengthGained, shakeFrequencyGained);
			rectTransform.DOShakeRotation(shakeDurationLost, shakeStrengthLost, shakeFrequencyLost);
		}

		public void SetFilled(bool filled) {
			if (Filled ^ filled) {
				if (!Filled) 	onFill();
				else 			onEmpty();
				Filled = filled;
			}
		}
	}
}