using System;
using FakeItEasy;
using Xunit;

namespace _2048EventBased.Tests
{
	public class GameWonTests
	{
		public class When1024At00And01
		{
			private readonly Game sut = new Game(new FakeNumberChooser())
			{
				[0, 0] = 1024,
				[0, 1] = 1024
			};
			private readonly Action<GameWonEvent> gameWonListener = A.Fake<Action<GameWonEvent>>();

			public When1024At00And01()
			{
				sut.GameWon += gameWonListener;
			}

			[Fact]
			public void MoveRight_GameWon()
			{
				sut.Move(Direction.Right);

				A.CallTo(() => gameWonListener.Invoke(A<GameWonEvent>._)).MustHaveHappened();
			}

			[Fact]
			public void MoveDown_GameNotWon()
			{
				sut.Move(Direction.Down);

				A.CallTo(() => gameWonListener.Invoke(A<GameWonEvent>._)).MustNotHaveHappened();
			}

			[Fact]
			public void MoveLeft_GameWon()
			{
				sut.Move(Direction.Left);

				A.CallTo(() => gameWonListener.Invoke(A<GameWonEvent>._)).MustHaveHappened();
			}

			[Fact]
			public void MoveUp_GameNotWon()
			{
				sut.Move(Direction.Up);

				A.CallTo(() => gameWonListener.Invoke(A<GameWonEvent>._)).MustNotHaveHappened();
			}
		}
	}
}