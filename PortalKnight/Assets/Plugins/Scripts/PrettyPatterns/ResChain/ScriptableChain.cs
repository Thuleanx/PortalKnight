using System.Collections.Generic;
using UnityEngine;

namespace Thuleanx.PrettyPatterns.ResChain {
	public abstract class ScriptableChain<T> : ScriptableObject, IChain<T> {
		List<IProgram<T>> programs = new List<IProgram<T>>();

		public T Assemble(T input) {
			foreach (IProgram<T> program in programs)
				input = program.Process(input);
			return input;
		}

		public void Attach(IProgram<T> program) {
			programs.Add(program);
			programs.Sort((i1, i2) => i1.GetPriority() - i2.GetPriority());
		}

		public bool Dettach(IProgram<T> program) 
			=>programs.Remove(program);
	}
}