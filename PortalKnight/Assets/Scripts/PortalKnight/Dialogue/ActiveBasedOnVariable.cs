using UnityEngine;

namespace Thuleanx.PortalKnight.Dialogue {
	public class ActiveBasedOnVariable : MonoBehaviour {
		[SerializeField] string Condition;
		[SerializeField] bool Expected;

		public VariableStorage Storage {get; private set; }

		void Awake() {
			Storage = App.instance.GetComponentInChildren<VariableStorage>();
		}

		void Update() {
			if (Storage.TryGetValue<bool>(Condition, out bool value)) 
				if (value ^ Expected)
					gameObject.SetActive(false);
		}
	}
}