using UnityEngine;
using Thuleanx.AI.FSM;
using System.Collections;

namespace Thuleanx.PortalKnight {
	public class GenericDeadState<T> : State<T> where T : Alive {
		public override void Begin(T agent) {
			agent.ResetMovements();
			agent.Vanquish();
		}

		public override IEnumerator Coroutine(T agent) {
			yield return new WaitForSeconds(3);
			agent.gameObject.SetActive(false);
		}
	}
}