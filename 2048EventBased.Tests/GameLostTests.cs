using System;
using FakeItEasy;
using Xunit;

namespace _2048EventBased.Tests
{
	public class GameLostTests
	{
		public class WhenOneMoveAwayFromLoss
		{
			private readonly Game sut = new Game(new FakeNumberChooser(4))
			{
				[0, 0] = 2, [0, 1] = 4, [0, 2] = 2, [0, 3] = 4,
				[1, 0] = 4, [1, 1] = 2, [1, 2] = 4, [1, 3] = 2,
				[2, 0] = 2, [2, 1] = 4, [2, 2] = 2, [2, 3] = 4,
				[3, 0] = 2, [3, 1] = 4, [3, 2] = 2
			};

			private readonly Action gameLostListener = A.Fake<Action>();

			public WhenOneMoveAwayFromLoss()
			{
				sut.GameLost += gameLostListener;
			}

			[Fact]
			public void MoveRight_GameLost()
			{
				sut.Move(Direction.Right);

				A.CallTo(() => gameLostListener.Invoke()).MustHaveHappened();
			}

			[Fact]
			public void MoveDown_GameNotLost()
			{
				sut.Move(Direction.Down);

				A.CallTo(() => gameLostListener.Invoke()).MustNotHaveHappened();
			}

			[Fact]
			public void MoveLeft_GameNotLost()
			{
				sut.Move(Direction.Left);

				A.CallTo(() => gameLostListener.Invoke()).MustNotHaveHappened();
			}

			[Fact]
			public void MoveUp_GameNotLost()
			{
				sut.Move(Direction.Up);

				A.CallTo(() => gameLostListener.Invoke()).MustNotHaveHappened();
			}
		}
	}
}