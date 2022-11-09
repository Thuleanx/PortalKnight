using UnityEngine;
using UnityEngine.UI;

namespace Thuleanx.PortalKnight.UI {
	[RequireComponent(typeof(RectTransform))]
	[RequireComponent(typeof(Slider))]
	public class ManaUI : MonoBehaviour {
		public Slider Slider {get; private set; }
		public Player Player {get; private set; }

		void Awake() {
			Player = FindObjectOfType<Player>();
			Slider = GetComponent<Slider>();
		}

		void Update() {
			Slider.value = Player.Mana / Player.MaxMana;
		}
	}
}