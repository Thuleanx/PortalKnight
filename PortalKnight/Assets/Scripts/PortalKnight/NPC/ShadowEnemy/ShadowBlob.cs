using Thuleanx.Combat3D;
using UnityEngine;
using Thuleanx.Utils;

namespace Thuleanx.PortalKnight {
	public class ShadowBlob : MonoBehaviour, iHitGenerator3D {
		public Hitbox3D Hitbox {get; private set; }

		[SerializeField] int damage = 1;
		[SerializeField, Range(0, 40)] float knockback = 8;
		[SerializeField] GameObject bomb;
		[SerializeField] float lifetime = 4;

		Timer alive;

		void Awake() {
			Hitbox = GetComponentInChildren<Hitbox3D>();
			Hitbox.HitGenerator = this;
		}

		void OnEnable() {
			Hitbox.OnHit.AddListener(OnHit);
			alive = lifetime;
		}

		void OnDisable() {
			Hitbox.OnHit.RemoveListener(OnHit);
		}

		void Update() {
			if (!alive) gameObject.SetActive(false);
		}

		void OnHit(Hit3D hit) {
			gameObject.SetActive(false);
		}

		public Hit3D GenerateHit(Hitbox3D hitbox, Hurtbox3D hurtbox) => new Hit3D(damage, knockback, (hurtbox.transform.position - hitbox.transform.position).normalized, hurtbox.transform.position);
	}
}