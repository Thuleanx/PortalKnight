using System;
using UnityEngine;
using Yarn.Unity;

using Thuleanx.PrettyPatterns.ResChain;

namespace Thuleanx.PortalKnight.Dialogue {
	[RequireComponent(typeof(DialogueRunner))]
	public class DialogueSceneInteracter : MonoBehaviour, IProgram<PlayerInputChain> {
		public DialogueRunner Runner {get; private set; }
		public Player Player {get; private set; }
		public bool InDialogue {get; private set; }

		public int GetPriority() => 1;
		public PlayerInputChain Process(PlayerInputChain data) {
			data.mousePosSS = Vector2.zero;
			data.movement = Vector2.zero;
			Array.Fill(data.canTriggerAction, false);
			return data;
		}

		void Awake() {
			Runner = GetComponent<DialogueRunner>();
			Player = FindObjectOfType<Player>();
		}

		public void OnDialogueStart() {
			if (!InDialogue) {
				InDialogue = true;
				// locks the player by adding itself into the chain
				Player.Input.Attach(this);
			}
		}

		public void OnDialogueEnd() {
			if (InDialogue) {
				InDialogue = false;
				Player.Input.Dettach(this);
			}
		}
	}
}