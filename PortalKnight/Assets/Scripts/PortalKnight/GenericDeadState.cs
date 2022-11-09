using UnityEngine;
using Thuleanx.AI.FSM;
using System.Collections;

namespace Thuleanx.PortalKnight {
	public class GenericDeadState<T> : State<T> where T : Movable {
		public override void Begin(T agent) {
			agent.ResetMovements();
		}

		public override IEnumerator Coroutine(T agent) {
			Debug.Log("DEAD");
			yield return new WaitForSeconds(3);
			agent.gameObject.SetActive(false);
		}
	}
}