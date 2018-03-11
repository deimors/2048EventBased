using Functional.Maybe;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _2048EventBased
{
	public class Game
	{
		private readonly Maybe<int>[,] _cells = new Maybe<int>[4,4];

		private Maybe<int> this[Position position] 
			=> _cells[position.Row, position.Column];

		private IEnumerable<Position> AllPositions
			=> Enumerable.Range(0, _cells.GetLength(0)).SelectMany(row => Enumerable.Range(0, _cells.GetLength(1)).Select(column => new Position(row, column)));

		public event Action<NumberAddedEvent> NumberAdded;
		public event Action<NumberMovedEvent> NumberMoved;

		public int this[int row, int column]
		{
			set
			{
				_cells[row, column] = value.ToMaybe();
				NumberAdded?.Invoke(new NumberAddedEvent(value, row, column));
			}
		}

		public void Move(Direction direction)
		{
			foreach (var position in AllPositions)
			{
				FindMoveTarget(position, direction).Match(
					target => NumberMoved?.Invoke(new NumberMovedEvent(2, position, target)),
					() => { }
				);
			}
		}
		
		private readonly IReadOnlyDictionary<Direction, Position> _increments = new Dictionary<Direction, Position>
		{
			{ Direction.Down, new Position(1, 0) },
			{ Direction.Up, new Position(-1, 0) },
			{ Direction.Left, new Position(0, -1) },
			{ Direction.Right, new Position(0, 1) }
		};
		
		private Maybe<Position> FindMoveTarget(Position origin, Direction direction)
			=> this[origin].SelectOrElse(
				number =>
				{
					var increment = _increments[direction];

					return origin.Project(increment)
						.TakeWhile(IsInBounds)
						.LastMaybe();
				},
				() => Maybe<Position>.Nothing
			);
		
		private bool IsInBounds(Position position)
			=> position.Row >= 0 
			   && position.Row < _cells.GetLength(0) 
			   && position.Column >= 0 
			   && position.Column < _cells.GetLength(1);
	}
}
