using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Sudoku.Shared;
using Z3.LinqBinding;

namespace Sudoku.Z3Solvers
{
    public static class SudokuTheorem
    {
        public static Theorem<SudokuGrid> Create(Z3Context context)
        {
            var sudokuTheorem = context.NewTheorem<SudokuGrid>();

            var cells = typeof(SudokuGrid).GetProperties();

            foreach (var cell in cells)
            {
                sudokuTheorem = sudokuTheorem.Where(Between1And9(cell));
            }

            sudokuTheorem = sudokuTheorem.Where(DistinctRows(cells));
            sudokuTheorem = sudokuTheorem.Where(DistinctColumns(cells));

            sudokuTheorem = sudokuTheorem.Where(t => Z3Methods.Distinct(t.Cells[1][1], t.Cells[1][2], t.Cells[1][3],t.Cells[2][1], t.Cells[2][2], t.Cells[2][3], t.Cells[3][1], t.Cells[3][2], t.Cells[3][3]));
            sudokuTheorem = sudokuTheorem.Where(t => Z3Methods.Distinct(t.Cells[1][4], t.Cells[1][5], t.Cells[1][6], t.Cells[2][4], t.Cells[2][5], t.Cells[2][6], t.Cells[3][4], t.Cells[3][5], t.Cells[3][6]));
            sudokuTheorem = sudokuTheorem.Where(t => Z3Methods.Distinct(t.Cells[1][7], t.Cells[1][8], t.Cells[1][9], t.Cells[2][7], t.Cells[2][8], t.Cells[2][9], t.Cells[3][7], t.Cells[3][8], t.Cells[3][9]));
            sudokuTheorem = sudokuTheorem.Where(t => Z3Methods.Distinct(t.Cells[4][1], t.Cells[4][2], t.Cells[4][3], t.Cells[5][1], t.Cells[5][2], t.Cells[5][3], t.Cells[6][1], t.Cells[6][2], t.Cells[6][3]));
            sudokuTheorem = sudokuTheorem.Where(t => Z3Methods.Distinct(t.Cells[4][4], t.Cells[4][5], t.Cells[4][6], t.Cells[5][4], t.Cells[5][5], t.Cells[5][6], t.Cells[6][4], t.Cells[6][5], t.Cells[6][6]));
            sudokuTheorem = sudokuTheorem.Where(t => Z3Methods.Distinct(t.Cells[4][7], t.Cells[4][8], t.Cells[4][9], t.Cells[5][7], t.Cells[5][8], t.Cells[5][9], t.Cells[6][7], t.Cells[6][8], t.Cells[6][9]));
            sudokuTheorem = sudokuTheorem.Where(t => Z3Methods.Distinct(t.Cells[7][1], t.Cells[7][2], t.Cells[7][3], t.Cells[8][1], t.Cells[8][2], t.Cells[8][3], t.Cells[9][1], t.Cells[9][2], t.Cells[9][3]));
            sudokuTheorem = sudokuTheorem.Where(t => Z3Methods.Distinct(t.Cells[7][4], t.Cells[7][5], t.Cells[7][6], t.Cells[8][4], t.Cells[8][5], t.Cells[8][6], t.Cells[9][4], t.Cells[9][5], t.Cells[9][6]));
            sudokuTheorem = sudokuTheorem.Where(t => Z3Methods.Distinct(t.Cells[7][7], t.Cells[7][8], t.Cells[7][9], t.Cells[8][7], t.Cells[8][8], t.Cells[8][9], t.Cells[9][7], t.Cells[9][8], t.Cells[9][9]));

            return sudokuTheorem;
        }

        private static Expression<System.Func<SudokuGrid, bool>> Between1And9(PropertyInfo cellProperty)
        {
            ParameterExpression tParam = Expression.Parameter(typeof(SudokuGrid), "t");
            MemberExpression cell = Expression.Property(tParam, cellProperty);

            ConstantExpression one = Expression.Constant(1, typeof(int[]));
            ConstantExpression nine = Expression.Constant(9, typeof(int[]));

            BinaryExpression cellGreaterThanOrEqual1 = Expression.GreaterThanOrEqual(cell, one);
            BinaryExpression cellLessThanOrEqual9 = Expression.LessThanOrEqual(cell, nine);

            var expr = Expression.Lambda<System.Func<SudokuGrid, bool>>(
                Expression.And(cellGreaterThanOrEqual1, cellLessThanOrEqual9),
                new[] { tParam });

            return expr;
        }

        private static Expression<System.Func<SudokuGrid, bool>> Distinct(PropertyInfo[] cells, string cellPattern)
        {
            ParameterExpression tParam = Expression.Parameter(typeof(SudokuGrid), "t");

            Expression distincts = null;

            for (int distinctIndex = 1; distinctIndex <= 9; distinctIndex++)
            {
                var cellsInDistinct = new List<MemberExpression>();
                for (int otherIndex = 1; otherIndex <= 9; otherIndex++)
                {
                    var cellName = string.Format(cellPattern, distinctIndex, otherIndex);

                    MemberExpression cell = Expression.Property(tParam, cells.Single(_ => _.Name == cellName));
                    cellsInDistinct.Add(cell);
                }

                NewArrayExpression distinctArray = Expression.NewArrayInit(typeof(int), cellsInDistinct);
                MethodCallExpression distinct = Expression.Call(typeof(Z3Methods), "Distinct", new[] { typeof(int) }, distinctArray);

                if (distincts == null)
                {
                    distincts = distinct;
                }
                else
                {
                    distincts = Expression.And(distincts, distinct);
                }
            }

            var expr = Expression.Lambda<System.Func<SudokuGrid, bool>>(
                distincts,
                new[] { tParam });

            return expr;
        }

        private static Expression<System.Func<SudokuGrid, bool>> DistinctColumns(PropertyInfo[] cells)
        {
            return Distinct(cells, "Cell{1}{0}");
        }

        private static Expression<System.Func<SudokuGrid, bool>> DistinctRows(PropertyInfo[] cells)
        {
            return Distinct(cells, "Cell{0}{1}");
        }
    }
}