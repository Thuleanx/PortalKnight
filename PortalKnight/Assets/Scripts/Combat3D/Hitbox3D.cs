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

		[SerializeField, Min(0f)] float frequency;
		[SerializeField] float knockback;
		Dictionary<long, float> hurtboxCooldown = new Dictionary<long, float>();

		[Space]
		public UnityEvent<Hit3D> OnHit;

		private void Awake() {
			Collider = GetComponent<Collider>();
		}

		public virtual Hit3D GenerateHit(Collider other) 
			=> new Hit3D(1,this,other.GetComponent<Hurtbox3D>(),knockback);

		private void OnTriggerStay(Collider other) {
			Hurtbox3D hurtbox = other.GetComponent<Hurtbox3D>();
			if (hurtbox && hurtbox.CanTakeHit && TimedOut(hurtbox.ID)) {
				Hit3D Hit = GenerateHit(other);
				hurtbox.ApplyHit(Hit);
				OnHit?.Invoke(Hit);
				Refresh(hurtbox.ID);
			}
		}

		public void startCheckingCollision() => Active = true;
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