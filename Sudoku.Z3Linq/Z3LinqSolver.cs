using Sudoku.Shared;
using Z3.LinqBinding;


namespace Sudoku.Z3Linq
{
    public class Z3LinqSolver : ISudokuSolver
    {

	    private static readonly int[] Indices = Enumerable.Range(0, 9).ToArray();

		public SudokuGrid Solve(SudokuGrid s)
        {
            //Console.WriteLine("Le solver Z3-Linq a bien été appelé !");
            {
                using (var ctx = new Z3Context())
                {
                    var theorem = CreateTheorem(ctx, s);
                    s = theorem.Solve();
                }
            }
            return s;
        }
        
          public static Theorem<SudokuGrid> CreateTheorem(Z3Context context, SudokuGrid s)
        {
            var toReturn = Create(context);
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (s.Cells[i][j] != 0)
                    {
                        var idxi = i;
                        var idxj = j;
                        var cellValue = s.Cells[i][j];
                        toReturn = toReturn.Where(sudoku => sudoku.Cells[idxi][idxj] == cellValue);
                    }
                }
            }
            return toReturn;
        }
        
        public static Theorem<SudokuGrid> Create(Z3Context context)
        {
            var sudokuTheorem = context.NewTheorem<SudokuGrid>();

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    // To avoid closure side effects with lambdas, we copy indices to local variables
                    var i1 = i;
                    var j1 = j;
                    sudokuTheorem = sudokuTheorem.Where(sudoku => (sudoku.Cells[i1][j1] >= 1) && (sudoku.Cells[i1][j1] <= 9));
                }
            }

            // Rows must have distinct digits
            for (int r = 0; r < 9; r++)
            {
                // Again we avoid Lambda closure side effects
                var r1 = r;
				//sudokuTheorem = sudokuTheorem.Where(t =>
				//    Z3Methods.Distinct(t.Cells[r1].Select((cell, j) => t.Cells[r1][j]).ToArray()));
				sudokuTheorem = sudokuTheorem.Where(t =>
					Z3Methods.Distinct(Indices.Select(j => t.Cells[r1][j]).ToArray()));
			}

            // Columns must have distinct digits
            for (int c = 0; c < 9; c++)
            {
                // Again we avoid Lambda closure side effects
                var c1 = c;
				//sudokuTheorem = sudokuTheorem.Where(t => Z3Methods.Distinct(t.Cells.Select((row, i) => t.Cells[i][c1]).ToArray()));
				sudokuTheorem = sudokuTheorem.Where(t => Z3Methods.Distinct(Indices.Select(i => t.Cells[i][c1]).ToArray()));
			}

            // Boxes must have distinct digits
            for (int b = 0; b < 9; b++)
            {
                //On Ã©vite les effets de bords par closure
                var b1 = b;
                // We retrieve to top left cell for all boxes, using integer division and remainders.
                var iStart = b1 / 3 * 3;
                var jStart = b1 % 3 * 3;
                //var indexStart = iStart * 3 * 9 + jStart * 3;
                sudokuTheorem = sudokuTheorem.Where(t => Z3Methods.Distinct(new int[]
                        {
                            t.Cells[iStart][jStart],
                            t.Cells[iStart][jStart+1],
                            t.Cells[iStart][jStart+2],
                            t.Cells[iStart+1][jStart],
                            t.Cells[iStart+1][jStart+1],
                            t.Cells[iStart+1][jStart+2],
                            t.Cells[iStart+2][jStart],
                            t.Cells[iStart+2][jStart+1],
                            t.Cells[iStart+2][jStart+2],
                        }
                    )
                );
            }
            return sudokuTheorem;
        }
    }
}

