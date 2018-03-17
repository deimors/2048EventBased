using System;
using System.Collections.Generic;
using System.Linq;

namespace _2048EventBased.Tests
{
	public class FakeNumberChooser : IChooseNewNumber
	{
		private readonly Func<IEnumerable<Position>, Position> _positionSelector;
		private readonly int _value;

		public FakeNumberChooser() : this(2) { }

		public FakeNumberChooser(int value) : this(positions => positions.First(), value) { }

		public FakeNumberChooser(Position position, int value) : this (_ => position, value) { }

		public FakeNumberChooser(Func<IEnumerable<Position>, Position> positionSelector, int value)
		{
			_positionSelector = positionSelector ?? throw new ArgumentNullException(nameof(positionSelector));
			_value = value;
		}

		public Position ChoosePosition(IEnumerable<Position> emptyPositions) => _positionSelector(emptyPositions);

		public int ChooseValue() => _value;
	}
}