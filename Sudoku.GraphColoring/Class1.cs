using Sudoku.Shared;
using System;

namespace Sudoku.GraphColoring;
public class GraphSolver : ISudokuSolver
{
    public SudokuGrid Solve(SudokuGrid s)
    {

        
        var newSudoku = s.CloneSudoku();

       
        var cells = newSudoku.Cells;
       
        Node[,] _nodes = new Node[9, 9];
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                _nodes[row, col] = new Node(row, col, cells[row][col]);
            }
        }
        SolverColoring graph = new SolverColoring();
        _nodes[1, 1].Value = 6;
        if (graph.SudokuSolver(newSudoku))
        {
           
            var result = graph.Solve(_nodes, newSudoku);
            Console.WriteLine("VALIDE");
        }
        else
            Console.WriteLine("Puzzle couldn't be solved");
       
        

        return newSudoku;

    }

}

