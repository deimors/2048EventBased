using System;

namespace _2048EventBased
{
	public struct NumberAddedEvent : IEquatable<NumberAddedEvent>
	{
		public int Number { get; }
		public Position Position { get; }

		public NumberAddedEvent(int number, Position position)
		{
			Number = number;
			Position = position;
		}

		public NumberAddedEvent(int number, int row, int column) : this(number, new Position(row, column))
		{
		}

		public bool Equals(NumberAddedEvent other) 
			=> Number == other.Number 
			   && Position.Equals(other.Position);

		public override bool Equals(object obj) 
			=> !(obj is null) 
			   && obj is NumberAddedEvent @event 
			   && Equals(@event);

		public override int GetHashCode()
		{
			unchecked
			{
				return (Number * 397) ^ Position.GetHashCode();
			}
		}
	}
}