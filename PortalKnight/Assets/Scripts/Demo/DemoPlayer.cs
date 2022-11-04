using UnityEngine;
using UnityEngine.InputSystem;
using NaughtyAttributes;

using Thuleanx.Combat3D;
using Thuleanx.Utils;

namespace Thuleanx {
	public class DemoPlayer : MonoBehaviour {
		[ReadOnly] Vector2 Movement;
		[ReadOnly] Vector3 Velocity;
		[ReadOnly] Vector3 _lastNonZeroVelocity;

		[SerializeField, Range(0, 10f)] float speed;
		[SerializeField, Range(1,10f)] float turnAlpha;
		[SerializeField] Hitbox3D Hitbox;
		[SerializeField] float hitTime = 0.4f;
		[SerializeField] GameObject BulletPrefab;
		[SerializeField] float bulletSpeed = 10;

#region Components
		public CharacterController Controller {get; private set; }
#endregion

		Timer hitting;

		private void Awake() {
			Controller = GetComponent<CharacterController>();
		}

		private void Update() {

			Vector3 inputDir = new Vector3(
				Movement.x, 0, Movement.y
			).normalized;

			if (inputDir.sqrMagnitude >= 0.01f) {
				Vector3 moveDir = Quaternion.Euler(
					0, Camera.main.transform.eulerAngles.y, 0f
				) * inputDir;
				Velocity = moveDir * speed;
				_lastNonZeroVelocity = Velocity;

			} else Velocity = Vector3.zero;

			Controller.Move(Velocity * Time.deltaTime);
			// turn to match velocity
			if (_lastNonZeroVelocity != Vector3.zero) 
				transform.rotation = Quaternion.Slerp(
					transform.rotation, 
					Quaternion.LookRotation(_lastNonZeroVelocity, Vector3.up), 
					turnAlpha * Time.deltaTime);
			
			if (Hitbox.Active != hitting) {
				if (hitting) 	Hitbox.startCheckingCollision();
				else			Hitbox.stopCheckingCollision();
			}
		}

		public void OnAttack(InputAction.CallbackContext ctx) {
			if (ctx.performed) 	hitting = hitTime;
			if (ctx.canceled) 	hitting.Stop();
		}
		public void OnShoot(InputAction.CallbackContext ctx) {
			if (ctx.started) {
				Bullet3D bullet = Instantiate(BulletPrefab, transform.position, Quaternion.identity).GetComponent<Bullet3D>();
				bullet.Initialize(transform.rotation * Vector3.forward * bulletSpeed);
			}
		}
		public void OnMovement(InputAction.CallbackContext ctx) => Movement = ctx.ReadValue<Vector2>();
	}
}