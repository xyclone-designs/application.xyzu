
namespace System.Collections.Generic
{
	public class EqualityComparerDefault<T> : IEqualityComparer<T>
	{
		public EqualityComparerDefault() { }
		public EqualityComparerDefault(Func<T, T, bool> funcequals) : this(funcequals, null) { }
		public EqualityComparerDefault(Func<T, int> funcgethashcode) : this(null, funcgethashcode) { }
		public EqualityComparerDefault(Func<T, T, bool>? funcequals, Func<T, int>? funcgethashcode) 
		{
			FuncEquals = funcequals;
			FuncGetHashCode = funcgethashcode;
		}

		public Func<T, T, bool>? FuncEquals { get; set; }
		public Func<T, int>? FuncGetHashCode { get; set; }

		public bool Equals(T x, T y)
		{
			return FuncEquals?.Invoke(x, y) ?? x?.Equals(y) ?? y?.Equals(x) ?? false;
		}
		public int GetHashCode(T obj)
		{
			return FuncGetHashCode?.Invoke(obj) ?? obj?.GetHashCode() ?? 0;
		}
	}
}
