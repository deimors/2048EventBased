using System;
using System.Collections.Generic;
using System.Linq;
using Functional.Maybe;

namespace _2048EventBased
{
	internal static class ChangeEvaluation
	{
		private static IEnumerable<int> Columns(this Board board) => Enumerable.Range(0, board.Size);
		private static IEnumerable<int> Rows(this Board board) => Enumerable.Range(0, board.Size);

		public static IEnumerable<BoardChange> GetChangesForDirection(this Board board, Direction direction) 
			=> board.GetChangesForSequences(board.GetSequencesForDirection(direction));

		private static IEnumerable<IEnumerable<Position>> GetSequencesForDirection(this Board board, Direction direction)
		{
			switch (direction)
			{
				case Direction.Right:
					return board.Rows().Select(row => board.Columns().Reverse().Select(column => new Position(row, column)));

				case Direction.Down:
					return board.Columns().Select(column => board.Rows().Reverse().Select(row => new Position(row, column)));

				case Direction.Left:
					return board.Rows().Select(row => board.Columns().Select(column => new Position(row, column)));

				case Direction.Up:
					return board.Columns().Select(column => board.Rows().Select(row => new Position(row, column)));

				default:
					throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
			}
		}

		private static IEnumerable<BoardChange> GetChangesForSequences(this Board board, IEnumerable<IEnumerable<Position>> sequences) 
			=> sequences
				.Select(sequence => sequence.ToArray())
				.SelectMany(board.GetChangesForSequence);

		private static IEnumerable<BoardChange> GetChangesForSequence(this Board board, Position[] sequence) 
			=> sequence.GetChanges(board.CollapseSequence(sequence).GetRunsOfMax2());

		private static IEnumerable<Number> CollapseSequence(this Board board, IEnumerable<Position> sequence)
			=> sequence
				.Select(pos => board[pos].Select(value => new Number(pos, value)))
				.WhereValueExist();

		private static IEnumerable<BoardChange> GetChanges(this IEnumerable<Position> sequence, IEnumerable<IEnumerable<Number>> runs)
			=> runs
				.Select(run => run.ToArray())
				.Zip(sequence, (run, position) => new BoardChange(run, position))
				.Where(change => IsMovement(change));

		private static bool IsMovement(BoardChange change)
			=> !(change.Origins.Length == 1 && change.Origins[0].Position.Equals(change.Target));
		
		public static IEnumerable<IEnumerable<Number>> GetRunsOfMax2(this IEnumerable<Number> sequence)
			=> sequence.ToArray().GetRunsOfMaxLength((first, number) => number.Value == first.Value, 2);
	}
}