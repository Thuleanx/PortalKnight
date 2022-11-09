using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Thuleanx.PortalKnight;

namespace Thuleanx.PortalKnight.UI {
	[RequireComponent(typeof(RectTransform))]
	public class PlayerHealthUI : MonoBehaviour {
		public Player Player {get; private set; }

		List<HealthKnotch> healthKnotches = new List<HealthKnotch>();
		[SerializeField] GameObject healthPrefab;

		void Start() {
			Player = FindObjectOfType<Player>();
			Construct();
		}

		void Update() => Redraw();

		void Construct() {
			for (int i = 0; i < Player.Status.MaxHealth; i++) {
				GameObject healthKnotch = Instantiate(healthPrefab, transform);
				healthKnotches.Add(healthKnotch.GetComponent<HealthKnotch>());
			}
		}

		void Redraw() {
			for (int i = 0; i < Player.Status.MaxHealth; i++) 
				healthKnotches[i].SetFilled(Player.Status.Health > i);
		}
	}
}