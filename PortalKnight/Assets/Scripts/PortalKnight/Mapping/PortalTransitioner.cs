using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

using Thuleanx.PrettyPatterns.ResChain;
using Thuleanx.Utils;
using Thuleanx.PortalKnight.Interactions;

namespace Thuleanx.PortalKnight.Mapping {
	public class PortalTransitioner : SceneTransitioner {
		[SerializeField] float enterOffset = 1;
		[SerializeField] float exitOffset = 1;
		[SerializeField] float transitionDuration = 0.5f;


		public override bool Transition(SceneReference TargetScene, Interactible Triggerer) {
			if (transitioning) return false;
			transitioning = true;
			StartCoroutine(iTransition(TargetScene, Triggerer));
			return true;
		}

		IEnumerator iTransition(SceneReference TargetScene, Interactible Triggerer) {
			int ID = (Triggerer as Passage).ID;
			yield return iWalkToPos(-Triggerer.transform.forward * enterOffset + Triggerer.transform.position);
			yield return iWalkToPos(Triggerer.transform.forward * exitOffset + Triggerer.transform.position);
			yield return new WaitForSeconds(transitionDuration);
			Triggerer = null; // Triggerer will now throw errors. Do not use it
			App.instance.RequestLoad(TargetScene.SceneName);
			yield return null; // for scene to load
			foreach (Passage passage in FindObjectsOfType<Passage>()) {
				if (passage.ID == ID) {
					Player player = FindObjectOfType<Player>();
					// we need to teleport the player ==> disable controller
					player.Controller.enabled = false;
					player.transform.position = passage.transform.position;
					player.Controller.enabled = true;
					yield return iWalkToPos(-passage.transform.forward * enterOffset + passage.transform.position);
					// TODO: animate the portal out
					passage.gameObject.SetActive(false);
					break;
				}
			}
			transitioning = false;
		}

		IEnumerator iWalkToPos(Vector3 target) {
			Player player = FindObjectOfType<Player>();
			PlayerInputChain inputChain = player.GetComponent<PlayerInputChain>();
			WalkToLocation guide = new WalkToLocation(target);
			inputChain.Attach(guide);
			while (true) {
				Vector3 displacement = (target - player.transform.position);
				displacement.y = 0;
				if (displacement.sqrMagnitude < 0.1f) break;
				yield return null;
			}
			inputChain.Dettach(guide);
		}

		class WalkToLocation : IProgram<PlayerInputChain> {
			Vector3 target;

			public WalkToLocation(Vector3 target) {
				this.target = target;
			}

			public int GetPriority() => 2;

			public PlayerInputChain Process(PlayerInputChain data) {
				Vector3 displacement = (target - data.transform.position);
				displacement.y = 0;
				data.movement = data.WorldDirToMovement(displacement);
				Array.Fill(data.canTriggerAction, false);
				Array.Fill(data.triggerAction, false);

				return data;
			}
		}

	}
}