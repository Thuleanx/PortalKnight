using UnityEngine;
using System.Collections;

using DG.Tweening;

using Thuleanx.AI.FSM;
using Thuleanx.Utils;

namespace Thuleanx.PortalKnight {
	public partial class Player {
		public class PlayerDashState : State<Player> {
			Vector3 dashDirection;
			Vector3 beforeDashVelocity;

			Timer 	onCooldown;

			public override bool CanEnter(Player player) => !onCooldown;

			public override void Begin(Player player) {
				dashDirection = player.InputMovementToWorldDir(player.lastNonZeroMovement);
				beforeDashVelocity = player.Velocity;
			}

			public override void End(Player player) {
				player.Drag = 0;
				onCooldown = player.dashCooldown;
			}

			public override IEnumerator Coroutine(Player player) {
				Tween tween = DOVirtual.Float(player.dashDrag, 0, player.dashDuration, (float x) => {
					player.Drag = x;
				});
				player.Velocity = dashDirection * player.dashSpeed;

				// you can call the FX right here
				yield return new WaitForSeconds(player.dashDuration);

				tween.Kill();

				stateMachine.SetState((int) Player.State.Neutral);
			}
		}
	}
}