using System;
using System.Collections.Generic;

namespace _2048EventBased
{
	internal static class EnumerableExtensions
	{
		public static IEnumerable<Tuple<T, T>> Pairwise<T>(this IEnumerable<T> source)
		{
			using (var enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
					yield break;

				var first = enumerator.Current;

				while (enumerator.MoveNext())
				{
					var second = enumerator.Current;
					yield return Tuple.Create(first, second);
					first = second;
				}
			}
		}
	}
}