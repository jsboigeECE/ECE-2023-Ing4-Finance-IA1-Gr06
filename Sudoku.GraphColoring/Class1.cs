using Sudoku.Shared;
using System;


namespace Sudoku.GraphColoring
{
    

    public class GraphSolver : ISudokuSolver
    {
         

        public SudokuGrid Solve(SudokuGrid s)
        {
            

            var grid = s.CloneSudoku(); //Sudoku incomplet
           
            WelshPowellSolver wpSolver = new WelshPowellSolver();

            var solved = wpSolver.SolveSudoku(grid); //Solve the sudoku using the WelshPowellSolver

         

            return solved;
            



        }


    }
}
