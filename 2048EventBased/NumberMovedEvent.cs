using System;

namespace _2048EventBased
{
	public struct NumberMovedEvent : IEquatable<NumberMovedEvent>
	{
		
		public int Number { get; }

		public Position Origin { get; }
		public Position Target { get; }

		public NumberMovedEvent(int number, Position origin, Position target)
		{
			Number = number;
			Origin = origin;
			Target = target;
		}

		public NumberMovedEvent(int number, int originRow, int originColumn, int targetRow, int targetColumn)
			: this(number, new Position(originRow, originColumn), new Position(targetRow, targetColumn))
		{ }

		public bool Equals(NumberMovedEvent other) 
			=> Number == other.Number 
			   && Origin.Equals(other.Origin) 
			   && Target.Equals(other.Target);

		public override bool Equals(object obj) 
			=> obj is NumberMovedEvent @event && Equals(@event);

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = Number;
				hashCode = (hashCode * 397) ^ Origin.GetHashCode();
				hashCode = (hashCode * 397) ^ Target.GetHashCode();
				return hashCode;
			}
		}

		public override string ToString()
			=> $"NumberMoved {Number} @ {Origin} -> {Target}";
	}
}