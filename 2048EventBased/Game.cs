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

		private readonly IChooseNewNumber _numberChooser;

		private Board _currentState;

		public Game(IChooseNewNumber numberChooser) : this(4, numberChooser) { }

		public Game(int size, IChooseNewNumber numberChooser)
		{
			_currentState = Board.Empty(size);
			_numberChooser = numberChooser;
		}

		public event Action<NumberAddedEvent> NumberAdded;
		public event Action<NumberMovedEvent> NumberMoved;
		public event Action<NumbersMergedEvent> NumbersMerged;
		public event Action<GameWonEvent> GameWon;

		public int this[int row, int column]
		{
			set => AddNumber(new Position(row, column), value);
		}
		
		public void Move(Direction direction)
		{
			var changes = GetChangesForDirection(direction).ToArray();
			
			if (changes.Any())
			{
				ApplyChanges(changes);

				AddNewNumber();
			}
		}

		private IEnumerable<BoardChange> GetChangesForDirection(Direction direction) 
			=> GetChangesForSequences(GetSequencesForDirection(direction));

		private IEnumerable<BoardChange> GetChangesForSequences(IEnumerable<IEnumerable<Position>> sequences) 
			=> sequences
				.Select(sequence => sequence.ToArray())
				.SelectMany(GetChangesForSequence);

		private IEnumerable<BoardChange> GetChangesForSequence(Position[] sequence) 
			=> GetChanges(sequence, GetRuns(CollapseSequence(sequence)));

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
				case 1:
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

		private void AddNewNumber() 
			=> AddNumber(
				_numberChooser.ChoosePosition(_currentState.EmptyPositions), 
				_numberChooser.ChooseValue()
			);

		private void AddNumber(Position position, int value)
		{
			_currentState = _currentState.Add(position, value);
			NumberAdded?.Invoke(new NumberAddedEvent(value, position));
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
				.Zip(sequence, (run, position) => new BoardChange(run, position))
				.Where(change => IsMovement(change));

		private static bool IsMovement(BoardChange change)
			=> !(change.Origins.Length == 1 && change.Origins[0].Position.Equals(change.Target));

		private static IEnumerable<int> Columns => Enumerable.Range(0, Width);

		private static IEnumerable<int> Rows => Enumerable.Range(0, Height);
	}
}
