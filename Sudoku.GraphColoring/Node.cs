using System;
namespace Sudoku.GraphColoring
{
    public class Node
    {
        public int Row { get; set; } //Ligne
        public int Col { get; set; } //Colonne
        public int Value { get; set; } //Valeur (chiffre dans cellule)
        public int Color { get; set; } //Couleur
        public List<Node> AdjacentNodes { get; set; }

        public Node(int row, int col, int value)
        {
            Row = row;
            Col = col;
            Value = value;
            Color = 0;
            AdjacentNodes = new List<Node>();
        }
    }
}

