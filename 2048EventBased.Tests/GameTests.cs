using FakeItEasy;
using System;
using System.Collections.Generic;
using Xunit;

namespace _2048EventBased.Tests
{
	public class FakeNumberChooser : IChooseNewNumber
	{
		private readonly Position _position;
		private readonly int _value;

		public FakeNumberChooser(Position position, int value)
		{
			_position = position;
			_value = value;
		}

		public Position ChoosePosition(IEnumerable<Position> emptyPositions) => _position;

		public int ChooseValue() => _value;
	}

	public class GameTests
	{
		[Fact]
		public void WhenGameEmpty_Add2At00_NumberAddedInvokedWith2At00()
		{
			var sut = new Game(new FakeNumberChooser(new Position(0, 0), 2));

			var numberAddedListener = A.Fake<Action<NumberAddedEvent>>();
			sut.NumberAdded += numberAddedListener;

			sut[0, 0] = 2;

			A.CallTo(() => numberAddedListener.Invoke(new NumberAddedEvent(2, 0, 0))).MustHaveHappened();
		}
	}
}
