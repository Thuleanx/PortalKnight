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
		Timer _iframe;

		[Space] public UnityEvent<Hit3D> OnHit;

		private void Awake() {
			Collider = GetComponent<Collider>();
		}

		public void ApplyHit(Hit3D hit) => OnHit.Invoke(hit);
		public bool CanTakeHit => !_iframe;
		public void GiveIframe(float duration) => _iframe = duration;

		private void OnDrawGizmos() {
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