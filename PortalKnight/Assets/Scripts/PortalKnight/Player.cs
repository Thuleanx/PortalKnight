using System;
using UnityEngine;
using UnityEngine.InputSystem;
using NaughtyAttributes;

using Thuleanx.AI.FSM;
using Thuleanx.Utils;

namespace Thuleanx.PortalKnight {
	[RequireComponent(typeof(CharacterController))]
	public class Player : MonoBehaviour {
		#region Components
		public StateMachine<Player> StateMachine {get; private set;}
		public CharacterController Controller {get; private set; }
		#endregion

		#region Movement
		[HorizontalLine(color:EColor.Blue)]
		[BoxGroup("Movement"), Range(0, 10)] public float Speed = 4;
		[BoxGroup("Movement"), Range(0, 64)] public float AccelerationAlpha = 24;
		[BoxGroup("Movement"), Range(0, 64)] public float DeccelerationAlpha = 12;

		[HorizontalLine(color:EColor.Violet)]
		[BoxGroup("Dash"), Range(0, 100)] public float DashSpeed = 10;
		[BoxGroup("Dash"), Range(0, 10), Tooltip("Dash duration in seconds")] public float DashDuration = 10;
		[BoxGroup("Dash"), Range(0, 64)] public float DashDrag;
		#endregion

		# region Input Fields
		[Header("Input Related Fields")]
		[SerializeField] float inputBufferTime = 0.2f;
		[ReadOnly] public Vector2 Movement;
		[ReadOnly] public Vector2 AimPosition;
		[ReadOnly] public Vector2 LastNonZeroMovement;

		#endregion

		#region State Related Fields
		[Header("State Related Fields")]
		[ReadOnly] public Vector3 Velocity;
		[ReadOnly] public float Drag = 0;

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
		public Action[] ActionHandler;
		#endregion

		private void Awake() {
			StateMachine = GetComponent<StateMachine<Player>>();
			Controller = GetComponent<CharacterController>();

			inputBuffers = new Timer[Enum.GetNames(typeof(ActionType)).Length];
			ActionHandler = new Action[Enum.GetNames(typeof(ActionType)).Length];
		}

		public void OnMovement(InputAction.CallbackContext ctx) {
			Movement = ctx.ReadValue<Vector2>();
			if (Movement.sqrMagnitude > 0)
				LastNonZeroMovement = Movement;
		}

		// Buffering an input to an action
		public void OnButton(InputAction.CallbackContext ctx, ActionType action) {
			if (ctx.started) inputBuffers[(int) action] = inputBufferTime;
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
