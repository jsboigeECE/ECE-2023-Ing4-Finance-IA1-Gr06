using Sudoku.Shared;
using System;

namespace Sudoku.GraphColoringv2;


public class GraphSolver2 : ISudokuSolver
{
    public SudokuGrid Solve(SudokuGrid s)
    {

        try
        {
            GrapheColoring graphe = new GrapheColoring(s);

            // Coloration algorithme WelshPowell
            Console.WriteLine("ALGORITHME WelshPowell");
            graphe.WelshPowell();
            Console.WriteLine("Affichage du resultat");
            graphe.AfficherGrille();
            Console.WriteLine();
            Console.WriteLine("Verification du resultat");
            return graphe.getGrid();

          

        }
        catch (Exception e)
        {
            Console.Write("Attention ", e);
        }
        return s;






    }

}
