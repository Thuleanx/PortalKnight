using UnityEngine;
using Thuleanx.Utils;

namespace Thuleanx.PortalKnight.Effects {
	[RequireComponent(typeof(Renderer))]
	public class FlashEffect : MonoBehaviour {
		[SerializeField] float flashDuration;
		public Renderer Renderer {get; private set;}
		Timer Flashing;

		public void Flash() => Flashing = flashDuration;

		void Awake() {
			Renderer = GetComponent<Renderer>();
		}

		void LateUpdate() {
			Renderer.material.SetFloat("_Flash", Flashing ? 1 : 0);
		}
	}
}