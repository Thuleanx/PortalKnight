using UnityEngine;
using NaughtyAttributes;

using Thuleanx.Combat3D;
using Thuleanx.PrettyPatterns;

namespace Thuleanx.PortalKnight {
	public class Nova : MonoBehaviour, iHitGenerator3D {
		public Player Player {get; private set; }
		public Hitbox3D Hitbox {get; private set; }

		[SerializeField] int damage = 9999;
		[SerializeField, Range(0,1)] float delayTillHitboxCheck = 0.9f;
		[SerializeField, Range(0,1)] float hitboxCheckingDuration = 0.3f;
		[SerializeField, Range(0,2)] float lingerDuration = 1f;
		[SerializeField] Vector3 healthOrbSpawnOffset;
		[SerializeField, Required] BubblePool healthOrbPool;

		float aliveTime = 0;

		void Awake() {
			Hitbox = GetComponentInChildren<Hitbox3D>();
		}

		void Start() {
			Hitbox.OnHit.AddListener(OnHit);
			Hitbox.HitGenerator = this;
		}

		void OnEnable() {
			aliveTime = 0;
		}

		void OnDisable() {
			Hitbox.stopCheckingCollision();
		}

		void Update() {
			aliveTime += Time.deltaTime;
			if (aliveTime <= delayTillHitboxCheck) {
				if (Hitbox.Active) Hitbox.stopCheckingCollision();
			} else if (aliveTime <= delayTillHitboxCheck + hitboxCheckingDuration)  {
				if (!Hitbox.Active) Hitbox.startCheckingCollision();
			} else if (aliveTime <= delayTillHitboxCheck + hitboxCheckingDuration + lingerDuration) {
				if (Hitbox.Active) Hitbox.stopCheckingCollision();
			} else gameObject.SetActive(false);
		}

		public void Initialize(Player player) => Player = player;

		void OnHit(Hit3D hit) {
			if (hit.damage == damage) {
				Vector3 spawnPos = hit.position;
				spawnPos.y = transform.position.y;
				spawnPos += healthOrbSpawnOffset;
				healthOrbPool.BorrowTyped<HealthOrb>(gameObject.scene, spawnPos).Initialize(Player);
			}
		}

		public Hit3D GenerateHit(Hitbox3D hitbox, Hurtbox3D hurtbox) {
			if (hurtbox.GetComponentInParent<SpawnerEnemy>()) 
				return new Hit3D(1, 0, Vector3.zero, hurtbox.transform.position);
			return new Hit3D(damage, 0, Vector3.zero, hurtbox.transform.position);
		}
	}
}