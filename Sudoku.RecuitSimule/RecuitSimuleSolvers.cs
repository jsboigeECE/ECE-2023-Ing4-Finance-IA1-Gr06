using Python.Runtime;
using Sudoku.Shared;
using System.Resources;

namespace Sudoku.RecuitSimule
{
 
    public class RecuitSimulePythonSolver : PythonSolverBase
    {
        public override Shared.SudokuGrid Solve(Shared.SudokuGrid s)
        {

            //using (Py.GIL())
            //{
            // create a Python scope
            using (PyModule scope = Py.CreateScope())
            {
                // convert the Person object to a PyObject
                PyObject pyCells = s.Cells.ToPython();

                // create a Python variable "person"
                scope.Set("sudokud", pyCells);

                string numpyConverter = Resource1.Numpy_convert;
                scope.Exec(numpyConverter);

                // the person object may now be used in Python
                string code = Resource1.RecuitSimule;
                scope.Exec(code);
                var result = scope.Get("solution");
                var managedResult = result.As<int[,]>().ToJaggedArray();
                //var convertesdResult = managedResult.Select(objList => objList.Select(o => (int)o).ToArray()).ToArray();
                return new Shared.SudokuGrid() { Cells = managedResult };
            }
            //}

        }



        protected override void InitializePythonComponents()
        {
            InstallPipModule("numpy");

            base.InitializePythonComponents();
        }

    }




    public class SimannealSolverPython : PythonSolverBase
    {
        public override Shared.SudokuGrid Solve(Shared.SudokuGrid s)
        {

            //using (Py.GIL())
            //{
            // create a Python scope
            using (PyModule scope = Py.CreateScope())
            {
                // convert the Person object to a PyObject
                PyObject pyCells = s.Cells.ToPython();

                // create a Python variable "person"
                scope.Set("sudokud", pyCells);

                string numpyConverter = Resource1.Numpy_convert;
                scope.Exec(numpyConverter);

                // the person object may now be used in Python
                string code = Resource1.RecuitSimule;
                scope.Exec(code);
                var result = scope.Get("solution");
                var managedResult = result.As<int[,]>().ToJaggedArray();
                //var convertesdResult = managedResult.Select(objList => objList.Select(o => (int)o).ToArray()).ToArray();
                return new Shared.SudokuGrid() { Cells = managedResult };
            }
            //}

        }


        protected override void InitializePythonComponents()
        {
            InstallPipModule("numpy");
            InstallPipModule("simanneal");
            base.InitializePythonComponents();
        }







    }

}
