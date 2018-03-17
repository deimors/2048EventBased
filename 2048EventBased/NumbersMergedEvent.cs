using System;

namespace _2048EventBased
{
	public struct NumbersMergedEvent : INumberEvent, IEquatable<NumbersMergedEvent>
	{
		public int NewNumber { get; }

		public Position Origin1 { get; }
		public Position Origin2 { get; }
		public Position Target { get; }

		public NumbersMergedEvent(int newNumber, int origin1Row, int origin1Column, int origin2Row, int origin2Column, int targetRow, int targetColumn)
			: this(newNumber, new Position(origin1Row, origin1Column), new Position(origin2Row, origin2Column), new Position(targetRow, targetColumn))
		{ }

		public NumbersMergedEvent(int newNumber, Position origin1, Position origin2, Position target)
		{
			NewNumber = newNumber;
			Origin1 = origin1;
			Origin2 = origin2;
			Target = target;
		}

		public bool Equals(NumbersMergedEvent other) 
			=> NewNumber == other.NewNumber 
			   && ((Origin1.Equals(other.Origin1) && Origin2.Equals(other.Origin2)) || (Origin1.Equals(other.Origin2) && Origin2.Equals(Origin1)))
			   && Target.Equals(other.Target);

		public override bool Equals(object obj) 
			=> !(obj is null) 
			   && obj is NumbersMergedEvent @event 
			   && Equals(@event);

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = NewNumber;
				hashCode = (hashCode * 397) ^ Origin1.GetHashCode();
				hashCode = (hashCode * 397) ^ Origin2.GetHashCode();
				hashCode = (hashCode * 397) ^ Target.GetHashCode();
				return hashCode;
			}
		}

		public override string ToString() 
			=> $"Merge {Origin1} & {Origin2} -> {NewNumber} @ {Target}";

		public static bool operator ==(NumbersMergedEvent left, NumbersMergedEvent right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(NumbersMergedEvent left, NumbersMergedEvent right)
		{
			return !left.Equals(right);
		}
	}
}