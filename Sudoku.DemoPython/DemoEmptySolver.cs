using Sudoku.Shared;

namespace Sudoku.DemoPython
{
	public class DemoEmptySolver : ISudokuSolver
	{
		public SudokuGrid Solve(SudokuGrid s)
		{
			return s.CloneSudoku();
		}

	}
}