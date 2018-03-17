using FakeItEasy;
using System;
using Xunit;

namespace _2048EventBased.Tests
{
	public class NewNumberTests
	{
		public class When2At00AndNumberChooserReturns2At33
		{
			private readonly Game sut = new Game(new FakeNumberChooser(new Position(3, 3), 2))
			{
				[0, 0] = 2
			};

			private readonly Action<NumberAddedEvent> numberAddedListener = A.Fake<Action<NumberAddedEvent>>();

			public When2At00AndNumberChooserReturns2At33()
			{
				sut.NumberAdded += numberAddedListener;
			}

			[Fact]
			public void MoveRight_2AddedAt33()
			{
				sut.Move(Direction.Right);

				A.CallTo(() => numberAddedListener.Invoke(new NumberAddedEvent(2, 3, 3))).MustHaveHappened();
			}

			[Fact]
			public void MoveDown_2AddedAt33()
			{
				sut.Move(Direction.Down);

				A.CallTo(() => numberAddedListener.Invoke(new NumberAddedEvent(2, 3, 3))).MustHaveHappened();
			}

			[Fact]
			public void MoveLeft_NoNumberAdded()
			{
				sut.Move(Direction.Left);

				A.CallTo(() => numberAddedListener.Invoke(A<NumberAddedEvent>._)).MustNotHaveHappened();
			}

			[Fact]
			public void MoveUp_NoNumberAdded()
			{
				sut.Move(Direction.Up);

				A.CallTo(() => numberAddedListener.Invoke(A<NumberAddedEvent>._)).MustNotHaveHappened();
			}
		}

		public class When2At33AndNumberChooserReturns4At00
		{
			private readonly Game sut = new Game(new FakeNumberChooser(new Position(0, 0), 4))
			{
				[3, 3] = 2
			};

			private readonly Action<NumberAddedEvent> numberAddedListener = A.Fake<Action<NumberAddedEvent>>();

			public When2At33AndNumberChooserReturns4At00()
			{
				sut.NumberAdded += numberAddedListener;
			}

			[Fact]
			public void MoveRight_NoNumberAdded()
			{
				sut.Move(Direction.Right);

				A.CallTo(() => numberAddedListener.Invoke(A<NumberAddedEvent>._)).MustNotHaveHappened();
			}

			[Fact]
			public void MoveDown_NoNumberAdded()
			{
				sut.Move(Direction.Down);

				A.CallTo(() => numberAddedListener.Invoke(A<NumberAddedEvent>._)).MustNotHaveHappened();
			}

			[Fact]
			public void MoveLeft_4AddedAt00()
			{
				sut.Move(Direction.Left);

				
				A.CallTo(() => numberAddedListener.Invoke(new NumberAddedEvent(4, 0, 0))).MustHaveHappened();
			}

			[Fact]
			public void MoveUp_4AddedAt00()
			{
				sut.Move(Direction.Up);

				A.CallTo(() => numberAddedListener.Invoke(new NumberAddedEvent(4, 0, 0))).MustHaveHappened();
			}
		}
	}
}