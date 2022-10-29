using UnityEngine;

namespace Thuleanx.Combat3D {
	public struct Hit3D {
		public int damage;
		public float knockbackAmt;

		public Hitbox3D hitbox;
		public Hurtbox3D hurtbox;

		public Hit3D(int damage, Hitbox3D hitbox, Hurtbox3D hurtbox, float knockback = 0) {
			this.damage = damage;
			this.hitbox = hitbox;
			this.hurtbox = hurtbox;
			this.knockbackAmt = knockback;
		}
	}
}