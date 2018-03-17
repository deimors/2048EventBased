using System.Collections.Generic;

namespace _2048EventBased
{
	public interface IChooseNewNumber
	{
		Position ChoosePosition(IEnumerable<Position> emptyPositions);
		int ChooseValue();
	}
}