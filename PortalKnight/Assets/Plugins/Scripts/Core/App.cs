using System;
using UnityEngine;

namespace Thuleanx {
	public class App : MonoBehaviour {
		public static App Instance;

		private void Awake() {
			#if UNITY_EDITOR
				Application.targetFrameRate = 60;
				QualitySettings.vSyncCount = 0;
			#endif
			Instance = this;
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		public static void Bootstrap() {
			GameObject app = FindObjectOfType<App>()?.gameObject;
			if (app == null) {
				app = UnityEngine.Object.Instantiate(Resources.Load("App")) as GameObject;
				if (app == null) throw new ApplicationException();
				Instance = app.GetComponent<App>();
			}
			UnityEngine.Object.DontDestroyOnLoad(app);
		}
	}
}