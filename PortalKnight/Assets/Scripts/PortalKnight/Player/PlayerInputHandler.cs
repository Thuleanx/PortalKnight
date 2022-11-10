using System;
using NaughtyAttributes;
using Thuleanx.PrettyPatterns.ResChain;
using Thuleanx.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Thuleanx.PortalKnight {
	public class PlayerInputHandler : MonoBehaviour, IProgram<PlayerInputChain> {
		[SerializeField] float inputBufferTime = 0.2f;
		Vector2 movement;
		Vector2 mousePosSS;
		Timer[] inputBuffers;

		void Awake() {
			inputBuffers = new Timer[Enum.GetNames(typeof(Player.ActionType)).Length];
		}
		
		void Start() {
			PlayerInputChain inputChain = GetComponent<PlayerInputChain>();
			for (int i = 0; i < inputBuffers.Length; i++) 
				inputChain.AddListener((Player.ActionType) i, stopBuffer);
		}

		public void OnMovement(InputAction.CallbackContext ctx) {
			movement = ctx.ReadValue<Vector2>();
		}
		public void OnMousePos(InputAction.CallbackContext ctx) => mousePosSS = ctx.ReadValue<Vector2>();

		// Buffering an input to an action
		public void OnButton(InputAction.CallbackContext ctx, Player.ActionType action) {
			if (ctx.started) inputBuffers[(int)action] = inputBufferTime;
		}

		public void OnAttack(InputAction.CallbackContext ctx) => OnButton(ctx, Player.ActionType.Attack);
		public void OnShoot(InputAction.CallbackContext ctx) => OnButton(ctx, Player.ActionType.Shoot);
		public void OnDash(InputAction.CallbackContext ctx) => OnButton(ctx, Player.ActionType.Dash);

		public int GetPriority() => 0;

		public PlayerInputChain Process(PlayerInputChain data) {
			data.movement = movement;
			data.mousePosSS = mousePosSS;
			for (int i = 0; i < inputBuffers.Length; i++)
				if (inputBuffers[i] && data.canTriggerAction[i])
					data.triggerAction[i] = true;
			return data;
		}

		void stopBuffer(Player.ActionType action) => inputBuffers[(int) action].Stop();
	}
}