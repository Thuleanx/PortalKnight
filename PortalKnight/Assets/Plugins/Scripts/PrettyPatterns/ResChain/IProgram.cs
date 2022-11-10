namespace Thuleanx.PrettyPatterns.ResChain {
	public interface IProgram<T> {
		int GetPriority();
		T Process(T data);
	}
}