using UnityEngine;
using NaughtyAttributes;
using TMPro;
using WizOsu.Dialogue;

namespace Thuleanx.PortalKnight.Dialogue  {
	[ExecuteAlways]
	[RequireComponent(typeof(RectTransform))]
	public class SpeechBubble : MonoBehaviour {
		[SerializeField, ReadOnly] Speaker attachedSpeaker = null;
		[SerializeField, Required] public TextMeshProUGUI textObj;
		[SerializeField] Vector2 padding;
		[SerializeField] Vector2 screenPadding = Vector2.one * .15f;
		RectTransform rectTransform;
		Canvas canvas;

		void Awake() {
			rectTransform = GetComponent<RectTransform>();
			canvas = GetComponentInParent<Canvas>();
		}

		public void Setup(Speaker speaker, string text) {
			attachedSpeaker = speaker;
			textObj.SetText(text);
			textObj.ForceMeshUpdate();
			ResizeToTextContent();
		}

		public void ResizeToTextContent() {
			Vector2 textSize = textObj.GetRenderedValues();
			rectTransform.sizeDelta = textSize + padding * 2;
		}

		void LateUpdate() {
			Reposition();
		}

		public void Reposition() {
			if (attachedSpeaker) {
				Plane plane = new Plane(Vector3.forward, canvas.transform.position);
				Vector3 screenPoint = Camera.main.WorldToScreenPoint(attachedSpeaker.transform.position + (Vector3) attachedSpeaker.offset);
				Ray ray = Camera.main.ScreenPointToRay(screenPoint);
				float dist = 0;
				plane.Raycast(ray, out dist);
				Vector3 corPos = ray.GetPoint(dist);
				transform.position = corPos;
				BoundToScreen();
			}
		}

		public void BoundToScreen() {
			Plane plane = new Plane(Vector3.forward, canvas.transform.position);
			Ray ray = Camera.main.ViewportPointToRay(Vector3.zero);
			Ray ray2 = Camera.main.ViewportPointToRay(Vector3.one);
			float dist = 0, dist2;
			plane.Raycast(ray, out dist);
			plane.Raycast(ray2, out dist2);
			Vector3 botLeft = ray.GetPoint(dist);
			Vector3 topRight = ray2.GetPoint(dist2);

			Vector3 dim = transform.lossyScale * rectTransform.sizeDelta;

			botLeft += (dim + (Vector3) screenPadding) / 2;
			topRight -= (dim + (Vector3) screenPadding) / 2;

			transform.position = new Vector3(
				Mathf.Clamp(transform.position.x, botLeft.x, topRight.x),
				Mathf.Clamp(transform.position.y, botLeft.y, topRight.y),
				transform.position.z
			);
		}

		private void OnDisable() => attachedSpeaker = null;
	}
}