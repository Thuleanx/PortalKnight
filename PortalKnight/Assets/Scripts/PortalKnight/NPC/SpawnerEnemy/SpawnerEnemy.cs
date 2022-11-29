using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Thuleanx.AI.FSM;
using Thuleanx.Combat3D;
using Thuleanx.PrettyPatterns;
using Thuleanx.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

namespace Thuleanx.PortalKnight {
	public partial class SpawnerEnemy {
		public enum State {
			Neutral,
			Spell,
			Dead
		}
	}

	public partial class SpawnerEnemy : Alive {
		#region Components
		public StateMachine<SpawnerEnemy> StateMachine {get; private set; }
		#endregion

		#region Spawing
		[SerializeField] float detectionRadius = 20;
		[SerializeField, BoxGroup("Spawning"), MinMaxSlider(0, 30)] Vector2 cooldown;
		[SerializeField, BoxGroup("Spawning"), Range(0, 3)] float spellDuration;
		[SerializeField, BoxGroup("Spawning")] Vector3 offset;
		[SerializeField, BoxGroup("Spawning"), Range(0, 20)] float range = 3;
		[SerializeField, BoxGroup("Spawning"), Range(0, 20)] int maxShadowEnemies;
		[SerializeField, BoxGroup("Spawning"), Required] BubblePool shadowEnemeyPool;
		[SerializeField, Space] UnityEvent OnPermaDeath;

		Player player;
		#endregion

		List<ShadowEnemy> enemies = new List<ShadowEnemy>();

		public override void Awake() {
			base.Awake();
			player = FindObjectOfType<Player>();
			StateMachine = GetComponent<StateMachine<SpawnerEnemy>>();
			StateMachine.Construct();
		}

		public override void Start(){
			base.Start();
			StateMachine.Init();
		}

		protected override void Update() {
			StateMachine.RunUpdate();
			base.Update();
		}

		void FixedUpdate() {
			StateMachine.RunFixUpdate();
		}

		void OnEnemyDeath(Puppet enemyPuppet) {
			enemies.Remove(enemyPuppet.GetComponent<ShadowEnemy>());
			enemyPuppet.OnDeath.RemoveListener(OnEnemyDeath);
		}

		[Button]
		void ___SpawnEnemy() {
			StartCoroutine(iSpawnEnemyInRange());
		}

		IEnumerator iSpawnEnemyInRange() {
			while (true) {
				Vector3 desiredSpawnPos = transform.position + offset;
				Vector2 randInsideCircle = Random.insideUnitCircle * range;
				desiredSpawnPos += randInsideCircle.x * Vector3.right + randInsideCircle.y * Vector3.forward;
				NavMeshHit hit;
				if (NavMesh.SamplePosition(desiredSpawnPos, out hit, range, NavMesh.AllAreas)) {
					SpawnEnemy(hit.position);
					break;
				} else yield return new WaitForSeconds(0.5f); // keep waiting if not hitting a point
			}
		}

		public ShadowEnemy SpawnEnemy(Vector3 spawnPos) {
			GameObject shadowEnemyObj = shadowEnemeyPool.Borrow(gameObject.scene, spawnPos);
			ShadowEnemy shadowEnemy = shadowEnemyObj.GetComponent<ShadowEnemy>();
			enemies.Add(shadowEnemy);
			shadowEnemy.GetComponent<Puppet>().OnDeath.AddListener(OnEnemyDeath);
			return shadowEnemy;
		}

		protected override void Move(Vector3 displacement) {
			// does nothing, this boi do not move
		}

		void OnDrawGizmosSelected() {
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position + offset, range);
		}
		protected override void OnDeath(Puppet puppet) {
			StateMachine.SetState((int) State.Dead);
		}

		public override void Reanimated() {
		}

		public override void Vanquish() {
		}

		bool playerInRange => (player.transform.position - transform.position).magnitude < detectionRadius;
	}
}