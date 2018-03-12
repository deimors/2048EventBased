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
		{
			get => _cells[position.Row, position.Column];
			set => _cells[position.Row, position.Column] = value;
		}
		
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
			foreach (var origin in GetPositionsForDirection(direction))
			{
				this[origin].Match(
					number => FindMoveTarget(origin, direction).Match(
						target => MoveNumberTo(number, origin, target),
						() => { }
					),
					() => { }
				);
			}
		}

		private void MoveNumberTo(int number, Position origin, Position target)
		{
			this[origin] = Maybe<int>.Nothing;
			this[target] = number.ToMaybe();
			
			NumberMoved?.Invoke(new NumberMovedEvent(number, origin, target));
		}

		private IEnumerable<Position> GetPositionsForDirection(Direction direction) 
			=> RowsOrderedFor(direction)
				.SelectMany(
					row => ColumnsOrderedFor(direction)
							.Select(column => new Position(row, column))
				);

		private IEnumerable<int> RowsOrderedFor(Direction direction)
			=> RowsOrderedFor(direction, Enumerable.Range(0, _cells.GetLength(0)));

		private static IEnumerable<int> RowsOrderedFor(Direction direction, IEnumerable<int> rows)
			=> direction == Direction.Down ? rows.Reverse() : rows;

		private IEnumerable<int> ColumnsOrderedFor(Direction direction)
			=> ColumnsOrderedFor(direction, Enumerable.Range(0, _cells.GetLength(1)));

		private static IEnumerable<int> ColumnsOrderedFor(Direction direction, IEnumerable<int> columns)
			=> direction == Direction.Right ? columns.Reverse() : columns;

		private readonly IReadOnlyDictionary<Direction, Position> _increments = new Dictionary<Direction, Position>
		{
			{ Direction.Down, new Position(1, 0) },
			{ Direction.Up, new Position(-1, 0) },
			{ Direction.Left, new Position(0, -1) },
			{ Direction.Right, new Position(0, 1) }
		};

		private Maybe<Position> FindMoveTarget(Position origin, Direction direction)
			=> this[origin].SelectOrElse(
				number => SelectMoveTarget(GetMoveCandidates(origin, direction), number),
				() => Maybe<Position>.Nothing
			);

		private IEnumerable<Position> GetMoveCandidates(Position origin, Direction direction)
			=> origin.Project(_increments[direction]).TakeWhile(IsInBounds);

		private Maybe<Position> SelectMoveTarget(IEnumerable<Position> moveCandidates, int originNumber)
			=> moveCandidates
				.Pairwise()
				.FirstMaybe(pair => this[pair.Item2].SelectOrElse(value => value != originNumber, () => false))
				.SelectOrElse(pair => pair.Item1.ToMaybe(), moveCandidates.LastMaybe);
		
		private bool IsInBounds(Position position)
			=> position.Row >= 0 
			   && position.Row < _cells.GetLength(0) 
			   && position.Column >= 0 
			   && position.Column < _cells.GetLength(1);
	}
}
