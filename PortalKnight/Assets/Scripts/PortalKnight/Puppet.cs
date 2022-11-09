using UnityEngine;
using UnityEngine.Events;

using Thuleanx.Combat3D;

namespace Thuleanx.PortalKnight {
	[RequireComponent(typeof(Alive))]
	public class Puppet : MonoBehaviour {
		public Status 	Status;
		public Alive Entity {get; private set; }

		[Space]
		public UnityEvent<Puppet> OnDeath;
		public UnityEvent<Puppet> OnHit;


		public int InitialMaxHealth;

		void Awake() {
			Status = new Status(){
				MaxHealth = 10
			};
			Entity = GetComponent<Alive>();
		}

		void OnEnable() {
			Status.MaxHealth = InitialMaxHealth;
			Status.Health = Status.MaxHealth;
		}

		void Start() {
			foreach (var hurtbox in GetComponentsInChildren<Hurtbox3D>())
				hurtbox.OnHit.AddListener(ProcessHit);
		}

		void ProcessHit(Hit3D hit) {
			if (!Status.IsDead) {
				Entity.ApplyKnockback(hit.knockbackAmount * hit.hitDir);
				Status.Health -= hit.damage;
				if (Status.Health == 0) 	OnDeath?.Invoke(this);
				else 						OnHit?.Invoke(this);
			}
		}
	}
}