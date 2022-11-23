using UnityEngine;

namespace Thuleanx.Combat3D {
	public struct Hit3D {
		public int damage;
		public float knockbackAmount;
		public Vector3 hitDir;
		public Vector3 position;

		public Hit3D(int damage, float knockbackAmount, Vector3 hitDir, Vector3 position) {
			this.damage = damage;
			this.knockbackAmount = knockbackAmount;
			this.hitDir = hitDir.sqrMagnitude > 0 ? hitDir.normalized : Vector3.zero;
			this.position = position;
		}
	}
}