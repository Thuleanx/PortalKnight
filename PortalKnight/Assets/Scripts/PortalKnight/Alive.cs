using UnityEngine;

namespace Thuleanx.PortalKnight {
	public abstract class Alive : Movable {
		protected Puppet Puppet {get; private set; }
		public virtual void Awake() {
			Puppet = GetComponent<Puppet>();
			Puppet.OnDeath.AddListener(OnDeath);
		}
		protected abstract void OnDeath(Puppet puppet);
	}
}