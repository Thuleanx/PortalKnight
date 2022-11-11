using Thuleanx.Combat3D;
using UnityEngine;
using NaughtyAttributes;

namespace Thuleanx.PortalKnight {
	public class Nova : MonoBehaviour, iHitGenerator3D {
		public Hitbox3D Hitbox {get; private set; }

		[SerializeField] int damage = 9999;
		[SerializeField, MinMaxSlider(0, 2)] Vector2 lifeTime;

		float aliveTime = 0;

		void Awake() {
			Hitbox = GetComponentInChildren<Hitbox3D>();
		}

		void OnEnable() {
			Hitbox.HitGenerator = this;
			aliveTime = 0;
		}

		void OnDisable() {
			Hitbox.startCheckingCollision();
		}

		void Update() {
			aliveTime += Time.deltaTime;
			if (aliveTime > lifeTime.x && Hitbox.Active) {
				Hitbox.HitGenerator = null;
				Hitbox.stopCheckingCollision();
			}
			if (aliveTime > lifeTime.y) gameObject.SetActive(false);
		}

		public Hit3D GenerateHit(Hitbox3D hitbox, Hurtbox3D hurtbox) {
			if (hurtbox.GetComponentInParent<ShadowEnemy>()) 
				return new Hit3D(damage, 0, Vector3.zero);
			return new Hit3D(1, 0, Vector3.zero);
		}
	}
}