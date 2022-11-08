using UnityEngine;
using NaughtyAttributes;

namespace Thuleanx.PortalKnight.Dialogue {
	public class Speaker : MonoBehaviour {
		public string Name;
		public Vector2 offset;
		bool init = false;

		void Start() => TryInit();

		void TryInit() {
			if (!init && SpeakerManager.instance) {
				SpeakerManager.instance.RegisterSpeaker(this);
				init = true;
			}
		}

		void OnEnable() {
			SpeakerManager.instance?.RegisterSpeaker(this);
			if (SpeakerManager.instance) init = true;
	}
		void OnDisable() {
			SpeakerManager.instance?.DeregisterSpeaker(this);
		}
	}
}