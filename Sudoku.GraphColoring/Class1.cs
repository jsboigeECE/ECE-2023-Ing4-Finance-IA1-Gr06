using Sudoku.Shared;
using System;

namespace Sudoku.GraphColoring;
public class GraphSolver : ISudokuSolver
{
    public SudokuGrid Solve(SudokuGrid s)
    {

        // return s.CloneSudoku();

        // return s.CloneSudoku();

        //retourne une copie de la grille de sudoku
        var grid = s.CloneSudoku();
        //retourne une copie de la grille de sudoku
        //affiche sur la console "bravo"
        Console.WriteLine("Bravo");
        //crée un nouveau sudoku avec que des "0"
        var newSudoku = s.CloneSudoku();
        //Accède à la l'indice (1,1) de "newSudoku"



        //acceder aux cellules de la grille
        var cells = newSudoku.Cells;
        //Ajouter des "1"
        Console.WriteLine(cells[8][5]);
        
        Node[,] _nodes = new Node[9,9];
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                _nodes[row, col] = new Node(row, col, cells[row][col]);
            }
        }
        _nodes[1, 1].Value = 6; 
        if (newSudoku.SudokuSolver(newSudoku))
        {
            Console.WriteLine(_nodes[0, 0].Value);
            Console.WriteLine(cells[1][1]);
            Console.WriteLine(_nodes[1, 1].Value);
            Console.WriteLine(_nodes[2, 2].Value);
            Console.WriteLine("Bravo1");
            var result = newSudoku.Solve(_nodes, newSudoku);

            Console.WriteLine(newSudoku);
            Console.WriteLine("Bravo2");
        }
        else
            Console.WriteLine("Puzzle couldn't be solved");
        //ajouter des 1 sur la première ligne

        return newSudoku;

    }

}

