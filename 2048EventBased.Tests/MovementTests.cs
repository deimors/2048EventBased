using System;
using FakeItEasy;
using Xunit;

namespace _2048EventBased.Tests
{
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
			public void MoveUp_NumberMovedInvokedWith2MovedFrom33To03()
			{
				sut.Move(Direction.Up);

				A.CallTo(() => numberMovedListener.Invoke(new NumberMovedEvent(2, 3, 3, 0, 3))).MustHaveHappened();
			}

			[Fact]
			public void MoveLeft_NumberMovedInvokedWith2MovedFrom33To30()
			{
				sut.Move(Direction.Left);

				A.CallTo(() => numberMovedListener.Invoke(new NumberMovedEvent(2, 3, 3, 3, 0))).MustHaveHappened();
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


		public class When2At00And33And4At03And30
		{
			private readonly Game sut = new Game
			{
				[0, 0] = 2,
				[3, 3] = 2,
				[0, 3] = 4,
				[3, 0] = 4
			};

			private readonly Action<NumberMovedEvent> numberMovedListener = A.Fake<Action<NumberMovedEvent>>();

			public When2At00And33And4At03And30()
			{
				sut.NumberMoved += numberMovedListener;
			}

			[Fact]
			public void MoveUp_NumberMovedInvokedWith2MovedFrom33To13AndWith4MovedFrom30To10()
			{
				sut.Move(Direction.Up);

				A.CallTo(() => numberMovedListener.Invoke(new NumberMovedEvent(2, 3, 3, 1, 3))).MustHaveHappened();
				A.CallTo(() => numberMovedListener.Invoke(new NumberMovedEvent(4, 3, 0, 1, 0))).MustHaveHappened();
			}

			[Fact]
			public void MoveLeft_NumberMovedInvokedWith2MovedFrom33To31AndWith4MovedFrom03To01()
			{
				sut.Move(Direction.Left);

				A.CallTo(() => numberMovedListener.Invoke(new NumberMovedEvent(2, 3, 3, 3, 1))).MustHaveHappened();
				A.CallTo(() => numberMovedListener.Invoke(new NumberMovedEvent(4, 0, 3, 0, 1))).MustHaveHappened();
			}

			[Fact]
			public void MoveDown_NumberMovedInvokedWith2MovedFrom00To20AndWith4MovedFrom03To23()
			{
				sut.Move(Direction.Down);

				A.CallTo(() => numberMovedListener.Invoke(new NumberMovedEvent(2, 0, 0, 2, 0))).MustHaveHappened();
				A.CallTo(() => numberMovedListener.Invoke(new NumberMovedEvent(4, 0, 3, 2, 3))).MustHaveHappened();
			}

			[Fact]
			public void MoveRight_NumberMovedInvokedWith2MovedFrom00To02AndWith4MovedFrom30To32()
			{
				sut.Move(Direction.Right);

				A.CallTo(() => numberMovedListener.Invoke(new NumberMovedEvent(2, 0, 0, 0, 2))).MustHaveHappened();
				A.CallTo(() => numberMovedListener.Invoke(new NumberMovedEvent(4, 3, 0, 3, 2))).MustHaveHappened();
			}
		}

		public class When2At01And4At02
		{
			private readonly Game sut = new Game
			{
				[0, 1] = 2,
				[0, 2] = 4
			};

			private readonly Action<NumberMovedEvent> numberMovedListener = A.Fake<Action<NumberMovedEvent>>();

			public When2At01And4At02()
			{
				sut.NumberMoved += numberMovedListener;
			}

			[Fact]
			public void MoveRight_NumberMovedInvokedWith2MovedFrom01To02AndWith4MovedFrom02To03()
			{
				sut.Move(Direction.Right);

				A.CallTo(() => numberMovedListener.Invoke(new NumberMovedEvent(2, 0, 1, 0, 2))).MustHaveHappened();
				A.CallTo(() => numberMovedListener.Invoke(new NumberMovedEvent(4, 0, 2, 0, 3))).MustHaveHappened();
			}

			[Fact]
			public void MoveLeft_NumberMovedInvokedWith2MovedFrom01To00AndWith4MovedFrom02To01()
			{
				sut.Move(Direction.Left);

				A.CallTo(() => numberMovedListener.Invoke(new NumberMovedEvent(2, 0, 1, 0, 0))).MustHaveHappened();
				A.CallTo(() => numberMovedListener.Invoke(new NumberMovedEvent(4, 0, 2, 0, 1))).MustHaveHappened();
			}
		}

		public class When2At10And4At20
		{
			private readonly Game sut = new Game
			{
				[1, 0] = 2,
				[2, 0] = 4
			};

			private readonly Action<NumberMovedEvent> numberMovedListener = A.Fake<Action<NumberMovedEvent>>();

			public When2At10And4At20()
			{
				sut.NumberMoved += numberMovedListener;
			}

			[Fact]
			public void MoveUp_NumberMovedInvokedWith2MovedFrom10To00AndWith4MovedFrom20To10()
			{
				sut.Move(Direction.Up);

				A.CallTo(() => numberMovedListener.Invoke(new NumberMovedEvent(2, 1, 0, 0, 0))).MustHaveHappened();
				A.CallTo(() => numberMovedListener.Invoke(new NumberMovedEvent(4, 2, 0, 1, 0))).MustHaveHappened();
			}

			[Fact]
			public void MoveDown_NumberMovedInvokedWith2MovedFrom10To20AndWith4MovedFrom20To30()
			{
				sut.Move(Direction.Down);

				A.CallTo(() => numberMovedListener.Invoke(new NumberMovedEvent(2, 1, 0, 2, 0))).MustHaveHappened();
				A.CallTo(() => numberMovedListener.Invoke(new NumberMovedEvent(4, 2, 0, 3, 0))).MustHaveHappened();
			}
		}
	}
}