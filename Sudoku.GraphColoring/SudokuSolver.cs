using System;
using System.Xml.Linq;
using Sudoku.Shared;

namespace Sudoku.GraphColoring
{
	public class SudokuSolver
	{
        //var _grid = newSudoku.Cells;;
        private Node[,] _nodes;
        public SudokuSolver(SudokuGrid grid)
		{
            var _grid = grid.Cells;
            _nodes = new Node[9, 9];

            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    _nodes[row, col] = new Node(row, col, _grid[row][col]);
                }
            }
        }

        public bool Solve()
        {
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (_nodes[row, col].Value == 0)
                    {
                        for (int value = 1; value <= 9; value++)
                        {
                            if (IsValid(row, col, value))
                            {
                                _nodes[row, col].Value = value;

                                if (Solve())
                                    return true;

                                _nodes[row, col].Value = 0;
                            }
                        }

                        return false;
                    }
                }
            }

            return true;
        }

        private bool IsValid(int row, int col, int value)
        {
            // Check row
            for (int i = 0; i < 9; i++)
            {
                if (_nodes[row, i].Value == value)
                    return false;
            }

            // Check column
            for (int i = 0; i < 9; i++)
            {
                if (_nodes[i, col].Value == value)
                    return false;
            }

            // Check 3x3 subgrid
            int subGridRow = row - row % 3;
            int subGridCol = col - col % 3;

            for (int i = subGridRow; i < subGridRow + 3; i++)
            {
                for (int j = subGridCol; j < subGridCol + 3; j++)
                {
                    if (_nodes[i, j].Value == value)
                        return false;
                }
            }

            return true;
        }
    }
}

