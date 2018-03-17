namespace _2048EventBased
{
	public class Number
	{
		public Position Position { get; }
		public int Value { get; }

		public Number(Position position, int value)
		{
			Position = position;
			Value = value;
		}

		public override string ToString()
			=> $"{Value} @ {Position}";
	}
}