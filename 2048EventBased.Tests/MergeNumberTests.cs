using System;
using FakeItEasy;
using Xunit;

namespace _2048EventBased.Tests
{
	public class MergeNumberTests
	{
		public class When2At01AndAt02
		{
			private readonly Game sut = new Game
			{
				[0, 1] = 2,
				[0, 2] = 2
			};

			private readonly Action<NumbersMergedEvent> numbersMergedListener = A.Fake<Action<NumbersMergedEvent>>();
			private readonly Action<NumberMovedEvent> numberMovedListener = A.Fake<Action<NumberMovedEvent>>();

			public When2At01AndAt02()
			{
				sut.NumbersMerged += numbersMergedListener;
				sut.NumberMoved += numberMovedListener;
			}

			[Fact]
			public void MoveRight_NumbersMergedInvokedWith2At01And02MergedAt03()
			{
				sut.Move(Direction.Right);

				A.CallTo(() => numbersMergedListener.Invoke(new NumbersMergedEvent(2, 0, 2, 0, 1, 0, 3))).MustHaveHappened();
				A.CallTo(() => numberMovedListener.Invoke(A<NumberMovedEvent>._)).MustNotHaveHappened();
			}

			[Fact]
			public void MoveLeft_NumbersMergedInvokedWith2At01And02MergedAt00()
			{
				sut.Move(Direction.Left);

				A.CallTo(() => numbersMergedListener.Invoke(new NumbersMergedEvent(2, 0, 1, 0, 2, 0, 0))).MustHaveHappened();
				A.CallTo(() => numberMovedListener.Invoke(A<NumberMovedEvent>._)).MustNotHaveHappened();
			}
		}

		public class When2At10AndAt20
		{
			private readonly Game sut = new Game
			{
				[1, 0] = 2,
				[2, 0] = 2
			};

			private readonly Action<NumbersMergedEvent> numbersMergedListener = A.Fake<Action<NumbersMergedEvent>>();
			private readonly Action<NumberMovedEvent> numberMovedListener = A.Fake<Action<NumberMovedEvent>>();

			public When2At10AndAt20()
			{
				sut.NumbersMerged += numbersMergedListener;
				sut.NumberMoved += numberMovedListener;
			}

			[Fact]
			public void MoveUp_NumbersMergedInvokedWith2At10And20MergedAt00()
			{
				sut.Move(Direction.Up);

				A.CallTo(() => numbersMergedListener.Invoke(new NumbersMergedEvent(2, 1, 0, 2, 0, 0, 0))).MustHaveHappened();
				A.CallTo(() => numberMovedListener.Invoke(A<NumberMovedEvent>._)).MustNotHaveHappened();
			}

			[Fact]
			public void MoveDown_NumbersMergedInvokedWith2At10And20MergedAt30()
			{
				sut.Move(Direction.Down);

				A.CallTo(() => numbersMergedListener.Invoke(new NumbersMergedEvent(2, 2, 0, 1, 0, 3, 0))).MustHaveHappened();
				A.CallTo(() => numberMovedListener.Invoke(A<NumberMovedEvent>._)).MustNotHaveHappened();
			}
		}

		public class When2At01And02And4At03
		{
			private readonly Game sut = new Game
			{
				[0, 1] = 2,
				[0, 2] = 2,
				[0, 3] = 4
			};

			private readonly Action<NumbersMergedEvent> numbersMergedListener = A.Fake<Action<NumbersMergedEvent>>();
			private readonly Action<NumberMovedEvent> numberMovedListener = A.Fake<Action<NumberMovedEvent>>();

			public When2At01And02And4At03()
			{
				sut.NumbersMerged += numbersMergedListener;
				sut.NumberMoved += numberMovedListener;
			}

			[Fact]
			public void MoveRight_NumbersMergedInvokedWith2At01And02MergedAt03()
			{
				sut.Move(Direction.Right);

				A.CallTo(() => numbersMergedListener.Invoke(new NumbersMergedEvent(2, 0, 2, 0, 1, 0, 2))).MustHaveHappened();
				A.CallTo(() => numberMovedListener.Invoke(A<NumberMovedEvent>._)).MustNotHaveHappened();
			}

			[Fact]
			public void MoveLeft_NumbersMergedInvokedWith2At01And02MergedAt00()
			{
				sut.Move(Direction.Left);

				A.CallTo(() => numbersMergedListener.Invoke(new NumbersMergedEvent(2, 0, 1, 0, 2, 0, 0))).MustHaveHappened();
				A.CallTo(() => numberMovedListener.Invoke(new NumberMovedEvent(4, 0, 3, 0, 1))).MustHaveHappened();
			}
		}

		public class When2At00And01And4At02And03
		{
			private readonly Game sut = new Game
			{
				[0, 0] = 2,
				[0, 1] = 2,
				[0, 2] = 4,
				[0, 3] = 4
			};

			private readonly Action<NumbersMergedEvent> numbersMergedListener = A.Fake<Action<NumbersMergedEvent>>();
			private readonly Action<NumberMovedEvent> numberMovedListener = A.Fake<Action<NumberMovedEvent>>();

			public When2At00And01And4At02And03()
			{
				sut.NumbersMerged += numbersMergedListener;
				sut.NumberMoved += numberMovedListener;
			}

			[Fact]
			public void MoveRight_NumbersMergedInvokedWith2At01And02MergedAt03()
			{
				sut.Move(Direction.Right);

				A.CallTo(() => numbersMergedListener.Invoke(new NumbersMergedEvent(2, 0, 1, 0, 0, 0, 2))).MustHaveHappened();
				A.CallTo(() => numbersMergedListener.Invoke(new NumbersMergedEvent(4, 0, 3, 0, 2, 0, 3))).MustHaveHappened();
				A.CallTo(() => numberMovedListener.Invoke(A<NumberMovedEvent>._)).MustNotHaveHappened();
			}

			[Fact]
			public void MoveLeft_NumbersMergedInvokedWith2At01And02MergedAt00()
			{
				sut.Move(Direction.Left);

				A.CallTo(() => numbersMergedListener.Invoke(new NumbersMergedEvent(2, 0, 0, 0, 1, 0, 0))).MustHaveHappened();
				A.CallTo(() => numbersMergedListener.Invoke(new NumbersMergedEvent(4, 0, 2, 0, 3, 0, 1))).MustHaveHappened();
				A.CallTo(() => numberMovedListener.Invoke(A<NumberMovedEvent>._)).MustNotHaveHappened();
			}
		}

		public class When2InLineFrom00To03
		{
			private readonly Game sut = new Game
			{
				[0, 0] = 2,
				[0, 1] = 2,
				[0, 2] = 2,
				[0, 3] = 2
			};

			private readonly Action<NumbersMergedEvent> numbersMergedListener = A.Fake<Action<NumbersMergedEvent>>();
			private readonly Action<NumberMovedEvent> numberMovedListener = A.Fake<Action<NumberMovedEvent>>();

			public When2InLineFrom00To03()
			{
				sut.NumbersMerged += numbersMergedListener;
				sut.NumberMoved += numberMovedListener;
			}

			[Fact]
			public void MoveRight_NumbersMergedInvokedWith2At01And02MergedAt03()
			{
				sut.Move(Direction.Right);

				A.CallTo(() => numbersMergedListener.Invoke(new NumbersMergedEvent(2, 0, 1, 0, 0, 0, 2))).MustHaveHappened();
				A.CallTo(() => numbersMergedListener.Invoke(new NumbersMergedEvent(2, 0, 3, 0, 2, 0, 3))).MustHaveHappened();
				A.CallTo(() => numberMovedListener.Invoke(A<NumberMovedEvent>._)).MustNotHaveHappened();
			}

			[Fact]
			public void MoveLeft_NumbersMergedInvokedWith2At01And02MergedAt00()
			{
				sut.Move(Direction.Left);

				A.CallTo(() => numbersMergedListener.Invoke(new NumbersMergedEvent(2, 0, 0, 0, 1, 0, 0))).MustHaveHappened();
				A.CallTo(() => numbersMergedListener.Invoke(new NumbersMergedEvent(2, 0, 2, 0, 3, 0, 1))).MustHaveHappened();
				A.CallTo(() => numberMovedListener.Invoke(A<NumberMovedEvent>._)).MustNotHaveHappened();
			}
		}
	}
}