using UnityEngine;
using Thuleanx.Utils;

namespace Thuleanx.PortalKnight.Mapping {
	[RequireComponent(typeof(Collider))]
	public class Passage : MonoBehaviour {
		[SerializeField] SceneReference Endpoint;

		public void Transition() {
			FindObjectOfType<SceneTransitioner>().Transition(Endpoint);
		}

		void OnTriggerEnter(Collider other) {
			Transition();
		}
	}
}