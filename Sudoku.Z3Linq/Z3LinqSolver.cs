using Sudoku.Shared;

namespace Sudoku.Z3Linq;

public class Z3LinqSolver : ISudokuSolver
{
    public SudokuGrid Solve(SudokuGrid s)
    {
        return s.CloneSudoku();
    }

}