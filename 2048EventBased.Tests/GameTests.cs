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
			var sut = new Game(new FakeNumberChooser());

			var numberAddedListener = A.Fake<Action<NumberAddedEvent>>();
			sut.NumberAdded += numberAddedListener;

			sut[0, 0] = 2;

			A.CallTo(() => numberAddedListener.Invoke(new NumberAddedEvent(2, 0, 0))).MustHaveHappened();
		}
	}
}
