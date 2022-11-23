using UnityEngine;
using UnityEngine.Events;
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
		[SerializeField, ColorUsage(true, true)] Color fillFlashColor;
		[SerializeField, ColorUsage(true, true)] Color pulsingColor;
		[SerializeField, Range(0, 2)] float flashDuration;
		[SerializeField, Range(0, 2), Tooltip("Number of pulses per second")] float pulseFrequency;
		[SerializeField] UnityEvent OnFill;
		[SerializeField] UnityEvent OnUse;

		bool manaFull => Player.Mana == Player.MaxMana;
		bool fillFlash;
		Timer flashing;
		float timeOffsetPulsing;

		void Awake() {
			Player = FindObjectOfType<Player>(); // permitted on awake
			Slider = GetComponent<Slider>();
		}

		void Start() {
			Player.OnManaGained.AddListener(OnManaGained);
			Player.OnManaUse.AddListener(OnManaUse);
		}

		void Update() {
			Slider.value = Player.Mana / Player.MaxMana;
			if (flashing) {
				Material.SetColor(EMISSION_SHADER_NAME, Color.Lerp(fillFlash ? fillFlashColor : flashColor, normalColor, flashing.ElapsedFraction));
			} else if (manaFull) {
				float pulsingValue = Mathf.Sin((Time.time - timeOffsetPulsing) * 2 * Mathf.PI * pulseFrequency) / 2 + 1;
				Material.SetColor(EMISSION_SHADER_NAME, Color.Lerp(normalColor, pulsingColor, pulsingValue));
			} else TurnOffEmission();
		}

		void OnDestroy() {
			TurnOffEmission();
			Player.OnManaGained.RemoveListener(OnManaGained);
			Player.OnManaUse.RemoveListener(OnManaUse);
		}

		[Button]
		void StartFlash() {
			Material.EnableKeyword("_EMISSION");
			fillFlash = Player.Mana == Player.MaxMana;
			flashing = flashDuration;
			if (manaFull) OnFill?.Invoke();
			timeOffsetPulsing = Time.time + flashDuration;
		}

		void TurnOffEmission() {
			// Material.DisableKeyword("_EMISSION");
			Material.SetColor(EMISSION_SHADER_NAME, normalColor);
		}


		void OnManaGained() => StartFlash();
		void OnManaUse() => OnUse?.Invoke();

	}
}