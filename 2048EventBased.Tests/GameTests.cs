using FakeItEasy;
using System;
using Xunit;

namespace _2048EventBased.Tests
{
	public class GameTests
	{
		[Fact]
		public void WhenGameEmpty_Add2At00_NumberAddedInvokedWith2At00()
		{
			var sut = new Game();
			var numberAddedListener = A.Fake<Action<NumberAddedEvent>>();
			sut.NumberAdded += numberAddedListener;

			sut[0, 0] = 2;

			A.CallTo(() => numberAddedListener.Invoke(new NumberAddedEvent(2, 0, 0))).MustHaveHappened();
		}

		[Fact]
		public void When2At00_MoveRight_NumberMovedInvokedWith2MovedFrom00To03()
		{
			var sut = new Game { [0, 0] = 2 };
			var numberMovedListener = A.Fake<Action<NumberMovedEvent>>();
			sut.NumberMoved += numberMovedListener;

			sut.Move(Direction.Right);

			A.CallTo(() => numberMovedListener.Invoke(new NumberMovedEvent(2, 0, 0, 0, 3))).MustHaveHappened();
		}

		[Fact]
		public void When2At00_MoveDown_NumberMovedInvokedWith2MovedFrom00To30()
		{
			var sut = new Game { [0, 0] = 2 };
			var numberMovedListener = A.Fake<Action<NumberMovedEvent>>();
			sut.NumberMoved += numberMovedListener;

			sut.Move(Direction.Down);

			A.CallTo(() => numberMovedListener.Invoke(new NumberMovedEvent(2, 0, 0, 3, 0))).MustHaveHappened();
		}

		[Fact]
		public void When2At00_MoveLeft_NumberMovedNotInvoked()
		{
			var sut = new Game { [0, 0] = 2 };
			var numberMovedListener = A.Fake<Action<NumberMovedEvent>>();
			sut.NumberMoved += numberMovedListener;

			sut.Move(Direction.Left);

			A.CallTo(() => numberMovedListener.Invoke(A<NumberMovedEvent>._)).MustNotHaveHappened();
		}

		[Fact]
		public void When2At00_MoveUp_NumberMovedToInvokedWith2MovedFrom00To03()
		{
			var sut = new Game { [0, 0] = 2 };
			var numberMovedListener = A.Fake<Action<NumberMovedEvent>>();
			sut.NumberMoved += numberMovedListener;

			sut.Move(Direction.Up);

			A.CallTo(() => numberMovedListener.Invoke(A<NumberMovedEvent>._)).MustNotHaveHappened();
		}
	}
}
