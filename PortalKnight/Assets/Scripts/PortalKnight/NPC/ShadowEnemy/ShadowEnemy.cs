using System.Collections.Generic;
using NaughtyAttributes;
using Thuleanx.AI.FSM;
using Thuleanx.Combat3D;
using Thuleanx.Utils;
using UnityEngine;
using UnityEngine.AI;

namespace Thuleanx.PortalKnight {
	public partial class ShadowEnemy {

		public enum State {
			Spawn,
			Aggro,
			Attack,
			Dead
		}
	}

	[RequireComponent(typeof(NavMeshAgent))]
	public partial class ShadowEnemy : Alive {

		#region Components
		public CharacterController Controller {get; private set;}
		public NavMeshAgent NavAgent {get; private set;}
		public StateMachine<ShadowEnemy> StateMachine {get; private set; }

		Player player;
		#endregion

		#region Movement
		[HorizontalLine(color:EColor.Blue)]
		[SerializeField] float navMeshUpdateInterval = 0.2f;
		[BoxGroup("Movement"), Range(0, 64), SerializeField] float accelerationAlpha = 24;
		[BoxGroup("Movement"), Range(0, 64), SerializeField] float deccelerationAlpha = 12;
		#endregion

		#region Combat
		[HorizontalLine(color:EColor.Red)]
		[BoxGroup("Melee Attack"), Range(0, 3), SerializeField] float attackRange = 2;
		[BoxGroup("Melee Attack"), Range(0, 10), SerializeField] int attackDamage = 1;
		[BoxGroup("Melee Attack"), Range(0, 30), SerializeField] float attackKnockback = 15;
		[BoxGroup("Melee Attack"), Range(0, 3), SerializeField] float attackWindupTime = 1;
		[BoxGroup("Melee Attack"), Range(0, 1), SerializeField] float attackDuration = 1;
		[BoxGroup("Melee Attack"), Range(0, 1), SerializeField] float attackCooldown = 1;
		[BoxGroup("Melee Attack"), Required, SerializeField] 	Hitbox3D meleeHitbox;
		#endregion

		public override void Awake() {
			base.Awake();
			NavAgent = GetComponent<NavMeshAgent>();
			StateMachine = GetComponent<StateMachine<ShadowEnemy>>();
			Controller = GetComponent<CharacterController>();

			NavAgent.updatePosition = false;
			NavAgent.updateRotation = false;
		}

		void OnEnable() {
			player = FindObjectOfType<Player>();
			StateMachine.Construct();
			StateMachine.Init();
		}

		protected override void Update() {
			transform.position = FindClosestNavPoint(transform.position);
			NavAgent.nextPosition = transform.position; // syncs nav mesh agent position with current
			StateMachine.RunUpdate();
			NavAgent.velocity = Velocity + Knockback;
			base.Update();
		}

		void FixedUpdate() {
			StateMachine.RunFixUpdate();
		}

		void TurnToFace(Vector3 dir) {
			dir.y = 0;
			if (dir != Vector3.zero)
				transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
		}

		protected override void Move(Vector3 displacement) {
			if (displacement.sqrMagnitude > 0)  {
				displacement = AdjustVelocityToSlope(displacement, Controller.slopeLimit);
				bool inMesh = FindClosestNavPoint(transform.position + displacement, out Vector3 resPos); 
				if (inMesh) Controller.Move(resPos - transform.position);
			}
		}
		protected override void OnDeath(Puppet puppet) => StateMachine.SetState((int) State.Dead);

	}
}