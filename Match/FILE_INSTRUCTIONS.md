# FILE_INSTRUCTIONS
This page provides basic instructions and core functions of all the `*.cs` files or classes in this folder

* [Equation.cs](Match/Equation.cs): define the node (i.e. state) in the search graph, providing `Search` and `GenerateSearch` function.
  - [Search](Match/Equation.cs#L293): get the possible answer using DFS (Depth First Search) given a puzzle.
  - [GenerateSearch](Match/Equation.cs#L490): get a puzzle using DFS.
* [Expr.cs](Match/Expr.cs): provide functions `evaluate` to compute the result from a string formatted expression.
  - [evaluate](Match/Expr.cs#L87): transfer the expression to RPN (Reverse Polish Notation) and compute the result.
* <b>[MainWindow.cs](Match/MainWindow.cs): The main winform file, combine all classes altogether. </b>
* [Program.cs](Match/Program.cs): provide the entrance of the program
* [SSD.cs](Match/SSD.cs): SSD means Seven-Segment-Display, define how a <b>single</b> digit is stored, how it is converted between "Match form"
, "BCD (Binary Coded Decimal)" and `int32` and relevant functions.
* [SSD_match.cs](Match/SSD_match.cs): provides the visualization of SSD in matches.
