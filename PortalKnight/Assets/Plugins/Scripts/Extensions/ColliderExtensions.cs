using UnityEngine;

namespace Thuleanx.Extensions {
	public static class ColliderExtensions {
		public static Vector3 size(this Collider Collider) {

			if (Collider is BoxCollider) return (Collider as BoxCollider).size;
			if (Collider is SphereCollider) return (Collider as SphereCollider).radius * Vector3.one * 2;
			if (Collider is CapsuleCollider) {
				var radius = ((CapsuleCollider)Collider).radius;
				var height = ((CapsuleCollider)Collider).height;
				var direction = ((CapsuleCollider)Collider).direction;

				var directionArray = new Vector3[] { Vector3.right, Vector3.up, Vector3.forward };
				var result = new Vector3();
				for (int i = 0; i < 3; i++) {
					if (i == direction)
						result += directionArray[i] * height;
					else
						result += directionArray[i] * radius * 2;
				}
				return result;
			} else if (Collider is MeshCollider) return ((MeshCollider)Collider).sharedMesh.bounds.size;

			return Collider.bounds.size;
		}
	}
}