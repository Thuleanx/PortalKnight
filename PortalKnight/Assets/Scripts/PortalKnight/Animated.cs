using System.Collections;
using UnityEngine;

namespace Thuleanx.PortalKnight {
	public abstract class Animated : Alive {
		bool waitingForTrigger = false;
		public void _AnimTrigger() => waitingForTrigger = false;

		protected IEnumerator iWaitForTrigger() {
			waitingForTrigger = true;
			while (waitingForTrigger) yield return null;
		}
		protected void StartAnimationWait() => waitingForTrigger = true;
		protected bool WaitingForTrigger => waitingForTrigger;
	}
}