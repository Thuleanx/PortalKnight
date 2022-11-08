using UnityEngine;
using System;
using System.Collections.Generic;

using Thuleanx.PrettyPatterns;

namespace Thuleanx.PortalKnight.Dialogue {
	public class SpeakerManager : Singleton<SpeakerManager> {
		Dictionary<string, List<Speaker>> speakerMap = new Dictionary<string, List<Speaker>>();

		public void RegisterSpeaker(Speaker speaker) {
			if (!speakerMap.ContainsKey(speaker.Name))
				speakerMap[speaker.Name] = new List<Speaker>();
			speakerMap[speaker.Name].Add(speaker);
		}

		public void DeregisterSpeaker(Speaker speaker) {
			if (speakerMap.ContainsKey(speaker.Name) && speakerMap[speaker.Name].Contains(speaker)) {
				speakerMap[speaker.Name].Remove(speaker);
				if (speakerMap[speaker.Name].Count == 0) speakerMap.Remove(speaker.Name);
			}
		}

		public Speaker GetSpeaker(string Name) => speakerMap[Name]?[0];

		private void OnDestroy() {
			speakerMap.Clear();
		}
	}
}