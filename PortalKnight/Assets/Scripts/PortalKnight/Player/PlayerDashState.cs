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
			List<Material> meshMaterials;

			public override bool CanEnter(Player player) => !onCooldown;

			public override void Begin(Player player) {
				dashDirection = player.Input.MovementToWorldDir(player.Input.lastNonZeroMovement);
				if (dashDirection == Vector3.zero) dashDirection = Vector3.right;
				beforeDashVelocity = player.Velocity;
				player.TurnToFaceImmediate(dashDirection);
				player.Puppet.GiveIframes(player.dashIframes);
				player.OnDash?.Invoke();
				meshMaterials = new List<Material>();
				for (int i = 0; i < player.Renderers.Count; i++) {
					meshMaterials.Add(player.Renderers[i].material);
					player.Renderers[i].material = player.dashMaterial;
					player.Renderers[i].GetComponent<SmearDataUpdate>().Write = true;
				}
			}

			public override void End(Player player) {
				player.Drag = 0;
				onCooldown = player.dashCooldown;
				for (int i = 0; i < player.Renderers.Count; i++) {
					player.Renderers[i].material = meshMaterials[i];
					player.Renderers[i].GetComponent<SmearDataUpdate>().Write = false;
				}
				meshMaterials.Clear();
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