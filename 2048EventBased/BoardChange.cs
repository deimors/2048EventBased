namespace _2048EventBased
{
	internal class BoardChange
	{
		public Number[] Origins { get; }
		public Position Target { get; }

		public BoardChange(Number[] origins, Position target)
		{
			Origins = origins;
			Target = target;
		}
	}
}