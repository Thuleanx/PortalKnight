using UnityEngine;

namespace Thuleanx.PortalKnight.Interactions {
	public abstract class Interactible : MonoBehaviour {
		protected abstract void Interact();
		void OnTriggerStay(Collider other) {
			if (other.gameObject.tag == "Player") Interact();
		}
	}
}