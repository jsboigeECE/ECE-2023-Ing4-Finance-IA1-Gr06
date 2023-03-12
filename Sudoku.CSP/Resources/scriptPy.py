from aima import *
from csp import *

#r=instance
def solve_sudoku(puzzle):
    # Create CSP model for Sudoku
    csp = CSP()

    # Add variables to CSP model 
    for i in range(9):
        for j in range(9):
            if puzzle[i][j] == 0:
                csp.add_variable(f"{i},{j}", [1, 2, 3, 4, 5, 6, 7, 8, 9])
            else:
                csp.add_variable(f"{i},{j}", [puzzle[i][j]])

    # Add constraints to CSP model
    for i in range(9):
        for j in range(9):
            # Row constraint
            row = [f"{i},{x}" for x in range(9) if csp.variables.get(f"{i},{x}")]
            csp.add_constraint(row, lambda a, b: a != b)

            # Column constraint
            col = [f"{x},{j}" for x in range(9) if csp.variables.get(f"{x},{j}")]
            csp.add_constraint(col, lambda a, b: a != b)

            # Box constraint
            box = []
            for x in range(3):
                for y in range(3):
                    bx = i // 3 * 3 + x
                    by = j // 3 * 3 + y
                    if csp.variables.get(f"{bx},{by}"):
                        box.append(f"{bx},{by}")
            csp.add_constraint(box, lambda a, b: a != b)

    # Solve the CSP model
    solution = backtracking_search(csp, select_unassigned_variable=mrv, inference=forward_checking)

    # Convert solution to 2D array
    if solution:
        solution = [[solution.get(f"{i},{j}")[0] for j in range(9)] for i in range(9)]

    return solution

r=solve_sudoku(instance)
