using UnityEngine;
using FMODUnity;

namespace Thuleanx.Audio {
	public class MultiAudioEventInstances : MonoBehaviour {
		[SerializeField] int maxCount = 5;
		[SerializeField] EventReference reference;
		FMOD.Studio.EventInstance[] instances;
		int currentIndex;

		void Awake() {
			instances = new FMOD.Studio.EventInstance[maxCount];
			for (int i = 0; i < maxCount; i++)
				instances[i] = FMODUnity.RuntimeManager.CreateInstance(reference);
			currentIndex = 0;
		}

		public void Play() {
			int index = currentIndex++%maxCount;
			instances[index].start();
		}

		public void Play(Vector3 position) {
			int index = currentIndex++%maxCount;
			instances[index].set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(position));
			instances[index].start();
		}
	}
}