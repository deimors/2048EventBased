using System;

namespace _2048EventBased
{
	public class NumberAddedEvent : IEquatable<NumberAddedEvent>
	{
		public int Number { get; }
		public int Row { get; }
		public int Column { get; }

		public NumberAddedEvent(int number, int row, int column)
		{
			Number = number;
			Row = row;
			Column = column;
		}

		public bool Equals(NumberAddedEvent other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return Number == other.Number && Row == other.Row && Column == other.Column;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((NumberAddedEvent) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = Number;
				hashCode = (hashCode * 397) ^ Row;
				hashCode = (hashCode * 397) ^ Column;
				return hashCode;
			}
		}
	}
}