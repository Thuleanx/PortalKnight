using System;
using UnityEngine;
using UnityEngine.SceneManagement;

using Thuleanx.PrettyPatterns;

namespace Thuleanx {
	public class App : Singleton<App> {
		public override void Awake() {
			base.Awake();

			SceneManager.activeSceneChanged += _activeSceneChange;
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		public static void Bootstrap() {
			GameObject app = FindObjectOfType<App>()?.gameObject;
			if (app == null) {
				app = UnityEngine.Object.Instantiate(Resources.Load("App")) as GameObject;
				if (app == null) throw new ApplicationException();
			}
			UnityEngine.Object.DontDestroyOnLoad(app);
		}

		void _activeSceneChange(Scene before, Scene after) {
			SceneManager.SetActiveScene(after);
		}
	}
}