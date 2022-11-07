using UnityEngine;
using UnityEngine.Events;

using Thuleanx.Combat3D;

namespace Thuleanx.PortalKnight {
	[RequireComponent(typeof(Movable))]
	public class Puppet : MonoBehaviour {
		public Status 	Status;
		public Movable 	Movable {get; private set; }

		[Space]
		public UnityEvent OnDeath;
		public UnityEvent OnHit;


		public int InitialMaxHealth;

		void Awake() {
			Status = new Status(){
				MaxHealth = 10
			};
			Movable = GetComponent<Movable>();
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
				Movable.ApplyKnockback(hit.knockbackAmount * hit.hitDir);
				Status.Health -= hit.damage;
				if (Status.Health == 0) 	OnDeath?.Invoke();
				else 						OnHit?.Invoke();
			}
		}
	}
}