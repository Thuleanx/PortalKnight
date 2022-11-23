using UnityEngine;
using UnityEngine.UI;
using Thuleanx.Utils;
using NaughtyAttributes;

namespace Thuleanx.PortalKnight.UI {
	[RequireComponent(typeof(RectTransform))]
	[RequireComponent(typeof(Slider))]
	public class ManaUI : MonoBehaviour {
		const string EMISSION_SHADER_NAME = "_EmissionColor";

		public Slider Slider {get; private set; }
		public Player Player {get; private set; }

		[field:SerializeField]
		public Image Image {get; private set;}
		Material Material => Image.material;
		
		[SerializeField, ColorUsage(true, true)] Color normalColor;
		[SerializeField, ColorUsage(true, true)] Color flashColor;
		[SerializeField, Range(0, 2)] float flashDuration;

		Timer flashing;

		void Awake() {
			Player = FindObjectOfType<Player>(); // permitted on awake
			Slider = GetComponent<Slider>();
		}

		void Start() {
			Player.OnManaGained.AddListener(OnManaGained);
		}

		void Update() {
			Slider.value = Player.Mana / Player.MaxMana;
			if (flashing) {
				Material.SetColor(EMISSION_SHADER_NAME, Color.Lerp(flashColor, normalColor, flashing.ElapsedFraction));
			} else TurnOffEmission();
		}

		void OnDestroy() {
			TurnOffEmission();
			Player.OnManaGained.RemoveListener(OnManaGained);
		}

		[Button]
		void StartFlash() {
			Material.EnableKeyword("_EMISSION");
			Material.SetColor(EMISSION_SHADER_NAME, flashColor);
			flashing = flashDuration;
		}

		void TurnOffEmission() {
			// Material.DisableKeyword("_EMISSION");
			Material.SetColor(EMISSION_SHADER_NAME, normalColor);
		}


		void OnManaGained() => StartFlash();
	}
}