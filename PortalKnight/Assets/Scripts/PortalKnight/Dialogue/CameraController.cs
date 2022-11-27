using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Yarn.Unity;

namespace Thuleanx.PortalKnight.Dialogue {
	public class CameraController : MonoBehaviour {
		[field:SerializeField] List<string> cameraNames;
		public Animator Anim {get; private set;}

		void Awake() {
			Anim = GetComponent<Animator>();
		}

		[YarnCommand("camera_pan")]
		void yarn_Pan(string cameraName) {
			int index = cameraNames.IndexOf(cameraName);
			if (index == -1) {
				Debug.LogError($"yarnCommand camera_pan({cameraName}) failed: camera name not recognized.");
				return;
			}
			Anim.SetInteger("state", index);
		}
	}
}