using System;
using System.Collections.Immutable;
using Functional.Maybe;

namespace _2048EventBased
{
	internal class Board
	{
		public static readonly Board Empty = new Board(ImmutableDictionary<Position, int>.Empty);

		private readonly ImmutableDictionary<Position, int> _cells;
		
		private Board(ImmutableDictionary<Position, int> cells)
		{
			_cells = cells ?? throw new ArgumentNullException(nameof(cells));
		}

		public Maybe<int> this[Position position]
			=> _cells.ContainsKey(position)
				? _cells[position].ToMaybe()
				: Maybe<int>.Nothing;

		public Board Add(Position position, int number)
			=> new Board(_cells.Add(position, number));

		public Board Move(Position origin, Position target) 
			=> new Board(
				_cells.Add(target, _cells[origin])
					.Remove(origin)
			);

		public Board Merge(Position origin1, Position origin2, Position target)
			=> Merge(origin1, origin2, target, _cells[origin1] + _cells[origin2]);

		private Board Merge(Position origin1, Position origin2, Position target, int value)
			=> new Board(
				_cells
					.Remove(origin1)
					.Remove(origin2)
					.Add(target, value)
			);
	}
}