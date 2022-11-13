using UnityEngine;
using Thuleanx.Utils;
using Thuleanx.PortalKnight.Interactions;
using NaughtyAttributes;

namespace Thuleanx.PortalKnight.Mapping {
	[RequireComponent(typeof(Collider))]
	public class Passage : Interactible {
		public SceneTransitioner Transitioner { get; private set; }
		[Required] public Passage Link;

		protected override void Interact() {
			FindObjectOfType<PortalTransitioner>().Transition(this);
		}

	}
}