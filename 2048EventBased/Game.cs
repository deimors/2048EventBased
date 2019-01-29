using System;
using System.Collections.Generic;
using System.Linq;

namespace _2048EventBased
{
	public class Game
	{
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
		public event Action GameWon;
		public event Action GameLost;

		public int this[int row, int column]
		{
			set => AddNumber(new Position(row, column), value);
		}
		
		public void Move(Direction direction)
		{
			var changes = _currentState.GetChangesForDirection(direction).ToArray();
			
			if (changes.Any())
			{
				ApplyChanges(changes);

				AddNewNumber();

				EvaluateGameOver();
			}
		}

		private void EvaluateGameOver()
		{
			if (_currentState.IsGameWon())
				GameWon?.Invoke();

			if (_currentState.IsGameLost())
				GameLost?.Invoke();
		}
		
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
						_currentState[change.Origins[0].Position].Value + _currentState[change.Origins[1].Position].Value
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
	}
}
