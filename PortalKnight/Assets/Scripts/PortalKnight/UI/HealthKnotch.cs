using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using NaughtyAttributes;

namespace Thuleanx.PortalKnight.UI {
	[RequireComponent(typeof(Image))]
	public class HealthKnotch : MonoBehaviour {
		public bool Filled {get; private set; }
		public Image Image {get; private set;}
		[Space, SerializeField] UnityEvent OnFill;
		[Space, SerializeField] UnityEvent OnEmpty;
		[SerializeField, Required] Sprite filledSprite;
		[SerializeField, Required] Sprite emptySprite;

		void Awake() {
			Image = GetComponent<Image>();
		}

		void onFill() {
			OnFill?.Invoke();
			Image.sprite = filledSprite;
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