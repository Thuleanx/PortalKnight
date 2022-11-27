using UnityEngine;
using Yarn.Unity;

namespace Thuleanx.PortalKnight.Dialogue {
	public class DialogueHitbox : MonoBehaviour {
		[SerializeField] bool triggerOnce;
		[SerializeField] string node;

		void OnTriggerEnter(Collider other) {
			if (other.tag == "Player") {
				GameObject.FindObjectOfType<DialogueRunner>()?.StartDialogue(node);
				if (triggerOnce) Destroy(gameObject);
			}
		}
	}
}