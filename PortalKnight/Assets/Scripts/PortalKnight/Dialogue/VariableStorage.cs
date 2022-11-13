using UnityEngine;
using System.Collections.Generic;

using Yarn;
using Yarn.Unity;
using NaughtyAttributes;

namespace Thuleanx.PortalKnight.Dialogue {
	public class VariableStorage : InMemoryVariableStorage {
		[SerializeField] string deathCountTag = "$death_count";

		public int GetDeathCount() {
			int result = 0;
			if (TryGetValue(deathCountTag, out float resIntemediate)) 
				result = Mathf.RoundToInt(resIntemediate);
			return result;
		}

		public void IncrementDeath() => SetValue(deathCountTag, GetDeathCount() + 1);
	}
}