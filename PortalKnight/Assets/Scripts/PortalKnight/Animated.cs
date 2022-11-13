using System.Collections;
using UnityEngine;

namespace Thuleanx.PortalKnight {
	public abstract class Animated : Alive {
		[field:Header("Animated")]
		[field:SerializeField] protected Animator Anim {get; private set; }

		bool waitingForTrigger = false;
		public void _AnimTrigger() {
			Debug.Log("TRIGGERED");
			waitingForTrigger = false;
		}

		public override void Awake() {
			base.Awake();
			if (!Anim) Anim = GetComponent<Animator>();
		}

		protected IEnumerator iWaitForTrigger() {
			waitingForTrigger = true;
			while (waitingForTrigger) yield return null;
		}
		protected void StartAnimationWait() => waitingForTrigger = true;
		protected bool WaitingForTrigger => waitingForTrigger;
	}
}