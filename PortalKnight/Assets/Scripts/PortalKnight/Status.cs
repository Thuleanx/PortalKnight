using UnityEngine;

namespace Thuleanx.PortalKnight {
	public class Status {
		public int MaxHealth;
		public int Health {
			get => _health; 
			set => _health = Mathf.Clamp(value, 0, MaxHealth);
		}

		public bool IsDead => Health == 0;

		int _health;
	}
}