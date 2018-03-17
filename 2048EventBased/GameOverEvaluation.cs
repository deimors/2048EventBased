using System.Linq;

namespace _2048EventBased
{
	internal static class GameOverEvaluation
	{
		public static bool IsGameWon(this Board board)
			=> board.HasValue(2048);

		public static bool IsGameLost(this Board board)
			=> !board.EmptyPositions.Any()
			   && board.AllPositions.All(board.AllNeighborsAreDifferent);

		private static bool AllNeighborsAreDifferent(this Board board, Position position)
			=> position.Neighbors()
				.Where(board.IsOnBoard)
				.All(neighbor => !board[position].Equals(board[neighbor]));
	}
}