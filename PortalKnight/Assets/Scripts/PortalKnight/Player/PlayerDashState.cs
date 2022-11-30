using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using DG.Tweening;

using Thuleanx.AI.FSM;
using Thuleanx.Utils;
using Thuleanx.PortalKnight.Effects;

namespace Thuleanx.PortalKnight {
	public partial class Player {
		public class PlayerDashState : State<Player> {
			Vector3 dashDirection;
			Vector3 beforeDashVelocity;
			Timer 	onCooldown;

			public override bool CanEnter(Player player) => !onCooldown;

			public override void Begin(Player player) {
				dashDirection = player.Input.MovementToWorldDir(player.Input.lastNonZeroMovement);
				if (dashDirection == Vector3.zero) dashDirection = Vector3.right;
				beforeDashVelocity = player.Velocity;
				player.TurnToFaceImmediate(dashDirection);
				player.Puppet.GiveIframes(player.dashIframes);
				player.OnDash?.Invoke();
				player.flashEffect?.Flash(player.dashMaterial, player.dashDuration, true);
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
				player.Anim.SetTrigger(player.dashTrigger);

				// you can call the FX right here
				yield return new WaitForSeconds(player.dashDuration);

				tween.Kill();

				player.Anim.SetTrigger(player.neutralTrigger);
				player.StateMachine.SetState((int) Player.State.Neutral);
			}
		}
	}
}