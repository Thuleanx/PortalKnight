using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

using Thuleanx.Combat3D;
using Thuleanx.Utils;

namespace Thuleanx.PortalKnight {
	[RequireComponent(typeof(Alive))]
	public class Puppet : MonoBehaviour {
		[ReadOnly] public Status Status;
		public Alive Entity {get; private set; }
		public bool IsDead => Status.IsDead;
		List<Hurtbox3D> hurtboxes;
		bool lastFrameHasIFrame;
		Timer iFrames;

		[Space]
		public UnityEvent<Puppet> OnDeath;
		public UnityEvent<Puppet> OnHit;
		public UnityEvent<Puppet> OnHeal;

		public int InitialMaxHealth;

		void Awake() {
			Status = new Status(){
				MaxHealth = 10
			};
			Entity = GetComponent<Alive>();
			hurtboxes = new List<Hurtbox3D>(GetComponentsInChildren<Hurtbox3D>());
			Debug.Log(hurtboxes.Count);
		}

		void OnEnable() {
			Status.MaxHealth = InitialMaxHealth;
			Status.Health = Status.MaxHealth;
			SetVulnerable();
		}

		void Start() {
			foreach (var hurtbox in hurtboxes) hurtbox.OnHit.AddListener(ProcessHit);
		}

		void Update() {
			if (iFrames ^ lastFrameHasIFrame) {
				if (iFrames) 	SetInvulnerable();
				else			SetVulnerable();
			} 
			lastFrameHasIFrame = iFrames;
		}

		public void GiveIframes(float seconds) {
			if (!iFrames || iFrames.TimeLeft < seconds)
				iFrames = seconds;
		}

		void onDeath() {
			SetInvulnerable();
			OnDeath?.Invoke(this);
		}

		public void SetVulnerable() {
			foreach (var hurtbox in hurtboxes) hurtbox.SetVulnerable();
		}

		public void SetInvulnerable() {
			foreach (var hurtbox in hurtboxes) hurtbox.SetInvicible();
		}

		void ProcessHit(Hit3D hit) {
			if (!Status.IsDead) {
				Entity.ApplyKnockback(hit.knockbackAmount * hit.hitDir);
				Status.Health -= hit.damage;
				if (Status.Health == 0) 	onDeath();
				else 						OnHit?.Invoke(this);
			}
		}

		public void Heal(int value) {
			OnHeal?.Invoke(this);
			Status.Health += value;
		}
	}
}