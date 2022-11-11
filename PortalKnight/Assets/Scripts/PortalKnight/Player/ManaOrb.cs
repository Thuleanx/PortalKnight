using UnityEngine;
using NaughtyAttributes;
using DG.Tweening;

using Thuleanx.Combat3D;
using Thuleanx.Utils;

namespace Thuleanx.PortalKnight {
	public class ManaOrb : Movable, iHitGenerator3D {
		public Hitbox3D Hitbox {get; private set; }

		#region Movement
		[BoxGroup("Movement"), Range(0, 20),SerializeField] float maxVelocity = 5;
		[BoxGroup("Movement"), Range(0,1000),SerializeField] float maxForce = 100;
		[BoxGroup("Movement"), MinMaxSlider(0,10),SerializeField] Vector2 lifeTime;
		#endregion

		float forceLimit;
		int damage;
		Puppet target;
		Vector3 targetPosFallback;
		Vector3 targetOffset;
		Tween forceLimitTween;
		Timer alive;

		void Awake() {
			Hitbox = GetComponentInChildren<Hitbox3D>();
		}

		protected override void Update() {
			Vector3 targetPos = (target && !target.IsDead ? target.transform.position : targetPosFallback) + targetOffset;
			if (!alive || (!target && (transform.position - targetPos).sqrMagnitude < 0.5))
				Expire();
			Vector3 desiredVelocity = (targetPos - transform.position).normalized * maxVelocity;
			Vector3 steering = Vector3.ClampMagnitude(desiredVelocity - Velocity, maxForce * Time.deltaTime);

			Velocity = Vector3.ClampMagnitude(steering + Velocity, maxVelocity);
			base.Update();
		}

		void OnEnable() {
			Hitbox.HitGenerator = this;
			Hitbox.OnHit.AddListener(OnHit);
		}

		void OnDisable() {
			Hitbox.HitGenerator = null;
			Hitbox.OnHit.RemoveListener(OnHit);
			forceLimitTween?.Kill();
		}

		public void Initialize(Vector3 startingDirection, Puppet trackTarget, Vector3 targetOffset, Vector3 targetPosFallback, int damage) {
			Velocity = startingDirection == Vector3.zero ?  Vector3.zero : startingDirection.normalized * maxVelocity;
			target = trackTarget;
			this.targetOffset = targetOffset;
			this.targetPosFallback = targetPosFallback;
			this.damage = damage;
			TurnFaceVelocity();
			forceLimitTween = DOVirtual.Float(0, maxForce, lifeTime.x, (x) => { forceLimit = x;}).SetEase(Ease.OutExpo);
			alive = lifeTime.y;
		}

		void TurnFaceVelocity() {
			if (Velocity != Vector3.zero) transform.rotation = Quaternion.LookRotation(Velocity, transform.up);
		}

		protected override void Move(Vector3 displacement) {
			transform.position += displacement;
		}

		void OnHit(Hit3D hit) {
			gameObject.SetActive(false);
		}

		void Expire() {
			gameObject.SetActive(false);
		}

		public Hit3D GenerateHit(Hitbox3D hitbox, Hurtbox3D hurtbox) => new Hit3D(damage, 0, Velocity == Vector3.zero ? Vector3.zero : Velocity.normalized);

		// steering behaviour
	}
}