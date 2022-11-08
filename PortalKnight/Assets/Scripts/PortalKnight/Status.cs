using UnityEngine;
using NaughtyAttributes;

namespace Thuleanx.PortalKnight {
	[System.Serializable]
	public class Status {
		public int Health {
			get => _health; 
			set => _health = Mathf.Clamp(value, 0, MaxHealth);
		}
		[ReadOnly] public int MaxHealth;

		[SerializeField] 
		[ProgressBar("health", 10)]
		int _health;

		public bool IsDead => Health == 0;
	}
}