namespace Thuleanx.Combat3D {
	public interface iHitGenerator3D {
		Hit3D GenerateHit(Hitbox3D hitbox, Hurtbox3D hurtbox);
	}
}