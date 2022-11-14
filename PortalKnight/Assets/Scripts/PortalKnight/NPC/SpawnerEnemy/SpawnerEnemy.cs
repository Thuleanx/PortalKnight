using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NaughtyAttributes;

using Thuleanx.AI.FSM;
using Thuleanx.PrettyPatterns;
using Thuleanx.Combat3D;
using Thuleanx.Utils;

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
		[SerializeField, BoxGroup("Spawning"), MinMaxSlider(0, 30)] Vector2 cooldown;
		[SerializeField, BoxGroup("Spawning"), Range(0, 3)] float spellDuration;
		[SerializeField, BoxGroup("Spawning")] Vector3 offset;
		[SerializeField, BoxGroup("Spawning"), Range(0, 20)] float range = 3;
		[SerializeField, BoxGroup("Spawning"), Range(0, 20)] int maxShadowEnemies;
		[SerializeField, BoxGroup("Spawning"), Required] BubblePool shadowEnemeyPool;
		#endregion

		List<ShadowEnemy> enemies = new List<ShadowEnemy>();

		public override void Awake() {
			base.Awake();
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
	}
}