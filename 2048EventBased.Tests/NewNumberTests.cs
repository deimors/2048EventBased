using System;
using FakeItEasy;
using Xunit;

namespace _2048EventBased.Tests
{
	public class NewNumberTests
	{
		public class When2At00
		{
			private readonly Game sut = new Game(new FakeNumberChooser(new Position(0, 0), 2)) {[0, 0] = 2};

			private readonly Action<NumberAddedEvent> numberAddedListener = A.Fake<Action<NumberAddedEvent>>();

			public When2At00()
			{
				sut.NumberAdded += numberAddedListener;
			}

			[Fact]
			public void MoveRight_NumberMovedInvokedWith2MovedFrom00To03()
			{
				sut.Move(Direction.Right);

				A.CallTo(() => numberAddedListener.Invoke(new NumberAddedEvent(2, 0, 0))).MustHaveHappened();
			}

			[Fact]
			public void MoveDown_NumberMovedInvokedWith2MovedFrom00To30()
			{
				sut.Move(Direction.Down);

				A.CallTo(() => numberAddedListener.Invoke(new NumberAddedEvent(2, 0, 0))).MustHaveHappened();
			}

			[Fact]
			public void MoveLeft_NumberMovedNotInvoked()
			{
				sut.Move(Direction.Left);

				A.CallTo(() => numberAddedListener.Invoke(A<NumberAddedEvent>._)).MustNotHaveHappened();
			}

			[Fact]
			public void MoveUp_NumberMovedNotInvoked()
			{
				sut.Move(Direction.Up);

				A.CallTo(() => numberAddedListener.Invoke(A<NumberAddedEvent>._)).MustNotHaveHappened();
			}
		}
	}
}