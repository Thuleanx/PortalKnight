using UnityEngine;

namespace Thuleanx.PortalKnight {
	public class ShadowProjectile : Movable {
		[SerializeField] float gravityMultiplier = 0.1f;
		public Rigidbody Body { get; private set; }

		void Awake() {
			Body = GetComponent<Rigidbody>();
		}

		void OnCollisionEnter(Collision other) {
			gameObject.SetActive(false);
		}

		public void Initialize(Vector3 velocity) {
			Velocity = velocity;
		}

		protected override void Update() {
			// apply gravity
			Velocity += Physics.gravity * Time.deltaTime * gravityMultiplier;
			base.Update();
		}

		protected override void Move(Vector3 displacement) {
			// so no errors
			if (Time.deltaTime > 0) Body.velocity = displacement / Time.deltaTime;
		}
	}
}