using Sudoku.Shared;
using System;

namespace Sudoku.GraphColoringv2;


public class GraphSolver2 : ISudokuSolver
{
    public SudokuGrid Solve(SudokuGrid s)
    {

        //On récupere le sudoku 
        var grid = s.CloneSudoku();
        //On crée un nouveau graph 'GraphColor' à partir du sudoku 
        var graph = new GraphColor(grid);
        var grid_f = graph.Convert();




        return grid_f;



    }

}
