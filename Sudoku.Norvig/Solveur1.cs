using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using Sudoku.Shared;

namespace Sudoku.Norvig
{
    class NorvigSudokuSolver : ISudokuSolver
    {
        private const int size = 9;
        private int[,] board;
        private List<List<int>> rows, cols, boxes;

        public NorvigSudokuSolver()
        {

        }

        public NorvigSudokuSolver(int[,] board)
        {
            this.board = board;
            rows = new List<List<int>>();
            cols = new List<List<int>>();
            boxes = new List<List<int>>();

            for (int i = 0; i < size; i++)
            {
                rows.Add(new List<int>());
                cols.Add(new List<int>());
                boxes.Add(new List<int>());
            }
        }

        private int GetBoxIndex(int row, int col)
        {
            int boxRow = row / 3;
            int boxCol = col / 3;
            return boxRow * 3 + boxCol;
        }

        private bool Solve()
        {
            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    if (board[row, col] == 0)
                    {
                        int boxIndex = GetBoxIndex(row, col);
                        List<int> possibilities = Enumerable.Range(1, 9).Except(rows[row]).Except(cols[col]).Except(boxes[boxIndex]).ToList();

                        if (possibilities.Count == 0)
                        {
                            return false;
                        }
                        else if (possibilities.Count == 1)
                        {
                            board[row, col] = possibilities[0];
                            rows[row].Add(possibilities[0]);
                            cols[col].Add(possibilities[0]);
                            boxes[boxIndex].Add(possibilities[0]);
                        }
                    }
                }
            }
            return true;
        }

        public int[,] SolveSudoku()
        {
            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    if (board[row, col] != 0)
                    {
                        int boxIndex = GetBoxIndex(row, col);
                        rows[row].Add(board[row, col]);
                        cols[col].Add(board[row, col]);
                        boxes[boxIndex].Add(board[row, col]);
                    }
                }
            }

            while (!Solve()) { }
            return board;
        }

        public SudokuGrid Solve(SudokuGrid s)
        {
            return s.CloneSudoku();
        }
    }
}
