using UnityEngine;

namespace Thuleanx.Combat3D {
	public struct Hit3D {
		public float damage;
		public float knockbackAmount;
		public Vector3 hitDir;

		public Hit3D(float damage, float knockbackAmount, Vector3 hitDir) {
			this.damage = damage;
			this.knockbackAmount = knockbackAmount;
			this.hitDir = hitDir;
		}
	}
}