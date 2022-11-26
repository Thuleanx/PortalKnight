using UnityEngine;
using UnityEngine.AI;
using NaughtyAttributes;
using Thuleanx.Utils;

namespace Thuleanx.PortalKnight {
	public abstract class Movable : MonoBehaviour {
		[Header("Movement Info Fields")]
		[SerializeField] protected float skinWidth = 0.05f;
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
			this.Knockback = Knockback;
		}

		public void ResetMovements() {
			Velocity = Vector3.zero;
			Knockback = Vector3.zero;
			Drag = 0;
		}

		protected Vector3 AdjustVelocityToSlope(Vector3 velocity, float slopeLimit) {
			float slideFriction = 0.3f;
			var ray = new Ray(transform.position + Vector3.down* 0.005f, Vector3.down);
			if (Physics.Raycast(ray, out RaycastHit hit)) {
				Vector3 hitNormal = hit.normal;
				bool isGrounded = (Vector3.Angle (Vector3.up, hitNormal) <= slopeLimit);

				if (!isGrounded) {
					velocity.x += (1f - hitNormal.y) * hitNormal.x * (1f - slideFriction);
					velocity.z += (1f - hitNormal.y) * hitNormal.z * (1f - slideFriction);
				} 

				velocity += Physics.gravity * Time.deltaTime;
				// var slopeRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
				// var adjustedVelocity = slopeRotation * velocity;

				// if (adjustedVelocity.y < 0) 
				// 	velocity = adjustedVelocity;
					// return adjustedVelocity;
				// velocity += Physics.gravity * Time.deltaTime;
				return velocity;
			}
			return velocity;
		}

		protected bool FindClosestNavPoint(Vector3 pos, out Vector3 resPos) {
			if (NavMesh.SamplePosition(pos, out NavMeshHit hit, 0.5f, NavMesh.AllAreas)) {
				resPos = hit.position;
				return true;
			}
			resPos = pos;
			return false;
		}

		protected Vector3 FindClosestNavPoint(Vector3 pos) {
			if (NavMesh.SamplePosition(pos, out NavMeshHit hit, skinWidth, NavMesh.AllAreas))
				return hit.position;
			return pos;
		}

		protected Vector3 StickOnGround(Vector3 pos) {
			var ray = new Ray(transform.position + Vector3.down* skinWidth, Vector3.down);
			if (Physics.Raycast(ray, out RaycastHit hit)) {
				if (hit.distance > skinWidth) return hit.point;
			}
			return pos;
		}

		public void TurnToFace(Vector3 dir, float turnSpeed) {
			dir.y = 0;
			if (dir != Vector3.zero) {
				Quaternion desiredRotation = Quaternion.LookRotation(dir, Vector3.up);
				transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRotation, Time.deltaTime * turnSpeed);
			}
		}

		public void TurnToFaceImmediate(Vector3 dir) {
			dir.y = 0;
			if (dir != Vector3.zero)
				transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
		}
	}
}