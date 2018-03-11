using System;

namespace _2048EventBased
{
	public class NumberMovedEvent : IEquatable<NumberMovedEvent>
	{
		public int Number { get; }
		public int OriginRow { get; }
		public int OriginColumn { get; }
		public int TargetRow { get; }
		public int TargetColumn { get; }

		public NumberMovedEvent(int number, int originRow, int originColumn, int targetRow, int targetColumn)
		{
			Number = number;
			OriginRow = originRow;
			OriginColumn = originColumn;
			TargetRow = targetRow;
			TargetColumn = targetColumn;
		}

		public bool Equals(NumberMovedEvent other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return Number == other.Number && OriginRow == other.OriginRow && OriginColumn == other.OriginColumn && TargetRow == other.TargetRow && TargetColumn == other.TargetColumn;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((NumberMovedEvent) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = Number;
				hashCode = (hashCode * 397) ^ OriginRow;
				hashCode = (hashCode * 397) ^ OriginColumn;
				hashCode = (hashCode * 397) ^ TargetRow;
				hashCode = (hashCode * 397) ^ TargetColumn;
				return hashCode;
			}
		}
	}
}