using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Thuleanx.Utils;
using NaughtyAttributes;
using DG.Tweening;

namespace Thuleanx.PortalKnight.UI {
	[RequireComponent(typeof(RectTransform))]
	[RequireComponent(typeof(Slider))]
	public class ManaUI : MonoBehaviour {
		const string EMISSION_SHADER_NAME = "_EmissionColor";

		public Slider Slider {get; private set; }
		public Player Player {get; private set; }
		public RectTransform rectTransform {get; private set;}

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
			rectTransform = GetComponent<RectTransform>();
		}

		void Start() {
			Player.OnManaGained.AddListener(OnManaGained);
			Player.OnManaUse.AddListener(OnManaUse);
			Player.OnManaInsufficient.AddListener(OnInsufficientMana);
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
			Player.OnManaInsufficient.RemoveListener(OnInsufficientMana);
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

		[Header("Insufficient Mana Effect")]
		[SerializeField, Range(0,1)] float shakeDuration = 0.2f;
		[SerializeField, Range(0,100)] float shakeStrength = 25f;
		[SerializeField, Range(0,100)] int shakeVibrato = 10;
		[SerializeField] RectTransform shakeTarget;
		Vector3 shakeOriginalPos;
		Tween shakeTween;
		[Button("Start shake")]
		void OnInsufficientMana() {
			shakeTween?.Kill();
			shakeOriginalPos = shakeTarget.anchoredPosition3D;
			shakeTween = shakeTarget.DOShakeAnchorPos(shakeDuration, shakeStrength, shakeVibrato).OnKill(() => {
				shakeTarget.anchoredPosition3D = shakeOriginalPos;
			});
		}
		void OnManaGained() => StartFlash();
		void OnManaUse() => OnUse?.Invoke();

	}
}