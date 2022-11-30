using System.Collections.Generic;
using UnityEngine;
using Thuleanx.Utils;

namespace Thuleanx.PortalKnight.Effects {
	public class FlashMaterialEffect : MonoBehaviour {
		[field:SerializeField] public List<Renderer> Renderers {get; private set;}
		List<Material> materials;
		bool isFlashing;

		Timer flashing;

		void Awake() {
			materials = new List<Material>();
			for (int i = 0; i < Renderers.Count; i++) 
				materials.Add(Renderers[i].material);
		}

		public void Flash(Material material, float duration, bool replace = false) {
			if (isFlashing && !replace) return;
			isFlashing = true;
			flashing = duration;
			AssignMaterials(material);
		}

		void Update() {
			if (!flashing && isFlashing) {
				ResetMaterials();
				isFlashing = false;
			}
		}

		void AssignMaterials(Material material) {
			for (int i = 0; i < Renderers.Count; i++) {
				Renderers[i].material = material;
				SmearDataUpdate updater = Renderers[i].GetComponent<SmearDataUpdate>();
				if (updater) updater.Write = true;
			}
		}

		void ResetMaterials() {
			for (int i = 0; i < Renderers.Count; i++) {
				Renderers[i].material = materials[i];
				SmearDataUpdate updater = Renderers[i].GetComponent<SmearDataUpdate>();
				if (updater) updater.Write = false;
			}
		}
	}
}