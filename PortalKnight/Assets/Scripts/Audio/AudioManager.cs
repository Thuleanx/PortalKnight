using UnityEngine;
using Thuleanx.PrettyPatterns;
using FMODUnity;
using Yarn.Unity;

namespace Thuleanx.Audio {
	public class AudioManager : Singleton<AudioManager> {
		public void PlayOneShot(EventReference reference) 
			=> FMODUnity.RuntimeManager.PlayOneShot(reference);
		public void PlayOneShot3D(EventReference reference, Vector3 position) 
			=> FMODUnity.RuntimeManager.PlayOneShot(reference, position);

		[YarnCommand("playsound")]
		public void PlayOneShotString(string reference) 
			=> FMODUnity.RuntimeManager.PlayOneShot(reference);
	}
}