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
	}

	public class MovementTests
	{
		public class When2At00
		{
			private readonly Game sut = new Game { [0, 0] = 2 };
			private readonly Action<NumberMovedEvent> numberMovedListener = A.Fake<Action<NumberMovedEvent>>();

			public When2At00()
			{
				sut.NumberMoved += numberMovedListener;
			}

			[Fact]
			public void MoveRight_NumberMovedInvokedWith2MovedFrom00To03()
			{
				sut.Move(Direction.Right);

				A.CallTo(() => numberMovedListener.Invoke(new NumberMovedEvent(2, 0, 0, 0, 3))).MustHaveHappened();
			}

			[Fact]
			public void MoveDown_NumberMovedInvokedWith2MovedFrom00To30()
			{
				sut.Move(Direction.Down);

				A.CallTo(() => numberMovedListener.Invoke(new NumberMovedEvent(2, 0, 0, 3, 0))).MustHaveHappened();
			}

			[Fact]
			public void MoveLeft_NumberMovedNotInvoked()
			{
				sut.Move(Direction.Left);

				A.CallTo(() => numberMovedListener.Invoke(A<NumberMovedEvent>._)).MustNotHaveHappened();
			}

			[Fact]
			public void MoveUp_NumberMovedNotInvoked()
			{
				sut.Move(Direction.Up);

				A.CallTo(() => numberMovedListener.Invoke(A<NumberMovedEvent>._)).MustNotHaveHappened();
			}
		}


		public class When2At33
		{
			private readonly Game sut = new Game { [3, 3] = 2 };
			private readonly Action<NumberMovedEvent> numberMovedListener = A.Fake<Action<NumberMovedEvent>>();

			public When2At33()
			{
				sut.NumberMoved += numberMovedListener;
			}

			[Fact]
			public void MoveUp_NumberMovedInvokedWith2MovedFrom00To03()
			{
				sut.Move(Direction.Up);

				A.CallTo(() => numberMovedListener.Invoke(new NumberMovedEvent(2, 0, 0, 0, 3))).MustHaveHappened();
			}

			[Fact]
			public void MoveLeft_NumberMovedInvokedWith2MovedFrom00To30()
			{
				sut.Move(Direction.Left);

				A.CallTo(() => numberMovedListener.Invoke(new NumberMovedEvent(2, 0, 0, 3, 0))).MustHaveHappened();
			}

			[Fact]
			public void MoveDown_NumberMovedNotInvoked()
			{
				sut.Move(Direction.Down);

				A.CallTo(() => numberMovedListener.Invoke(A<NumberMovedEvent>._)).MustNotHaveHappened();
			}

			[Fact]
			public void MoveRight_NumberMovedNotInvoked()
			{
				sut.Move(Direction.Right);

				A.CallTo(() => numberMovedListener.Invoke(A<NumberMovedEvent>._)).MustNotHaveHappened();
			}
		}
	}
}
