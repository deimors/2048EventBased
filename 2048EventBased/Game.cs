using Functional.Maybe;
using System;

namespace _2048EventBased
{
	public enum Direction
	{
		Right,
		Down,
		Left,
		Up
	}

	public class Game
	{
		private readonly Maybe<int>[,] _cells = new Maybe<int>[4,4];

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
			switch (direction)
			{
				case Direction.Right:
					NumberMoved?.Invoke(new NumberMovedEvent(2, 0, 0, 0, 3));
				break;
				case Direction.Down:
					NumberMoved?.Invoke(new NumberMovedEvent(2, 0, 0, 3, 0));
				break;
				case Direction.Left:
					break;
				case Direction.Up:
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
			}
		}
	}
}
