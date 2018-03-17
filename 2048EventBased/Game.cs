using Functional.Maybe;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _2048EventBased
{
	public interface IChooseNewNumber
	{
		Position ChoosePosition(IEnumerable<Position> emptyPositions);
		int ChooseValue();
	}

	public class Game
	{
		private const int Width = 4;
		private const int Height = 4;

		private readonly IChooseNewNumber numberChooser;

		private Board _currentState = Board.Empty;

		public Game(IChooseNewNumber numberChooser)
		{
			this.numberChooser = numberChooser;
		}

		public event Action<NumberAddedEvent> NumberAdded;
		public event Action<NumberMovedEvent> NumberMoved;
		public event Action<NumbersMergedEvent> NumbersMerged;

		public int this[int row, int column]
		{
			set => AddNumber(row, column, value);
		}
		
		public void Move(Direction direction)
		{
			var sequences = GetSequencesForDirection(direction);

			foreach (var sequence in sequences.Select(sequence => sequence.ToArray()))
			{
				var collapsed = CollapseSequence(sequence);

				var runs = GetRuns(collapsed);

				var changes = GetChanges(sequence, runs);

				ApplyChanges(changes);
			}
		}

		private IEnumerable<Number> CollapseSequence(IEnumerable<Position> sequence) 
			=> sequence
				.Select(pos => _currentState[pos].Select(value => new Number(pos, value)))
				.WhereValueExist();

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
			=> sequence.ToArray().GetRunsOfAtMost((first, number) => number.Value == first.Value, 2);
		
		private void ApplyChanges(IEnumerable<BoardChange> changes)
		{
			foreach (var change in changes)
			{
				ApplyChange(change);
			}
		}

		private void ApplyChange(BoardChange change)
		{
			switch (change.Origins.Length)
			{
				case 1 when !change.Origins[0].Position.Equals(change.Target):
					MoveNumber(
						change.Origins[0].Position,
						change.Target,
						_currentState[change.Origins[0].Position].Value
					);
					break;

				case 2:
					MergeNumbers(
						change.Origins[0].Position,
						change.Origins[1].Position,
						change.Target,
						_currentState[change.Origins[0].Position].Value
					);
					break;
			}
		}

		private void AddNumber(int row, int column, int value)
		{
			_currentState = _currentState.Add(new Position(row, column), value);
			NumberAdded?.Invoke(new NumberAddedEvent(value, row, column));
		}

		private void MoveNumber(Position origin, Position target, int number)
		{
			_currentState = _currentState.Move(origin, target);
			NumberMoved?.Invoke(new NumberMovedEvent(number, origin, target));
		}

		private void MergeNumbers(Position origin1, Position origin2, Position target, int number)
		{
			_currentState = _currentState.Merge(origin1, origin2, target);
			NumbersMerged?.Invoke(new NumbersMergedEvent(number, origin1, origin2, target));
		}

		private static IEnumerable<BoardChange> GetChanges(IEnumerable<Position> sequence, IEnumerable<IEnumerable<Number>> runs) 
			=> runs
				.Select(run => run.ToArray())
				.Zip(sequence, (run, position) => new BoardChange(run, position));

		private static IEnumerable<int> Columns => Enumerable.Range(0, Width);

		private static IEnumerable<int> Rows => Enumerable.Range(0, Height);
	}
}
