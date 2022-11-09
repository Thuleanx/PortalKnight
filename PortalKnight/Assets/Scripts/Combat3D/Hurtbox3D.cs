using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;
using Thuleanx.Utils;

using Thuleanx.Extensions;

namespace Thuleanx.Combat3D {
	[RequireComponent(typeof(Collider))]
	public class Hurtbox3D : MonoBehaviour {
		public static long NextID = 0;

		[ReadOnly] public long ID;

		public Collider Collider {get; private set;}
		bool active;

		[Range(0,1), Tooltip("Hitbox of the same faction as a hurtbox won't try to hit it")] 
		public int faction = 0;
		[Space] public UnityEvent<Hit3D> OnHit;

		void Awake() {
			Collider = GetComponent<Collider>();
		}

		void Update() {
			if (CanTakeHit ^ Collider.enabled) Collider.enabled ^= true;
		}

		public void SetInvicible() => active = false;
		public void SetVulnerable() => active = true;
		public void SetState(bool canTakeHit) => this.active = canTakeHit;


		public void ApplyHit(Hit3D hit) => OnHit.Invoke(hit);
		public bool CanTakeHit => active;

		void OnDrawGizmosSelected() {
			if (Collider && CanTakeHit) {
				Matrix4x4 prev = Gizmos.matrix;
				Matrix4x4 rotationMatrix = Matrix4x4.TRS(Collider.bounds.center, transform.rotation, Collider.size());

				Gizmos.matrix = rotationMatrix; 
				Color col = Color.yellow;
				col.a = 0.5f;
				Gizmos.color = col;
				Gizmos.DrawCube(Vector3.zero, Vector3.one);

				Gizmos.matrix = prev;
			}
		}
	}
}