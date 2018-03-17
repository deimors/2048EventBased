using System;
using System.Collections.Generic;
using System.Linq;

namespace _2048EventBased
{
	internal static class EnumerableExtensions
	{
		public static IEnumerable<IEnumerable<T>> GetRunsOfMaxLength<T>(this IEnumerable<T> sequence, Func<T, T, bool> comparator, int maxLength)
		{
			while (sequence.Any())
			{
				var first = sequence.First();
				var run = sequence.Take(maxLength).TakeWhile(value => comparator(first, value));

				sequence = sequence.Skip(run.Count());
				yield return run;
			}
		}
	}
}