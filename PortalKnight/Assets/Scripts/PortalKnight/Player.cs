using System;
using UnityEngine;
using UnityEngine.InputSystem;
using NaughtyAttributes;

using Thuleanx.AI.FSM;
using Thuleanx.Utils;

namespace Thuleanx.PortalKnight {
	[RequireComponent(typeof(CharacterController))]
	public partial class Player : MonoBehaviour {
		#region Components
		public StateMachine<Player> StateMachine {get; private set;}
		public CharacterController Controller {get; private set; }
		#endregion

		#region Movement
		[HorizontalLine(color:EColor.Blue)]
		[BoxGroup("Movement"), Range(0, 10), SerializeField] float speed = 4;
		[BoxGroup("Movement"), Range(0, 64), SerializeField] float accelerationAlpha = 24;
		[BoxGroup("Movement"), Range(0, 64), SerializeField] float deccelerationAlpha = 12;

		[HorizontalLine(color:EColor.Violet)]
		[BoxGroup("Dash"), Range(0, 4), SerializeField] float dashCooldown = 1;
		[BoxGroup("Dash"), Range(0, 100), SerializeField] float dashSpeed = 10;
		[BoxGroup("Dash"), Range(0, 1), Tooltip("Dash duration in seconds"), SerializeField] float dashDuration = 10;
		[BoxGroup("Dash"), Range(0, 64), SerializeField] float dashDrag;
		#endregion

		# region Input Fields
		[Header("Input Related Fields")]
		[SerializeField] float inputBufferTime = 0.2f;
		[ReadOnly, SerializeField] Vector2 movement;
		[ReadOnly, SerializeField] Vector2 aimPosition;
		[ReadOnly, SerializeField] Vector2 lastNonZeroMovement;

		#endregion

		#region State Related Fields
		[Header("State Related Fields")]
		[ReadOnly, SerializeField] Vector3 Velocity;
		[ReadOnly, SerializeField] float Drag = 0;

		public enum State {
			Neutral = 0,
			Attack = 1,
			Shoot = 2,
			Dash = 3,
			Frozen = 4,
			Dead = 5
		};

		public enum ActionType {
			Attack = 0,
			Shoot = 1,
			Dash = 2
		};

		Timer[] inputBuffers;

		// Only States should access this
		public Func<bool>[] ActionHandler;
		#endregion

		private void Awake() {
			StateMachine = GetComponent<StateMachine<Player>>();
			Controller = GetComponent<CharacterController>();

			inputBuffers = new Timer[Enum.GetNames(typeof(ActionType)).Length];
			ActionHandler = new Func<bool>[Enum.GetNames(typeof(ActionType)).Length];
		}

		public void OnMovement(InputAction.CallbackContext ctx) {
			movement = ctx.ReadValue<Vector2>();
			if (movement.sqrMagnitude > 0)
				lastNonZeroMovement = movement;
		}

		// Buffering an input to an action
		public void OnButton(InputAction.CallbackContext ctx, ActionType action) {
			if (ctx.started) inputBuffers[(int) action] = inputBufferTime;
		}

		public Vector3 InputMovementToWorldDir(Vector2 movement) {
			Vector3 inputDir = new Vector3(
				movement.x, 0, movement.y
			).normalized;

			return Quaternion.Euler(
				0, Camera.main.transform.eulerAngles.y, 0f
			) * inputDir;
		}

		public void OnAttack(InputAction.CallbackContext ctx) =>	OnButton(ctx, ActionType.Attack);
		public void OnShoot(InputAction.CallbackContext ctx) => 	OnButton(ctx, ActionType.Shoot);
		public void OnDash(InputAction.CallbackContext ctx) => 		OnButton(ctx, ActionType.Dash);

		private void Update() {
			// Trigger all ActionHandlers, if it is not null and the input is buffered recently
			for (int i = 0; i < inputBuffers.Length; i++) {
				if (inputBuffers[i] && ActionHandler[i] != null) {
					ActionHandler[i].Invoke();
					inputBuffers[i].Stop();
				}
			}
			StateMachine.RunUpdate();
			Controller.Move(Velocity * Time.deltaTime);
			if (Drag > 0) Velocity = Mathx.Damp(Vector3.Lerp, Velocity, Vector3.zero, Drag, Time.deltaTime);
		}
	}
}
