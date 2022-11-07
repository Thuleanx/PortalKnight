using UnityEngine;

using Thuleanx.Combat3D;

namespace Thuleanx.PortalKnight {
	[RequireComponent(typeof(Movable))]
	public class Puppet : MonoBehaviour {
		public Status 	Status {get; private set; }
		public Movable 	Movable {get; private set; }

		void Awake() {
			Status = new Status(){
				MaxHealth = 10
			};
			Movable = GetComponent<Movable>();
		}

		void Start() {
			foreach (var hurtbox in GetComponentsInChildren<Hurtbox3D>())
				hurtbox.OnHit.AddListener(ProcessHit);
		}

		void ProcessHit(Hit3D hit) {
			Movable.ApplyKnockback(hit.knockbackAmount * hit.hitDir);
			Status.Health -= hit.damage;
		}
	}
}