using System.Collections.Generic;

namespace _2048EventBased
{
	internal static class PositionExtensions
	{
		public static IEnumerable<Position> Project(this Position origin, Position increment)
		{
			while (true)
			{
				var next = origin + increment;

				yield return next;

				origin = next;
			}
		}
	}
}