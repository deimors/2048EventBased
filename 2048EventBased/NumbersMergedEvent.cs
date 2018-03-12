using System;

namespace _2048EventBased
{
	public struct NumbersMergedEvent : IEquatable<NumbersMergedEvent>
	{
		public int Number { get; }

		public Position Origin1 { get; }
		public Position Origin2 { get; }
		public Position Target { get; }

		public NumbersMergedEvent(int number, int origin1row, int origin1column, int origin2row, int origin2column, int targetRow, int targetColumn)
			: this(number, new Position(origin1row, origin1column), new Position(origin2row, origin2column), new Position(targetRow, targetColumn))
		{ }

		public NumbersMergedEvent(int number, Position origin1, Position origin2, Position target)
		{
			Number = number;
			Origin1 = origin1;
			Origin2 = origin2;
			Target = target;
		}

		public bool Equals(NumbersMergedEvent other) 
			=> Number == other.Number 
			   && Origin1.Equals(other.Origin1) 
			   && Origin2.Equals(other.Origin2) 
			   && Target.Equals(other.Target);

		public override bool Equals(object obj) 
			=> !(obj is null) 
			   && obj is NumbersMergedEvent @event 
			   && Equals(@event);

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = Number;
				hashCode = (hashCode * 397) ^ Origin1.GetHashCode();
				hashCode = (hashCode * 397) ^ Origin2.GetHashCode();
				hashCode = (hashCode * 397) ^ Target.GetHashCode();
				return hashCode;
			}
		}
	}
}