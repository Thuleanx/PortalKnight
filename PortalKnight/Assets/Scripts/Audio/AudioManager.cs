using UnityEngine;
using Thuleanx.PrettyPatterns;
using FMODUnity;

namespace Thuleanx.Audio {
	public class AudioManager : Singleton<AudioManager> {
		public void PlayOneShot(EventReference reference) 
			=> FMODUnity.RuntimeManager.PlayOneShot(reference);
		public void PlayOneShot3D(EventReference reference, Vector3 position) 
			=> FMODUnity.RuntimeManager.PlayOneShot(reference, position);
	}
}