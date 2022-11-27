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
		static void yarn_Pan(string cameraName) {
			CameraController controller = GameObject.FindObjectOfType<CameraController>();
			int index = controller.cameraNames.IndexOf(cameraName);
			if (index == -1) {
				Debug.LogError($"yarnCommand camera_pan({cameraName}) failed: camera name not recognized.");
				return;
			}
			controller.Anim.SetInteger("state", index);
		}
	}
}