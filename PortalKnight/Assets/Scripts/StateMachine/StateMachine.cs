using UnityEngine;
using System.Collections;

namespace Thuleanx.AI.FSM {
	public class StateMachine<Agent> : MonoBehaviour {
		bool isAutoUpdate;
		int defaultState;

		Agent agent;
		State<Agent>[] States;
		Coroutine currentCoroutine;

		int _currentState;
		public int State { get => _currentState; set {
			if (value != _currentState) {
				if (currentCoroutine != null) StopCoroutine(currentCoroutine);
				States[_currentState]?.End(agent);
				_currentState = value;
				States[_currentState]?.Begin(agent);
				currentCoroutine = StartCoroutine(States[_currentState].Coroutine(agent));
			}
		}}

		public void ConstructMachine(Agent agent, int numberOfStates, int defaultStateIndex, bool isAutoUpdate = true) {
			this.agent = agent;
			States = new State<Agent>[numberOfStates];
			_currentState = defaultStateIndex;
			this.isAutoUpdate = isAutoUpdate;
			this.defaultState = defaultStateIndex;
		}

		public void SetState(int index, State<Agent> state) => States[index] = state;

		public void Init() {
			_currentState = defaultState;
			for (int i = 0; i < States.Length; i++) 
				if (States[i] == null) 
					Debug.LogWarning("State " + i + " is null");
			States[_currentState].Begin(agent);
		}

		public void RunUpdate() {
			int nxtState = 0;
			while (true) {
				nxtState = States[State].Transition(agent);
				if (nxtState == -1 || nxtState == State) break;
				State = nxtState;
			}
			int nxt = States[State].Update(agent);
			if (nxt != -1) State = nxt;
		}

		public void RunFixUpdate() {
			int nxt = States[State].FixUpdate(agent);
			if (nxt != -1) State = nxt;
		}

		void Update() { 
			if (isAutoUpdate) RunUpdate();
		}

		void FixedUpdate() {
			if (isAutoUpdate) RunFixUpdate();
		}
	}

}