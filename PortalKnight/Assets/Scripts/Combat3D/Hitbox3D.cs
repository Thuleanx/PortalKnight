using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

using Thuleanx.Extensions;

namespace Thuleanx.Combat3D {
	[RequireComponent(typeof(Collider))]
	public class Hitbox3D : MonoBehaviour {
		public Collider Collider {get; private set;}


		public bool Active {
			get => Collider.enabled; 
			private set => Collider.enabled = value;
		}
		bool lastFrameActive;

		[HideInInspector] public iHitGenerator3D HitGenerator;

		[Range(0,1), Tooltip("Hitbox of the same faction as a hurtbox won't try to hit it")] 
		public int faction = 0;

		[SerializeField, Min(0f)] float frequency;
		Dictionary<long, float> hurtboxCooldown = new Dictionary<long, float>();

		[Space]
		public UnityEvent<Hit3D> OnHit;

		private void Awake() {
			Collider = GetComponent<Collider>();
		}

		private void OnTriggerStay(Collider other) {
			Hurtbox3D hurtbox = other.GetComponent<Hurtbox3D>();
			if (lastFrameActive ^ Active) hurtboxCooldown.Clear();
			if (HitGenerator != null && hurtbox && faction != hurtbox.faction && hurtbox.CanTakeHit && TimedOut(hurtbox.ID)) {
				Hit3D Hit = HitGenerator.GenerateHit(this, hurtbox);
				if (Hit.damage > 0) {
					hurtbox.ApplyHit(Hit);
					OnHit?.Invoke(Hit);
					Refresh(hurtbox.ID);
				}
			}
		}

		void OnEnable() {
			hurtboxCooldown.Clear();
		}

		void LateUpdate() {
			lastFrameActive = Active;
		}

		public void startCheckingCollision() { 
			Active = true;
		}

		public void stopCheckingCollision() {
			Active = false;
			hurtboxCooldown.Clear();
		} 

		bool TimedOut(long hurtboxID) => !hurtboxCooldown.ContainsKey(hurtboxID) 
			|| (frequency > 0 && hurtboxCooldown[hurtboxID] + frequency < Time.time);
		void Refresh(long hurtboxID) => hurtboxCooldown[hurtboxID] = Time.time;

		private void OnDrawGizmos() {
			if (Collider && Active) {
				Matrix4x4 prev = Gizmos.matrix;
				Matrix4x4 rotationMatrix = Matrix4x4.TRS(Collider.bounds.center, transform.rotation, Collider.size().Multiply(transform.lossyScale));

				Gizmos.matrix = rotationMatrix; 
				Color col = Color.red;
				col.a = 0.5f;
				Gizmos.color = col;
				Gizmos.DrawCube(Vector3.zero, Vector3.one);

				Gizmos.matrix = prev;
			}
		}
	}
}