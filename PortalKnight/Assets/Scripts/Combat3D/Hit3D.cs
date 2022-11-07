using UnityEngine;

namespace Thuleanx.Combat3D {
	public struct Hit3D {
		public int damage;
		public float knockbackAmount;
		public Vector3 hitDir;

		public Hit3D(int damage, float knockbackAmount, Vector3 hitDir) {
			this.damage = damage;
			this.knockbackAmount = knockbackAmount;
			this.hitDir = hitDir;
		}
	}
}