using System;

namespace _2048EventBased
{
	public struct Position : IEquatable<Position>
	{
		public int Row { get; }
		public int Column { get; }

		public Position(int row, int column)
		{
			Row = row;
			Column = column;
		}

		public bool Equals(Position other) 
			=> Row == other.Row 
			   && Column == other.Column;

		public override bool Equals(object obj) 
			=> !(obj is null) 
			   && obj is Position position 
			   && Equals(position);

		public override int GetHashCode()
		{
			unchecked
			{
				return (Row * 397) ^ Column;
			}
		}

		public override string ToString()
			=> $"({Row}, {Column})";

		public static Position operator +(Position left, Position right)
			=> new Position(left.Row + right.Row, left.Column + right.Column);
	}
}