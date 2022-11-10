using System;
using NaughtyAttributes;
using Thuleanx.PrettyPatterns.ResChain;
using Thuleanx.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Thuleanx.PortalKnight {
	public class PlayerInputChain : MonoChain<PlayerInputChain> {
		public Player Player { get; private set; }

		[HideInInspector]
		public Func<Player, bool>[] ActionHandler;

		[ReadOnly] public Vector2 movement;
		[ReadOnly] public Vector2 lastNonZeroMovement;
		[ReadOnly] public Vector2 mousePosSS;
		[ReadOnly]
		public bool[] canTriggerAction;
		[ReadOnly]
		public bool[] triggerAction;

		UnityEvent<Player.ActionType>[] triggerEvent;

		public Vector3 mousePosWS {
			get {
				Camera cam = Camera.main;
				Vector2 vp = cam.ScreenToViewportPoint(mousePosSS);
				Ray ray = Camera.main.ViewportPointToRay(vp);
				Plane plane = new Plane(Vector3.up, transform.position.y);
				float dist;
				if (plane.Raycast(ray, out dist)) {
					Vector3 pos = ray.GetPoint(dist);
					return pos;
				}
				return Camera.main.ScreenToWorldPoint(mousePosSS);
			}
		}

		public Vector3 MovementToWorldDir(Vector2 movement) {
			Vector3 inputDir = new Vector3(
				movement.x, 0, movement.y
			).normalized;

			return Quaternion.Euler(
				0, Camera.main.transform.eulerAngles.y, 0f
			) * inputDir;
		}

		void Awake() {
			Player = GetComponent<Player>();

			int n = Enum.GetNames(typeof(Player.ActionType)).Length;

			ActionHandler = new Func<Player, bool>[n];
			canTriggerAction = new bool[n];
			triggerAction = new bool[n];
			triggerEvent = new UnityEvent<Player.ActionType>[n];
			for (int i = 0; i < n; i++)
				triggerEvent[i] = new UnityEvent<Player.ActionType>();
		}

		public void ProcessInputs() {
			ResetInputs();
			Assemble(this);
			if (movement != Vector2.zero)
				lastNonZeroMovement = movement;
		}

		void ResetInputs() {
			Array.Fill(canTriggerAction, true);
			Array.Fill(triggerAction, false);
			movement = Vector2.zero;
			mousePosSS = Vector2.zero;
		}

		public void AddListener(Player.ActionType actionType, UnityAction<Player.ActionType> action) => triggerEvent[(int) actionType].AddListener(action);
		public void RemoveListener(Player.ActionType actionType, UnityAction<Player.ActionType> action) => triggerEvent[(int) actionType].RemoveListener(action);

		protected override void Notify() {
			for (int i = 0; i < ActionHandler.Length; i++)
				if (canTriggerAction[i] && triggerAction[i] && ActionHandler[i] != null) {
					ActionHandler[i]?.Invoke(Player);
					triggerEvent[i]?.Invoke((Player.ActionType) i);
				}
		}
	}
}