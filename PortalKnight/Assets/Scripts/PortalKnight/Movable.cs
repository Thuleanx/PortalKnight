using UnityEngine;
using NaughtyAttributes;

using Thuleanx.Utils;

namespace Thuleanx.PortalKnight {
	public abstract class Movable : MonoBehaviour {
		[Header("Movement Info Fields")]
		[ReadOnly, SerializeField] protected Vector3 Velocity;
		[ReadOnly, SerializeField] protected Vector3 Knockback;
		[Range(1,64), SerializeField] protected float KnockbackResistance = 1;
		[ReadOnly, SerializeField] protected float Drag = 0;

		protected virtual void Update() {
			Move((Velocity + Knockback) * Time.deltaTime);

			if (Drag > 0) Velocity = Mathx.Damp(Vector3.Lerp, Velocity, Vector3.zero, Drag, Time.deltaTime);
			if (KnockbackResistance > 0) Knockback = Mathx.Damp(Vector3.Lerp, Knockback, Vector3.zero, KnockbackResistance, Time.deltaTime);
		}

		protected abstract void Move(Vector3 displacement);
		public void ApplyKnockback(Vector3 Knockback) {
			this.Knockback += Knockback;
		}

		public void ResetMovements() {
			Velocity = Vector3.zero;
			Knockback = Vector3.zero;
			Drag = 0;
		}
	}
}