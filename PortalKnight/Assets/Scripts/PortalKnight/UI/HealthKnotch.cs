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
		[SerializeField, Range(0,1)] float shakeDuration = 0.2f;
		[SerializeField, Range(0,100)] float shakeStrength = 25;
		[SerializeField, Range(0,100)] int shakeFrequency = 30;

		void Awake() {
			if (!Image) Image = GetComponent<Image>();
			rectTransform = GetComponent<RectTransform>();
		}

		void OnEnable() => Filled = true;

		void onFill() {
			OnFill?.Invoke();
			Image.sprite = filledSprite;
			rectTransform.DOShakeAnchorPos(shakeDuration, shakeStrength, shakeFrequency);
		}

		void onEmpty() {
			OnEmpty?.Invoke();
			Image.sprite = emptySprite;
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