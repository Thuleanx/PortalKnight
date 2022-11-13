using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

using Thuleanx.Combat3D;

namespace Thuleanx.PortalKnight {
	[RequireComponent(typeof(Alive))]
	public class Puppet : MonoBehaviour {
		[ReadOnly] public Status Status;
		public Alive Entity {get; private set; }
		public bool IsDead => Status.IsDead;

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
			foreach (var hurtbox in GetComponentsInChildren<Hurtbox3D>()) 
				hurtbox.SetVulnerable();
		}

		void Start() {
			foreach (var hurtbox in GetComponentsInChildren<Hurtbox3D>())
				hurtbox.OnHit.AddListener(ProcessHit);
		}

		void onDeath() {
			foreach (var hurtbox in GetComponentsInChildren<Hurtbox3D>()) 
				hurtbox.SetInvicible();
			OnDeath?.Invoke(this);
		}

		void ProcessHit(Hit3D hit) {
			if (!Status.IsDead) {
				Entity.ApplyKnockback(hit.knockbackAmount * hit.hitDir);
				Status.Health -= hit.damage;
				if (Status.Health == 0) 	onDeath();
				else 						OnHit?.Invoke(this);
			}
		}
	}
}