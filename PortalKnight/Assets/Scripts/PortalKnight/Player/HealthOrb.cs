using UnityEngine;
using Thuleanx.Utils;
using Thuleanx.Audio;
using FMODUnity;

namespace Thuleanx.PortalKnight {
	public class HealthOrb : MonoBehaviour {
		public Player Player {get; private set; }

		[SerializeField, Range(1,5)] int healthRestored;
		[SerializeField, Range(0, 64)] float trackingDecceleration = 4;
		[SerializeField, Range(0, 5)] float trackingRange = 3;
		[SerializeField, Range(0,1)] float collectingRange = 0.2f;
		[SerializeField, Range(0, 30)] float maxSpeed = 10;
		[SerializeField] EventReference collectSound;
		Vector3 velocity;

		public void Initialize(Player player) {
			Player = player;
			velocity = Vector3.zero;
		}

		void Update() {
			Vector3 displacement = (Player.transform.position - transform.position);
			displacement.y = 0;

			if (Player.Status.Health != Player.Status.MaxHealth) {
				if (displacement.sqrMagnitude <= collectingRange * collectingRange) 
					Collect();
				else if (displacement.sqrMagnitude <= trackingRange * trackingRange) {
					float speed = Mathf.Lerp(maxSpeed, 0.5f, displacement.magnitude / trackingRange);
					if (Time.deltaTime > 0)
						velocity = Vector3.ClampMagnitude(displacement.normalized * speed, displacement.magnitude / Time.deltaTime);
				}
			} 

			transform.position += velocity * Time.deltaTime;
			velocity = Mathx.Damp(Vector3.Lerp, velocity, Vector3.zero, trackingDecceleration, Time.deltaTime);
		}

		void Collect() {
			Player.Puppet.Heal(1);
			gameObject.SetActive(false);
			AudioManager.instance?.PlayOneShot(collectSound);
		}
	}
}