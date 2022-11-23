using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

namespace Thuleanx.PortalKnight {
	public abstract class Animated : Alive {
		[field:Header("Animated")]
		[field:SerializeField] protected Animator Anim {get; private set; }
		[SerializeField, ReorderableList] List<UnityEvent> animationEvents = new List<UnityEvent>();

		bool waitingForTrigger = false;
		public void _AnimTrigger() {
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
		public void _TriggerAnimatedEvent(AnimationEvent animEvent) {
			if (animEvent.animatorClipInfo.weight > 0.8f) 
				animationEvents[animEvent.intParameter]?.Invoke();
		}
	}
}