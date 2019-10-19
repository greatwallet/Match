# FILE_INSTRUCTIONS
This page provides basic instructions and core functions of all the `*.cs` files or classes in this folder

* [Equation.cs](Equation.cs): define the node (i.e. state) in the search graph, providing `Search` and `GenerateSearch` function.
  - [Search](Equation.cs#L293): get the possible answer using DFS (Depth First Search) given a puzzle.
  - [GenerateSearch](Equation.cs#L490): get a puzzle using DFS.
* [Expr.cs](Expr.cs): provide functions `evaluate` to compute the result from a string formatted expression.
  - [evaluate](Expr.cs#L87): transfer the expression to RPN (Reverse Polish Notation) and compute the result.
* <b>[MainWindow.cs](MainWindow.cs): The main winform file, combine all classes altogether. </b>
* [Program.cs](Program.cs): provide the entrance of the program
* [SSD.cs](SSD.cs): SSD means Seven-Segment-Display, define how a <b>single</b> digit is stored, how it is converted between "Match form"
, "BCD (Binary Coded Decimal)" and `int32` and relevant functions.
* [SSD_match.cs](SSD_match.cs): provides the visualization of SSD in matches.
