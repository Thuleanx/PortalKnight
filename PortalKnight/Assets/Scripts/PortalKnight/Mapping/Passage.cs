using UnityEngine;
using Thuleanx.Utils;
using Thuleanx.PortalKnight.Interactions;

namespace Thuleanx.PortalKnight.Mapping {
	[RequireComponent(typeof(Collider))]
	public class Passage : Interactible {
		public SceneTransitioner Transitioner { get; private set; }
		public int ID;

		[SerializeField] SceneReference Endpoint;

		protected override void Interact() {
			FindObjectOfType<SceneTransitioner>().Transition(Endpoint, this);
		}
	}
}