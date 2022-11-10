namespace Thuleanx.PrettyPatterns.ResChain {
	public interface IChain<T> {
		void Attach(IProgram<T> program);
		bool Dettach(IProgram<T> program);
		T Assemble(T input);
	}
}