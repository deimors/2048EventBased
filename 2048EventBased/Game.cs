using Functional.Maybe;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

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
			=> new Board(
				_cells.Add(target, _cells[origin1] + _cells[origin2])
					.Remove(origin1)
					.Remove(origin2)
			);
	}

	public class Number
	{
		public Position Position { get; }
		public int Value { get; }

		public Number(Position position, int value)
		{
			Position = position;
			Value = value;
		}

		public override string ToString()
			=> $"{Value} @ {Position}";
	}

	public class Game
	{
		private const int Width = 4;
		private const int Height = 4;

		private Board _currentState = Board.Empty;

		public event Action<NumberAddedEvent> NumberAdded;
		public event Action<NumberMovedEvent> NumberMoved;
		public event Action<NumbersMergedEvent> NumbersMerged;

		public int this[int row, int column]
		{
			set
			{
				_currentState = _currentState.Add(new Position(row, column), value);
				NumberAdded?.Invoke(new NumberAddedEvent(value, row, column));
			}
		}

		public void Move(Direction direction)
		{
			var sequences = GetSequencesForDirection(direction);

			foreach (var sequence in sequences.Select(sequence => sequence.ToArray()))
			{
				var values = sequence.Select(pos => _currentState[pos].Select(value => new Number(pos, value))).WhereValueExist().ToArray();

				var runs = GetRuns(values);

				ProcessRuns(sequence, runs);
			}
		}

		private IEnumerable<IEnumerable<Position>> GetSequencesForDirection(Direction direction)
		{
			switch (direction)
			{
				case Direction.Right:
					return Rows.Select(row => Columns.Reverse().Select(column => new Position(row, column)));
					
				case Direction.Down:
					return Columns.Select(column => Rows.Reverse().Select(row => new Position(row, column)));

				case Direction.Left:
					return Rows.Select(row => Columns.Select(column => new Position(row, column)));

				case Direction.Up:
					return Columns.Select(column => Rows.Select(row => new Position(row, column)));

				default:
					throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
			}
		}

		public static IEnumerable<IEnumerable<Number>> GetRuns(IEnumerable<Number> sequence)
		{
			while (sequence.Any())
			{
				var first = sequence.First();
				var run = sequence.Take(2).TakeWhile(number => number.Value == first.Value);

				sequence = sequence.Skip(run.Count());
				yield return run;
			}
		}

		private void ProcessRuns(IEnumerable<Position> sequence, IEnumerable<IEnumerable<Number>> runs)
		{
			var movements = runs.Select(run => run.ToArray()).Zip(sequence, (run, position) => new {run, position});

			foreach (var movement in movements)
			{
				if (movement.run.Length == 1)
				{
					var origin = movement.run[0].Position;
					var number = _currentState[origin].Value;
					var target = movement.position;

					if (!origin.Equals(target))
					{
						_currentState = _currentState.Move(origin, movement.position);
						NumberMoved?.Invoke(new NumberMovedEvent(number, origin, target));
					}
				}
				else if (movement.run.Length == 2)
				{
					var origin1 = movement.run[0].Position;
					var origin2 = movement.run[1].Position;
					var target = movement.position;
					var number = _currentState[origin1].Value;

					_currentState = _currentState.Merge(origin1, origin2, target);
					NumbersMerged?.Invoke(new NumbersMergedEvent(number, origin1, origin2, target));
				}
			}
		}

		private static IEnumerable<int> Columns => Enumerable.Range(0, Width);

		private static IEnumerable<int> Rows => Enumerable.Range(0, Height);
	}
}
