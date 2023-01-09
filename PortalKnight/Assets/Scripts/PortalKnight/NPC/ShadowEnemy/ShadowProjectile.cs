using UnityEngine;
using Thuleanx.Combat3D;

using Thuleanx.PrettyPatterns;

namespace Thuleanx.PortalKnight {
	public class ShadowProjectile : Movable {
		public Rigidbody Body { get; private set; }

		[Header("Shadow Projectile")]
		[SerializeField] float gravityMultiplier = 0.1f;
		[SerializeField] BubblePool blobPool;
		[SerializeField] Hurtbox3D hurtbox;

		void Awake() {
			Body = GetComponent<Rigidbody>();
		}

		private void OnEnable() {
			hurtbox?.OnHit.AddListener(OnHitReceived);
		}

		private void OnDisable() {
			hurtbox?.OnHit.RemoveListener(OnHitReceived);
		}

		void OnCollisionEnter(Collision collision) {
			blobPool.BorrowTyped<ShadowBlob>(gameObject.scene, transform.position);
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

		void OnHitReceived(Hit3D hit) {
			if (hit.damage > 100) gameObject.SetActive(false);
		}

	}
}