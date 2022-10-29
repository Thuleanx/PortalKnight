using UnityEngine;
using NaughtyAttributes;

using Thuleanx.Combat3D;

namespace Thuleanx {
	public class Status : MonoBehaviour {
		public int MaxHealth = 10;
		[SerializeField, ReadOnly, ProgressBar(10)] int health;

		private void Awake() {
			Init();
		}

		private void Start() {
			foreach (Hurtbox3D hurtbox in GetComponentsInChildren<Hurtbox3D>())
				hurtbox.OnHit.AddListener(ProcessHit);
		}

		private void OnDestroy() {
			foreach (Hurtbox3D hurtbox in GetComponentsInChildren<Hurtbox3D>())
				hurtbox.OnHit.AddListener(ProcessHit);
		}

		public void ProcessHit(Hit3D hit) {
			health = (health - hit.damage + MaxHealth + 1) % (MaxHealth + 1);
		}

		public void Init() {
			health = MaxHealth;
		}
	}
}