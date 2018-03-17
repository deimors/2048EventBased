using System.Collections.Generic;
using System.Linq;

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

		private static readonly IEnumerable<Position> _neighborOffsets = new[]
		{
			new Position(1, 0), new Position(-1, 0), new Position(0, 1), new Position(0, -1),
		};

		public static IEnumerable<Position> Neighbors(this Position origin)
			=> _neighborOffsets.Select(offset => origin + offset);
	}
}