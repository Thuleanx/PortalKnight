using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;

namespace Thuleanx.PrettyPatterns {
	class Bubble : MonoBehaviour {
		public Scene scene;
		public UnityAction<Bubble> DisposalRequested = null;
		Coroutine collectCoroutine;

		public void Pop() => DisposalRequested?.Invoke(this);

		private void OnEnable() {
			if (collectCoroutine != null) App.instance.StopCoroutine(collectCoroutine);
		}

		void OnDisable() {
			Pop();
			// collectCoroutine = App.instance.StartCoroutine(CollectsAfterOneFrame());
		}

		public IEnumerator CollectsAfterOneFrame() {
			yield return null;
			Pop();
		}
	}
}