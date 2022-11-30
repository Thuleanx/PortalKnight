using UnityEngine;

namespace Thuleanx.PortalKnight {
	public class PKDirector : MonoBehaviour {
		public void BeginWrap() {
			FindObjectOfType<Player>()?.Puppet?.GiveIframes(1000); // make player invulnerable
		}

		public void Wrap() {
			foreach (var obj in FindObjectsOfType<ShadowEnemy>())
				obj.Puppet.SpontaneousCombustion();
			foreach (var obj in FindObjectsOfType<ShadowProjectile>())
				obj.gameObject.SetActive(false);
			foreach (var obj in FindObjectsOfType<ShadowBlob>())
				obj.CauseExpire();
		}
	}
}