using System;
using Sudoku.Shared;

namespace Sudoku.GraphColoring
{
	public class SolverColoring
	{
        private Node[,] _nodes;

        public SolverColoring()
		{
		}

        

        public bool SudokuSolver(SudokuGrid grid)
        {
            var cell = grid.Cells;
            _nodes = new Node[9, 9];

            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    _nodes[row, col] = new Node(row, col, cell[row][col]); //jai copi colle cette partie sur la class 1 car je voulais utiliser _nodes
                }
            }
            return true;
        }

        public bool Solve(Node[,] _nodes, SudokuGrid grid)
        {
            var cell = grid.Cells;
            while (!IsColoringComplete(grid)) //tant qu'il y a de zero
            {
                for (int row = 0; row < 9; row++)
                {
                    for (int col = 0; col < 9; col++)
                    {
                        if (cell[row][col] == 0)
                        {
                            bool[] possibleColors = new bool[10];
                            for (int i = 0; i < 9; i++)
                            {
                                if (cell[row][i] != 0)
                                {
                                    possibleColors[cell[row][i]] = true;

                                }
                                if (cell[i][col] != 0)
                                {
                                    possibleColors[cell[i][col]] = true;
                                }
                            }

                            int rowStart = (row / 3) * 3;
                            int colStart = (col / 3) * 3;
                            for (int i = 0; i < 3; i++)
                            {
                                for (int j = 0; j < 3; j++)
                                {
                                    if (cell[rowStart + i][colStart + j] != 0)
                                    {
                                        possibleColors[cell[rowStart + i][colStart + j]] = true;
                                    }
                                }
                            }

                            int color = 1;
                            for (; color <= 9; color++)
                            {
                                if (!possibleColors[color])
                                {
                                    cell[row][col] = color;
                                    _nodes[row, col].Value = color;
                                    _nodes[row, col].Color = color;

                                    if (Solve(_nodes, grid))
                                    {
                                        return true;
                                    }

                                    cell[row][col] = 0;
                                    _nodes[row, col].Value = 0;
                                    _nodes[row, col].Color = 0;
                                }
                            }
                            return false;

                        }
                    }
                }

            }
            return true;
        }

        private int GetNextAvailableColor(int row, int col, SudokuGrid grid)
        {
            for (int value = 1; value <= 9; value++)
            {
                if (IsValid(row, col, value, grid))
                    return value;
            }

            return 0;
        }

        private bool IsValid(int row, int col, int value, SudokuGrid grid)
        {
            var cell = grid.Cells;
            // Check row
            for (int i = 0; i < 9; i++)
            {
                if (cell[row][i] == value)// on verifie si value exitste deja sur toute la ligne
                    return false;
            }

            // Check column
            for (int i = 0; i < 9; i++)
            {
                if (cell[i][col] == value)// on verifie si value existe deja sur toute la colonne
                    return false;
            }

            // Check 3x3 subgrid
            int subGridRow = row - row % 3;
            int subGridCol = col - col % 3;

            for (int i = subGridRow; i < subGridRow + 3; i++)
            {
                for (int j = subGridCol; j < subGridCol + 3; j++)
                {
                    if (cell[i][j] == value) // on verifie si value existe deja sur le carre
                        return false;
                }
            }

            return true;

        }

        private bool IsColoringComplete(SudokuGrid grid)
        {
            var cell = grid.Cells;
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (cell[row][col] == 0) //si cell est egale a 0
                        return false;
                }
            }

            return true;
        }


    }
}

