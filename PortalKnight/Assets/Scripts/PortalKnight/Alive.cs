using UnityEngine;

namespace Thuleanx.PortalKnight {
	public abstract class Alive : Movable {
		public Puppet Puppet {get; private set; }
		public virtual void Awake() {
			Puppet = GetComponent<Puppet>();
		}
		void Start() {
			Puppet.OnDeath.AddListener(OnDeath);
		}
		protected abstract void OnDeath(Puppet puppet);
		public bool IsDead => Puppet.Status.IsDead;
	}
}