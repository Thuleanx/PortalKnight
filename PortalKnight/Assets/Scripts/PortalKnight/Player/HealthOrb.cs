using UnityEngine;
using Thuleanx.Utils;

namespace Thuleanx.PortalKnight {
	public class HealthOrb : MonoBehaviour {
		public Player Player {get; private set; }

		[SerializeField, Range(1,5)] int healthRestored;
		[SerializeField, Range(0, 5)] float trackingRange = 3;
		[SerializeField, Range(0,1)] float collectingRange = 0.2f;
		[SerializeField, Range(0, 30)] float maxSpeed = 10;

		public void Initialize(Player player) {
			Player = player;
		}

		void Update() {
			Vector3 displacement = (Player.transform.position - transform.position);
			displacement.y = 0;

			if (Player.Status.Health != Player.Status.MaxHealth) {
				if (displacement.sqrMagnitude <= collectingRange * collectingRange) 
					Collect();
				else if (displacement.sqrMagnitude <= trackingRange * trackingRange) {
					float speed = Mathf.Lerp(maxSpeed, 0.5f, displacement.magnitude / trackingRange);
					transform.position += Vector3.ClampMagnitude(displacement.normalized * speed * Time.deltaTime, displacement.magnitude);
				}
			}
		}

		void Collect() {
			// TODO don't do this
			Player.Status.Health++;
			gameObject.SetActive(false);
		}
	}
}