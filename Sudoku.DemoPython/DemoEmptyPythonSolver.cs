using Python.Runtime;
using Sudoku.Shared;
using System.Resources;

namespace Sudoku.DemoPython
{
	public class DemoEmptyPythonSolver : PythonSolverBase
	{
		public override SudokuGrid Solve(SudokuGrid s)
		{
			//using (Py.GIL())
			//{
			// create a Python scope
			using (PyModule scope = Py.CreateScope())
			{
				// convert the Person object to a PyObject
				PyObject pyCells = s.Cells.ToPython();

				// create a Python variable "person"
				scope.Set("instance", pyCells);

				// the person object may now be used in Python
				string code = Resource1.EmptyPythonSolver;
				scope.Exec(code);
				var result = scope.Get("r");
				var managedResult = result.As<int[][]>();
				//var convertesdResult = managedResult.Select(objList => objList.Select(o => (int)o).ToArray()).ToArray();
				return new Shared.SudokuGrid() { Cells = managedResult };
			}
			//}
		}

	}
}