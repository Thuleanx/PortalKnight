using Thuleanx.Combat3D;
using UnityEngine;
using Thuleanx.Utils;
using NaughtyAttributes;
using DG.Tweening;

namespace Thuleanx.PortalKnight {
	public class ShadowBlob : MonoBehaviour, iHitGenerator3D {
		public Hitbox3D Hitbox {get; private set; }

		[SerializeField] int damage = 1;
		[SerializeField, Range(0, 40)] float knockback = 8;
		[SerializeField] GameObject bomb;
		[SerializeField] float lifetime = 4;
		[SerializeField] float exitDuration = 1;
		[SerializeField, MinMaxSlider(0.5f, 2)] Vector2 sizeVariation;
		[SerializeField] Ease OutEase = Ease.InCirc;

		Timer alive;
		bool wasAlive = false;

		void Awake() {
			Hitbox = GetComponentInChildren<Hitbox3D>();
			Hitbox.HitGenerator = this;
		}

		void OnEnable() {
			transform.localScale = Vector3.one * Mathx.RandomRange(sizeVariation);
			Hitbox.OnHit.AddListener(OnHit);
			alive = lifetime;
			wasAlive = true;
		}

		void OnDisable() {
			Hitbox.OnHit.RemoveListener(OnHit);
		}


		void Update() {
			if (wasAlive && !alive) Expire();
			wasAlive = alive;
		}

		void Expire() {
			transform.DOScale(Vector3.zero, exitDuration).SetEase(OutEase).OnComplete(() => {
				gameObject.SetActive(false);
			});
		}

		void OnHit(Hit3D hit) {
			gameObject.SetActive(false);
		}

		public Hit3D GenerateHit(Hitbox3D hitbox, Hurtbox3D hurtbox) => new Hit3D(damage, knockback, (hurtbox.transform.position - hitbox.transform.position).normalized, hurtbox.transform.position);
	}
}