using System.Collections.Generic;
using NaughtyAttributes;
using Thuleanx.AI.FSM;
using Thuleanx.Combat3D;
using Thuleanx.PrettyPatterns;
using Thuleanx.Utils;
using UnityEngine;
using UnityEngine.AI;

namespace Thuleanx.PortalKnight {
	public partial class ShadowEnemy {

		public enum State {
			Spawn,
			Aggro,
			Attack,
			Special,
			Dead
		}
	}

	[RequireComponent(typeof(NavMeshAgent))]
	public partial class ShadowEnemy : Animated {

		#region Components
		public CharacterController Controller {get; private set;}
		public NavMeshAgent NavAgent {get; private set;}
		public StateMachine<ShadowEnemy> StateMachine {get; private set; }

		Player player;
		#endregion

		#region Animation
		[HorizontalLine(color:EColor.Indigo)]
		[BoxGroup("Animation"), AnimatorParam("Anim"), SerializeField] string neutralTrigger;
		[BoxGroup("Animation"), AnimatorParam("Anim"), SerializeField] string attackWindupTrigger;
		[BoxGroup("Animation"), AnimatorParam("Anim"), SerializeField] string attackTrigger;
		[BoxGroup("Animation"), AnimatorParam("Anim"), SerializeField] string specialTrigger;
		#endregion

		#region Movement
		[HorizontalLine(color:EColor.Blue)]
		[SerializeField] float navMeshUpdateInterval = 0.2f;
		[BoxGroup("Movement"), Range(0, 720), SerializeField] float turnSpeed = 24;
		[BoxGroup("Movement"), Range(0, 64), SerializeField] float accelerationAlpha = 24;
		[BoxGroup("Movement"), Range(0, 64), SerializeField] float deccelerationAlpha = 12;
		#endregion

		#region Combat
		[HorizontalLine(color:EColor.Red)]
		[BoxGroup("Melee Attack"), Range(0, 3), SerializeField] float attackRange = 2;
		[BoxGroup("Melee Attack"), Range(0, 10), SerializeField] int attackDamage = 1;
		[BoxGroup("Melee Attack"), Range(0, 30), SerializeField] float attackKnockback = 15;
		[BoxGroup("Melee Attack"), Range(0, 3), SerializeField] float attackWindupTime = 1;
		[BoxGroup("Melee Attack"), Range(0, 2), SerializeField] float attackRecovery = 1;
		[BoxGroup("Melee Attack"), Range(0, 1), SerializeField] float attackCooldown = 1;
		[BoxGroup("Melee Attack"), Range(0, 360), SerializeField] float attackTurnSpeed = 2;
		[BoxGroup("Melee Attack"), Required, SerializeField] 	Hitbox3D meleeHitbox;

		[BoxGroup("Special Attack"), Range(0, 10), SerializeField] float specialRange = 4;
		[BoxGroup("Special Attack"), Range(0, 10), SerializeField] float specialSpeed = 2;
		[BoxGroup("Special Attack"), Range(0, 10), SerializeField] float specialRecovery = 2;
		[BoxGroup("Special Attack"), Range(0, 3), SerializeField] float specialEmissionDistance = 0.5f;
		[BoxGroup("Special Attack"), MinMaxSlider(0, 90), SerializeField] Vector2 specialEmissionPhi;
		[BoxGroup("Special Attack"), Range(1, 10), SerializeField] int specialCount = 1;
		[BoxGroup("Special Attack"), MinMaxSlider(0, 30), SerializeField] Vector2 specialCooldown;
		[BoxGroup("Special Attack"), SerializeField, Required] BubblePool specialBulletPool;
		#endregion

		bool firstFrame = true;

		public override void Awake() {
			base.Awake();
			NavAgent = GetComponent<NavMeshAgent>();
			StateMachine = GetComponent<StateMachine<ShadowEnemy>>();
			Controller = GetComponent<CharacterController>();
			StateMachine.Construct();

			NavAgent.updatePosition = false;
			NavAgent.updateRotation = false;
		}

		public override void Start() {
			base.Start();
			StateMachine.Init();
			firstFrame = false;
		}

		void OnEnable() {
			player = FindObjectOfType<Player>();
			if (!firstFrame) StateMachine.Init();
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

		void TurnToFace(Vector3 dir, float turnSpeed = -1) {
			if (turnSpeed == -1) turnSpeed = this.turnSpeed;
			dir.y = 0;
			if (dir != Vector3.zero) {
				Quaternion desiredRotation = Quaternion.LookRotation(dir, Vector3.up);
				transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRotation, Time.deltaTime * turnSpeed);
			}
		}

		void TurnToFaceImmediate(Vector3 dir) {
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

		void OnDrawGizmosSelected() {
			Vector3 high = Calc.ToSpherical(specialEmissionDistance, specialEmissionPhi.x * Mathf.Deg2Rad, 0);
			Vector3 lo = Calc.ToSpherical(specialEmissionDistance, specialEmissionPhi.y * Mathf.Deg2Rad, 0);

			Vector3 centerHigh = transform.position;
			Vector3 centerLo = transform.position;
			centerHigh.y += high.y;
			centerLo.y += lo.y;
			high += transform.position;
			lo += transform.position;

			float radiusHigh = (high - centerHigh).magnitude;
			float radiusLo = (lo - centerLo).magnitude;

			Calc.DrawWireDisk(centerHigh, radiusHigh, Color.cyan);
			Calc.DrawWireDisk(centerLo, radiusLo, Color.cyan);
		}
	}
}