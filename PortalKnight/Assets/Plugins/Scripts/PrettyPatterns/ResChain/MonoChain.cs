using System.Collections.Generic;
using UnityEngine;

namespace Thuleanx.PrettyPatterns.ResChain {
	public abstract class MonoChain<T> : MonoBehaviour, IChain<T> {
		List<IProgram<T>> programs = new List<IProgram<T>>();

		public virtual void Start() {
			foreach (var program in GetComponents<IProgram<T>>())
				Attach(program);
		}

		public T Assemble(T input) {
			foreach (IProgram<T> program in programs)
				input = program.Process(input);
			Notify();
			return input;
		}

		protected abstract void Notify();

		public void Attach(IProgram<T> program) {
			programs.Add(program);
			programs.Sort((i1, i2) => i1.GetPriority() - i2.GetPriority());
		}

		public bool Dettach(IProgram<T> program) 
			=>programs.Remove(program);
	}
}