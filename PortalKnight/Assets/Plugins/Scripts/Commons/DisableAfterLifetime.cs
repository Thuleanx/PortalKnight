using UnityEngine;
using Thuleanx.Utils;

namespace Thuleanx.Commons {
	public class DisableAfterLifetime : MonoBehaviour {
		[SerializeField, Range(0,30)] float lifetimeSeconds;
		Timer alive;

		private void OnEnable() {
			alive = lifetimeSeconds;
		}

		private void Update() {
			if (!alive) gameObject.SetActive(false);
		}
	}
}