using System.Collections;

namespace Thuleanx.AI.FSM {
	public abstract class State<Agent> {
		protected StateMachine<Agent> stateMachine;

		public void SetStateMachine(StateMachine<Agent> stateMachine) => this.stateMachine = stateMachine;
		public virtual void Begin(Agent agent) {}
		public virtual void End(Agent agent) {}

		public virtual int Update(Agent agent) => -1;
		public virtual int FixUpdate(Agent agent) => -1;
		public virtual int Transition(Agent agent) => -1;
		public virtual bool CanEnter(Agent agent) => true;
		
		public virtual IEnumerator Coroutine(Agent agent) => null;
	}
}