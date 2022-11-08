using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;
using Yarn.Unity;
using NaughtyAttributes;
using DG.Tweening;

namespace Thuleanx.PortalKnight.Dialogue {
	[RequireComponent(typeof(CanvasGroup))]
	public class SpeechView : DialogueViewBase {
		public CanvasGroup CanvasGroup { get; private set;  }
		[SerializeField, Required] SpeechBubble speechBubble;
		[SerializeField, Range(0, 10f)] float holdTime = 1f;
		[SerializeField, Range(0, 10f)] float fadeInTime = 1f;
		[SerializeField, Range(0, 10f)] float fadeOutTime = 1f;
		[SerializeField, Range(.01f, 100f)] float lettersPerSecond;

		[SerializeField] UnityEvent onCharTyped;

		LocalizedLine currentLine = null;

		void Awake() {
			CanvasGroup = GetComponent<CanvasGroup>();
			CanvasGroup.alpha = 0;
			CanvasGroup.blocksRaycasts = false;
		}

        /// <inheritdoc/>
        public override void DismissLine(Action onDismissalComplete)
        {
            currentLine = null;
            StartCoroutine(DismissLineInternal(onDismissalComplete));
        }

        private IEnumerator DismissLineInternal(Action onDismissalComplete)
        {
			// disabling interaction temporarily while dismissing the line
			// we don't want people to interrupt a dismissal
			var interactable = CanvasGroup.interactable;
			CanvasGroup.interactable = false;

			// if (useFadeEffect)
			// {
			//     yield return StartCoroutine(Effects.FadeAlpha(canvasGroup, 1, 0, fadeOutTime, currentStopToken));
			//     currentStopToken.Complete();
			// }

			CanvasGroup.DOFade(0f, fadeInTime).From(1).Play();
			yield return new WaitForSeconds(fadeOutTime);

			speechBubble.gameObject.SetActive(false);
			CanvasGroup.alpha = 0;
			CanvasGroup.blocksRaycasts = false;
			CanvasGroup.interactable = interactable;
			onDismissalComplete();
        }

		        /// <inheritdoc/>
        public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
        {
            // Stop any coroutines currently running on this line view (for
            // example, any other RunLine that might be running)
            StopAllCoroutines();

            // Begin running the line as a coroutine.
            StartCoroutine(RunLineInternal(dialogueLine, onDialogueLineFinished));
        }

        private IEnumerator RunLineInternal(LocalizedLine dialogueLine, Action onDialogueLineFinished)
        {
            IEnumerator PresentLine()
            {
				GetComponentInChildren<FMODUnity.StudioEventEmitter>()?.Play();
				Speaker attachedSpeaker = null;
				if (dialogueLine.CharacterName != null && dialogueLine.CharacterName.Length > 0) 
					attachedSpeaker = SpeakerManager.instance?.GetSpeaker(dialogueLine.CharacterName);

				speechBubble.gameObject.SetActive(true);
				speechBubble.Setup(attachedSpeaker, dialogueLine.TextWithoutCharacterName.Text);
				speechBubble.textObj.maxVisibleCharacters = 0;

				CanvasGroup.DOFade(1f, fadeInTime).From(0).Play();
				yield return new WaitForSeconds(fadeInTime);


				CanvasGroup.alpha = 1f;
				CanvasGroup.interactable = true;
				CanvasGroup.blocksRaycasts = true;

				int charCnt = dialogueLine.TextWithoutCharacterName.Text.Length;

				while (speechBubble.textObj.maxVisibleCharacters < charCnt) {
					speechBubble.textObj.maxVisibleCharacters++;
					onCharTyped?.Invoke();
					yield return new WaitForSeconds(1f / lettersPerSecond);
				}

				speechBubble.textObj.maxVisibleCharacters = int.MaxValue;

				yield return null;
			}
            currentLine = dialogueLine;

            // Run any presentations as a single coroutine. If this is stopped,
            // which UserRequestedViewAdvancement can do, then we will stop all
            // of the animations at once.
            yield return StartCoroutine(PresentLine());

			CanvasGroup.alpha = 1f;
			CanvasGroup.blocksRaycasts = true;

			// If we have a hold time, wait that amount of time, and then
			// continue.
			if (holdTime > 0) yield return new WaitForSeconds(holdTime);

            // Our presentation is complete; call the completion handler.
            onDialogueLineFinished();
        }
	}
}