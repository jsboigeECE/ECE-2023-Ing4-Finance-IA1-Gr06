using System;

namespace Sudoku.Shared
{
    public class Node
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public int Value { get; set; }
        public int Color { get; set; }

        public Node(int row, int col, int value)
        {
            Row = row;
            Col = col;
            Value = value;
            Color = 0;
        }
    }
}

