using System.Collections;
using UnityEngine;

namespace Thuleanx.AI.FSM {
	public abstract class StateMachine<Agent> : MonoBehaviour {
		bool isAutoUpdate;
		int defaultState;

		Agent agent;
		State<Agent>[] States;
		Coroutine currentCoroutine;

		int _currentState;
		public int State { get => _currentState; private set {
			if (value != _currentState) {
				if (currentCoroutine != null) StopCoroutine(currentCoroutine);
				States[_currentState]?.End(agent);
				_currentState = value;
				States[_currentState]?.Begin(agent);

				IEnumerator enumerator = States[_currentState]?.Coroutine(agent);
				if (enumerator != null) currentCoroutine = StartCoroutine(enumerator);
			}
		}}

		public void ConstructMachine(Agent agent, int numberOfStates, int defaultStateIndex, bool isAutoUpdate = true) {
			this.agent = agent;
			States = new State<Agent>[numberOfStates];
			_currentState = defaultStateIndex;
			this.isAutoUpdate = isAutoUpdate;
			this.defaultState = defaultStateIndex;
		}

		public void AssignState(int index, State<Agent> state) {
			States[index] = state;
			state.SetStateMachine(this);
		}
		public bool CanEnter(int index) => States[index] != null && States[index].CanEnter(agent);
		public void SetState(int index) => State = index;
		public bool TrySetState(int index) {
			if (index == -1 || index == State) return false;
			bool canSet = CanEnter(index);
			if (canSet) State = index;
			return canSet;
		}

		public abstract void Construct();
		public void Init() {
			_currentState = defaultState;
			// for (int i = 0; i < States.Length; i++) 
			// 	if (States[i] == null) 
			// 		Debug.LogWarning("State " + i + " is null");
			States[_currentState].Begin(agent);
			IEnumerator enumerator = States[_currentState]?.Coroutine(agent);
			if (enumerator != null) currentCoroutine = StartCoroutine(enumerator);
		}

		public void RunUpdate() {
			while (TrySetState(States[State].Transition(agent)));
			TrySetState(States[State].Update(agent));
		}

		public void RunFixUpdate() {
			TrySetState(States[State].FixUpdate(agent));
		}

		void Update() { 
			if (isAutoUpdate) RunUpdate();
		}

		void FixedUpdate() {
			if (isAutoUpdate) RunFixUpdate();
		}
	}

}