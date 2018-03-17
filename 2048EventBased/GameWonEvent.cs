using System;

namespace _2048EventBased
{
	public struct GameWonEvent : IEquatable<GameWonEvent>
	{
		public bool Equals(GameWonEvent other)
			=> true;

		public override bool Equals(object obj) 
			=> !(obj is null) 
			   && obj is GameWonEvent @event 
			   && Equals(@event);

		public override int GetHashCode()
			=> nameof(GameWonEvent).GetHashCode();
	}
}