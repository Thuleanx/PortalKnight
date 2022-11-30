using UnityEngine;
using UnityEngine.Events;
using Thuleanx.Utils;
using Thuleanx.PortalKnight.Interactions;
using NaughtyAttributes;
using Yarn.Unity;

namespace Thuleanx.PortalKnight.Mapping {
	[RequireComponent(typeof(Collider))]
	public class Passage : Interactible {
		public SceneTransitioner Transitioner { get; private set; }
		[Required] public Passage Link;
		public UnityEvent onEnable;

		protected override void Interact() {
			FindObjectOfType<PortalTransitioner>().Transition(this);
		}

		[YarnCommand("enable2")]
		void yarn_enable() {
			Debug.Log("HI");
			onEnable?.Invoke();
		} 
	}
}