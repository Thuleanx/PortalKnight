using UnityEngine;
using Yarn;
using Yarn.Unity;

namespace Thuleanx.PortalKnight.Dialogue {
	public class VariableSetter : MonoBehaviour, IVariableStorage {
		public VariableStorage Storage {get; private set; }
		void Awake() {
			Storage = App.instance.GetComponentInChildren<VariableStorage>();
		}
		public void Clear() => Storage.Clear();
		public void SetValue(string variableName, string stringValue) => Storage.SetValue(variableName, stringValue);
		public void SetValue(string variableName, float floatValue) => Storage.SetValue(variableName, floatValue);
		public void SetValue(string variableName, bool boolValue) => Storage.SetValue(variableName, boolValue);
		public bool TryGetValue<T>(string variableName, out T result) => Storage.TryGetValue<T>(variableName, out result);

		public void SetBit(string variableName) => SetValue(variableName, true);
	}
}