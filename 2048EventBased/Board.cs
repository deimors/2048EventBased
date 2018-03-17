using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Functional.Maybe;

namespace _2048EventBased
{
	internal class Board
	{
		private readonly ImmutableDictionary<Position, int> _cells;

		public static Board Empty(int size) => new Board(size, ImmutableDictionary<Position, int>.Empty);

		private Board(int size, ImmutableDictionary<Position, int> cells)
		{
			Size = size;
			_cells = cells ?? throw new ArgumentNullException(nameof(cells));
		}

		public int Size { get; }

		public IEnumerable<Position> AllPositions
			=> Enumerable.Range(0, Size).SelectMany(row => Enumerable.Range(0, Size).Select(column => new Position(row, column)));

		public IEnumerable<Position> EmptyPositions
			=> AllPositions.Where(position => !_cells.ContainsKey(position));

		public bool HasValue(int value)
			=> _cells.ContainsValue(value);

		public bool IsOnBoard(Position position)
			=> position.Row >= 0 && position.Row < Size && position.Column >= 0 && position.Column < Size;

		public Maybe<int> this[Position position]
			=> _cells.ContainsKey(position)
				? _cells[position].ToMaybe()
				: Maybe<int>.Nothing;

		public Board Add(Position position, int number)
			=> new Board(Size, _cells.Add(position, number));

		public Board Move(Position origin, Position target) 
			=> new Board(
				Size,
				_cells.Add(target, _cells[origin])
					.Remove(origin)
			);

		public Board Merge(Position origin1, Position origin2, Position target)
			=> Merge(origin1, origin2, target, _cells[origin1] + _cells[origin2]);

		private Board Merge(Position origin1, Position origin2, Position target, int value)
			=> new Board(
				Size,
				_cells
					.Remove(origin1)
					.Remove(origin2)
					.Add(target, value)
			);
	}
}