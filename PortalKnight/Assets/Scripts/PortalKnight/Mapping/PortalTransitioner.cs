using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

using Thuleanx.PrettyPatterns.ResChain;
using Thuleanx.Utils;
using Thuleanx.PortalKnight.Interactions;

namespace Thuleanx.PortalKnight.Mapping {
	public class PortalTransitioner : MonoBehaviour {
		[SerializeField] float enterOffset = 1;
		[SerializeField] float exitOffset = 1;
		[SerializeField] float transitionDuration = 0.5f;
		bool transitioning = false;

		public bool Transition(Interactible Triggerer) {
			if (transitioning) return false;
			transitioning = true;
			StartCoroutine(iTransition(Triggerer));
			return true;
		}

		IEnumerator iTransition(Interactible Triggerer) {
			Player player = FindObjectOfType<Player>();
			player.Interactible = false;

			// project player onto Triggerer.transform.forward;
			float dist = Vector3.Dot(Triggerer.transform.forward, player.transform.position - Triggerer.transform.position);
			dist = Mathf.Min(dist, enterOffset);

			yield return iWalkToPos(-Triggerer.transform.forward * dist + Triggerer.transform.position);

			yield return iWalkToPos(Triggerer.transform.forward * exitOffset + Triggerer.transform.position);
			if (transitionDuration > 0) yield return new WaitForSeconds(transitionDuration);
			Passage destination = (Triggerer as Passage).Link;
			destination.GetComponent<Collider>().enabled = false;
			// we need to teleport the player ==> disable controller
			player.Controller.enabled = false;
			Vector3 deltaPos = destination.transform.position - player.transform.position;
			player.transform.position = destination.transform.position;
			FindObjectOfType<Cinemachine.CinemachineVirtualCamera>().OnTargetObjectWarped(player.transform, deltaPos);
			player.Controller.enabled = true;


			Debug.Log("WALKING TO POS");
			yield return iWalkToPos(-destination.transform.forward * enterOffset + destination.transform.position);
			Debug.Log("Finished TO POS");
			// TODO: animate the portal out
			destination.GetComponent<Collider>().enabled = true;

			player.Interactible = true;
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