using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Thuleanx.PrettyPatterns {
	[CreateAssetMenu(fileName = "BubblePool", menuName = "~/PortalKnight/BubblePool", order = 0)]
	public class BubblePool : ScriptableObject {
		static int DefaultSize = 5;

		bool initialized;

		[Required]
		public GameObject prefab;

		int totalBubbles = 0;
		[SerializeField, ReorderableList, ReadOnly] List<Bubble> pool = new List<Bubble>();
		Dictionary<string, List<Bubble>> borrowedLedger = new Dictionary<string, List<Bubble>>();

		void TryInit() {
			if (!initialized && App.instance) {
				App.BeforeSceneUnload.AddListener(BeforeSceneUnload);
				initialized = true;
			}
#if UNITY_EDITOR
			EditorApplication.quitting += OnApplicationQuits;
#endif
		}

		private void OnEnable() {
			initialized = false;
			Reset();
		}

		void BeforeSceneUnload(Scene scene) {
			CollectsAll(scene);
		}
		void OnApplicationQuits() => Reset();

		void Reset() {
			totalBubbles = 0;
			pool.Clear();
			borrowedLedger.Clear();
		}

		public T BorrowTyped<T>(Scene scene, Vector3? positionNullable = null, Quaternion? rotationNullable = null)
			=> Borrow(scene, positionNullable, rotationNullable).GetComponent<T>();

		public GameObject Borrow(Scene scene, Vector3? positionNullable = null, Quaternion? rotationNullable = null) {
			TryInit();
			if (pool.Count == 0) Expand(Mathf.Max(totalBubbles, DefaultSize));

			Vector3 position = positionNullable == null ? Vector3.zero : (Vector3) positionNullable;
			Quaternion rotation = rotationNullable == null ? Quaternion.identity : (Quaternion) rotationNullable;

			Bubble bubble = pool[pool.Count - 1];
			pool.RemoveAt(pool.Count - 1);
			bubble.gameObject.transform.SetPositionAndRotation(position, rotation);
			bubble.gameObject.SetActive(true);
			bubble.DisposalRequested = Collect;

			// foreach (var gameObject in scene.GetRootGameObjects())
			// 	if (gameObject.name == "_Dynamic")
			// 		bubble.gameObject.transform.SetParent(gameObject.transform);
			// if (bubble.gameObject.transform.parent == null)
				SceneManager.MoveGameObjectToScene(bubble.gameObject, scene);

			bubble.scene = scene;

			if (!borrowedLedger.ContainsKey(scene.name))
				borrowedLedger[scene.name] = new List<Bubble>();
			borrowedLedger[scene.name].Add(bubble);

			return bubble.gameObject;
		}

		void CollectsAll(Scene scene) {
			if (borrowedLedger.ContainsKey(scene.name)) {
				List<Bubble> bubbles = new List<Bubble>(borrowedLedger[scene.name]);
				foreach (Bubble bubble in bubbles) 
					bubble.Pop();
				borrowedLedger.Remove(scene.name);
			}
		}

		void Collect(Bubble bubble) {
			if (borrowedLedger.ContainsKey(bubble.gameObject.scene.name))
				borrowedLedger[bubble.gameObject.scene.name].Remove(bubble);
			bubble.DisposalRequested = null;
			DontDestroyOnLoad(bubble.gameObject);
			bubble.gameObject.SetActive(false);
			pool.Add(bubble);
			TryShrink();
		}

		void TryShrink() {
			if (pool.Count > 3 * (totalBubbles - pool.Count) && totalBubbles >= DefaultSize) {
				int desiredSz = Mathf.Max(totalBubbles / 2, DefaultSize);
				while (totalBubbles > desiredSz) {
					Bubble bubble = pool[pool.Count - 1];
					pool.RemoveAt(pool.Count - 1);
					Destroy(bubble.gameObject);
					totalBubbles--;
				}
			}
		}

		void Expand(int count) {
			while (count-->0) {
				GameObject obj = Instantiate(prefab, Vector2.zero, Quaternion.identity);
				obj.SetActive(false);
				DontDestroyOnLoad(obj);
				Bubble bubble = obj.GetComponent<Bubble>();
				if (bubble == null) bubble = obj.AddComponent<Bubble>();
				pool.Add(bubble);
			}
		}


	}
}