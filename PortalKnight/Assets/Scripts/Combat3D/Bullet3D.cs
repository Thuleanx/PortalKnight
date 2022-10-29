using UnityEngine;

namespace Thuleanx.Combat3D {
	public class Bullet3D : MonoBehaviour {
		public Rigidbody Body {get; private set; }

		public Vector3 Velocity { get => Body.velocity; set => Body.velocity = value; }

		public void Initialize(Vector3 Velocity, bool faceDir = true) {
			this.Velocity = Velocity;
			if (faceDir) transform.LookAt(transform.position + Velocity, Vector3.up);
		}

		private void Awake() {
			Body = GetComponent<Rigidbody>();
		}
	}
}