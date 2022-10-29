using UnityEngine;

namespace Thuleanx.Extensions {
	public static class VectorExtensions {
		public static Vector3 Multiply(this Vector3 x, Vector3 y) => new Vector3(x.x * y.x, x.y * y.y, x.z * y.z);
	}
}