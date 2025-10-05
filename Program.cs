namespace NotAnotherNameSpaceName
{
    // Online C# Editor for free
    // Write, Edit and Run your C# code using C# Online Compiler

    using System;
    using System.Collections.Generic;

    public class HelloWorld
    {
        //Note to the Editor: write Minimizer and Maximizer 
        //Note to the Editor: Implement shallow reference in Position to parent position (They are useful)
        //Evaluation should work backwards through the tree to minimize opponent's advantage and maximize personal advantage
        public class Position
        {
            public char[,] grid = new char[7, 6];
            public int evaluation;
            public int lastPlayedMoveX;
            public int lastPlayedMoveY;
            public bool lastPlayerIsX = false;
            public int numMovesPlayed;
            public List<int> movesPlayed = new List<int>();
            public List<Position> children = new List<Position>();
            public Position parent;//Shallow References
            public Position bestOffShoot;
            //X = X
            //O = O
            //_ = Blank
            public Position()
            {
                for (int i = 0; i < grid.GetLength(0); i++)
                {
                    for (int j = 0; j < grid.GetLength(1); j++)
                    {
                        grid[i, j] = '_';
                    }
                }
            }
            public Position(char[,] toGrid)
            {
                grid = toGrid;
            }
            public Position(char[,] toGrid, int _lastPlayedMoveX, int _lastPlayedMoveY, bool _lastPlayerIsX, int _numMovesPlayed, List<int> _movesPlayed)
            {
                grid = toGrid;
                lastPlayedMoveX = _lastPlayedMoveX;
                lastPlayedMoveY = _lastPlayedMoveY;
                lastPlayerIsX = _lastPlayerIsX;
                numMovesPlayed = _numMovesPlayed;
                for (int i = 0; i < _movesPlayed.Count; i++)
                {
                    movesPlayed.Add(_movesPlayed[i]);
                }
                if (movesPlayed.Count > numMovesPlayed)
                {
                    Console.WriteLine(movesPlayed.Count > numMovesPlayed);
                }
            }
            public bool[] GetPlayableMoves()
            {
                bool[] toReturn = new bool[7];
                for (int i = 0; i < grid.GetLength(0); i++)
                {
                    if (grid[i, 0] == '_')
                    {
                        toReturn[i] = true;
                    }
                }
                return toReturn;
            }
            public void PrintPosition()
            {
                for (int i = 0; i < grid.GetLength(1); i++)
                {
                    for (int j = 0; j < grid.GetLength(0); j++)
                    {
                        if(j == lastPlayedMoveX && i == lastPlayedMoveY)
                        {
                            Console.ForegroundColor = ConsoleColor.Magenta;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        Console.Write(grid[j, i] + " ");
                    }
                    Console.WriteLine();
                }
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("==============");
                Console.WriteLine("0 1 2 3 4 5 6 ");
            }
            public bool IsWinningMove(int xPos, int yPos, bool isX)
            {
                //CheckHorizontal
                char toCompare = ' ';
                if (isX)
                {
                    toCompare = 'X';
                }
                else
                {
                    toCompare = 'O';
                }
                int numToLeft = 0;
                for (int i = 1; i < 4; i++)
                {
                    if (xPos - i >= 0)
                    {
                        if (grid[xPos - i, yPos] == toCompare)
                        {
                            numToLeft++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                int numToRight = 0;
                for (int i = 1; i < 4; i++)
                {
                    if (xPos + i < 7)
                    {
                        if (grid[xPos + i, yPos] == toCompare)
                        {
                            numToRight++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                if (numToLeft + numToRight > 2)
                {
                    return true;
                }
                //Check Vertical
                int numDownwards = 0;
                for (int i = 1; i < 4; i++)
                {
                    if (yPos - i >= 0)
                    {
                        if (grid[xPos, yPos - i] == toCompare)
                        {
                            numDownwards++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                int numUpwards = 0;
                for (int i = 1; i < 4; i++)
                {
                    if (yPos + i < 6)
                    {
                        if (grid[xPos, yPos + i] == toCompare)
                        {
                            numUpwards++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                if (numDownwards + numUpwards > 2)
                {
                    return true;
                }
                //CheckRegularDiagonal
                int numUpLeft = 0;
                for (int i = 1; i < 4; i++)
                {
                    if (xPos - i >= 0 && yPos - i >= 0)
                    {
                        if (grid[xPos - i, yPos - i] == toCompare)
                        {
                            numUpLeft++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                int numDownRight = 0;
                for (int i = 1; i < 4; i++)
                {
                    if (xPos + i < 7 && yPos + i < 6)
                    {
                        if (grid[xPos + i, yPos + i] == toCompare)
                        {
                            numDownRight++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                if (numUpLeft + numDownRight > 2)
                {
                    return true;
                }
                //CheckBackwardsDiagonal
                int numDownLeft = 0;
                for (int i = 1; i < 4; i++)
                {
                    if (xPos - i >= 0 && yPos + i < 6)
                    {
                        if (grid[xPos - i, yPos + i] == toCompare)
                        {
                            numDownLeft++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                int numRightUp = 0;
                for (int i = 1; i < 4; i++)
                {
                    if (xPos + i < 7 && yPos - i >= 0)
                    {
                        if (grid[xPos + i, yPos - i] == toCompare)
                        {
                            numRightUp++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                if (numDownLeft + numRightUp > 2)
                {
                    return true;
                }
                return false;
            }
            public int numConnections(int xPos, int yPos, bool isX)
            {
                //CheckHorizontal
                char toCompare = ' ';
                if (isX)
                {
                    toCompare = 'X';
                }
                else
                {
                    toCompare = 'O';
                }
                int numToLeft = 0;
                for (int i = 1; i < 4; i++)
                {
                    if (xPos - i >= 0)
                    {
                        if (grid[xPos - i, yPos] == toCompare)
                        {
                            numToLeft++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                int numToRight = 0;
                for (int i = 1; i < 4; i++)
                {
                    if (xPos + i < 7)
                    {
                        if (grid[xPos + i, yPos] == toCompare)
                        {
                            numToRight++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                //Check Vertical
                int numDownwards = 0;
                for (int i = 1; i < 4; i++)
                {
                    if (yPos - i >= 0)
                    {
                        if (grid[xPos, yPos - i] == toCompare)
                        {
                            numDownwards++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                int numUpwards = 0;
                for (int i = 1; i < 4; i++)
                {
                    if (yPos + i < 6)
                    {
                        if (grid[xPos, yPos + i] == toCompare)
                        {
                            numUpwards++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                //CheckRegularDiagonal
                int numUpLeft = 0;
                for (int i = 1; i < 4; i++)
                {
                    if (xPos - i >= 0 && yPos - i >= 0)
                    {
                        if (grid[xPos - i, yPos - i] == toCompare)
                        {
                            numUpLeft++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                int numDownRight = 0;
                for (int i = 1; i < 4; i++)
                {
                    if (xPos + i < 7 && yPos + i < 6)
                    {
                        if (grid[xPos + i, yPos + i] == toCompare)
                        {
                            numDownRight++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                //CheckBackwardsDiagonal
                int numDownLeft = 0;
                for (int i = 1; i < 4; i++)
                {
                    if (xPos - i >= 0 && yPos + i < 6)
                    {
                        if (grid[xPos - i, yPos + i] == toCompare)
                        {
                            numDownLeft++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                int numRightUp = 0;
                for (int i = 1; i < 4; i++)
                {
                    if (xPos + i < 7 && yPos - i >= 0)
                    {
                        if (grid[xPos + i, yPos - i] == toCompare)
                        {
                            numRightUp++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                //Console.WriteLine($"{numDownLeft} {numDownRight} {numToLeft} {numToRight} {numUpwards} {numDownwards} {isX}");
                return numDownLeft + numDownRight + numToLeft + numToRight + numUpwards + numDownwards + numRightUp + numUpLeft;
            }
            public int totalNumConnections(bool isX)
            {
                char charChecker = ' ';
                int toReturn = 0;
                if (isX)
                {
                    charChecker = 'X';
                }
                else
                {
                    charChecker = 'O';
                }
                for (int i = 0; i < grid.GetLength(0); i++)
                {
                    for (int j = 0; j < grid.GetLength(1); j++)
                    {
                        if(grid[i, j] == charChecker)
                        {
                            toReturn += numConnections(i, j, isX);
                        }
                    }
                }
                return toReturn;
            }
            public bool IsWinningMove(int xPos, bool isX)
            {
                int yPos = 0;
                for (int i = 5; i >= 0; i--)
                {
                    if (grid[xPos, i] == '_')
                    {
                        yPos = i;
                    }
                }
                return IsWinningMove(xPos, yPos, isX);
            }
            public bool DidJustWin()
            {
                return IsWinningMove(lastPlayedMoveX, lastPlayedMoveY, lastPlayerIsX);
            }
            public bool TogglePosition(int slot, bool isX) //Num 0-7
            {
                for (int i = 5; i >= 0; i--)
                {
                    if (grid[slot, i] == '_')
                    {
                        if (isX)
                        {
                            grid[slot, i] = 'X';
                            lastPlayedMoveX = slot;
                            lastPlayedMoveY = i;
                            lastPlayerIsX = true;
                            numMovesPlayed++;
                            movesPlayed.Add(i);
                            if(movesPlayed.Count > numMovesPlayed)
                            {
                                Console.WriteLine(movesPlayed.Count > numMovesPlayed);
                            }
                            return true;
                        }
                        else
                        {
                            grid[slot, i] = 'O';
                            lastPlayedMoveX = slot;
                            lastPlayedMoveY = i;
                            lastPlayerIsX = false;
                            numMovesPlayed++;
                            movesPlayed.Add(i);
                            if (movesPlayed.Count > numMovesPlayed)
                            {
                                Console.WriteLine(movesPlayed.Count > numMovesPlayed);
                            }
                            return true;
                        }
                    }
                }
                return false;
            }
            public char[,] ClonePosition()
            {
                char[,] toReturn = new char[7, 6];
                for (int i = 0; i < toReturn.GetLength(0); i++)
                {
                    for (int j = 0; j < toReturn.GetLength(1); j++)
                    {
                        toReturn[i, j] = grid[i, j];
                    }
                }
                return toReturn;
            }
            public List<int> CloneMovesPlayed()
            {
                List<int> toReturn = new List<int>();
                for(int i = 0; i < movesPlayed.Count; i++)
                {
                    toReturn.Add(movesPlayed[i]);
                }
                return toReturn;
            }
            public bool CanWinOnNextMove()
            {
                for (int i = 0; i < GetPlayableMoves().Length; i++)
                {
                    Position tempPosition = new Position(ClonePosition(), lastPlayedMoveX, lastPlayedMoveY, lastPlayerIsX, numMovesPlayed, CloneMovesPlayed());
                    if (tempPosition.GetPlayableMoves()[i])
                    {
                        tempPosition.TogglePosition(i, !lastPlayerIsX);
                        if(tempPosition.DidJustWin())
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            public int EvaluationForX() //Always Returns the value for the Player with the X pieces, Positive is better for X
            {
                int modifier = 0;
                int toReturn = 0;
                if (lastPlayerIsX == false)
                {
                    modifier = -1;
                }
                else
                {
                    modifier = 1;
                }
                if (DidJustWin())
                {
                    toReturn = (1000 * modifier);
                }
                else if (CanWinOnNextMove())
                {
                    toReturn = (-1000 * modifier);
                }
                int numConnectionsX = totalNumConnections(true);
                int numConnectionsO = totalNumConnections(false);
                //Console.WriteLine($"{numConnectionsX}, {numConnectionsO}, {numConnectionsX - numConnectionsO}");
                //PrintPosition();
                toReturn += (numConnectionsX - numConnectionsO);
                for (int i = 0; i < 9; i++)
                {
                    int currentX = i % 3;
                    int currentY = (int)(i / 3.0);
                    if ((grid[currentX + 2, currentY + 1] == 'X' && lastPlayerIsX) || (grid[currentX + 2, currentY + 1] == 'O' && !lastPlayerIsX))
                    {
                        toReturn += (3 * modifier);
                    }
                    if ((grid[currentX + 2, currentY + 1] == 'X' && !lastPlayerIsX) || (grid[currentX + 2, currentY + 1] == 'O' && lastPlayerIsX))
                    {
                        toReturn -= (3 * modifier);
                    }
                }
                evaluation = toReturn;
                return toReturn;
            }
        }
        public static Position minimizer(Position inPosition)
        {
            List<Position> branches = inPosition.children;
            Position lowest = branches[0];
            int lowestEval = int.MaxValue;
            for (int i = 0; i < branches.Count; i++)
            {
                int currentEval = branches[i].evaluation;
                //Console.Write($"{currentEval}, ");
                //if (Math.Abs(currentEval) > 6)
                //{
                  //  branches[i].PrintPosition();
                  //  Console.WriteLine(currentEval);
                //}
                if (currentEval < lowestEval)
                {
                    lowest = branches[i];
                    lowestEval = currentEval;
                }
            }
            //Console.WriteLine($"Low: {lowestEval}  This Should be the Same Number = {lowest.evaluation}");
            return lowest;
        }
        public static Position maximizer(Position inPosition)
        {
            List<Position> branches = inPosition.children;
            Position highest = branches[0];
            int highestEval = int.MinValue;
            for (int i = 0; i < branches.Count; i++)
            {
                int currentEval = branches[i].evaluation;
                //Console.Write($"{currentEval}, ");
                //if (Math.Abs(currentEval) > 6)
                //{
                 //   branches[i].PrintPosition();
                  //  Console.WriteLine(currentEval);
                //}
                if (currentEval > highestEval)
                {
                    //if(highestEval != int.MinValue)
                    //{
                    //    Console.WriteLine($"Old Highest: {highestEval} New Highest: {currentEval}");
                    //}
                    highest = branches[i];
                    highestEval = currentEval;
                }
            }
            //Console.WriteLine($"Highest: {highestEval} This Should be the Same Number = {highest.evaluation} ");
            return highest;
        }
        public static List<Position> fullNextGeneration(List<Position> tree, int depth, bool isX)
        {
            List<Position> toReturn = new List<Position>();
            for (int i = 0; i < tree.Count; i++)
            {
                List<Position> toReturnOfI = newGeneration(tree[i], isX);
                for (int j = 0; j < toReturnOfI.Count; j++)
                {
                    toReturn.Add(toReturnOfI[j]);
                }
            }
            if (depth > 1)
            {
                return fullNextGeneration(toReturn, depth - 1, !isX);
            }
            else
            {
                return toReturn;
            }
        }
        public static List<Position> newGeneration(Position inPosition, bool isX)
        {
            List<Position> toReturn = new List<Position>();
            if(inPosition.DidJustWin())
            {
                bool needsToBeAdded = true;
                for(int i = 0; i < inPosition.parent.children.Count; i++)
                {
                    if(inPosition == inPosition.parent.children[i])
                    {
                        needsToBeAdded = false;
                        //Console.WriteLine("This should happen at least once");
                    }
                }
                if(needsToBeAdded)
                {
                    inPosition.parent.children.Add(inPosition);
                }
                toReturn.Add(inPosition);
                return toReturn;
            }
            for (int i = 0; i < 7; i++)
            {
                if (inPosition.GetPlayableMoves()[i])
                {
                    Position toAdd = new Position(inPosition.ClonePosition(), inPosition.lastPlayedMoveX, inPosition.lastPlayedMoveY, !isX, inPosition.numMovesPlayed, inPosition.CloneMovesPlayed());
                    if (isX == toAdd.lastPlayerIsX)
                    {
                        Console.WriteLine("This should never happen x2");
                    }
                    toAdd.TogglePosition(i, isX);
                    toAdd.parent = inPosition;
                    toReturn.Add(toAdd);
                }
            }
            inPosition.children = toReturn;
            return toReturn;
        }
        public static int bestMoveInPosition(Position inPosition, bool isX, int depth, int nextMoveNumber)
        {
            //isActivePlayerAtEndOfTree
            List<Position> tree = fullNextGeneration(newGeneration(inPosition, isX), depth, !isX);
            bool[] possibilities = inPosition.GetPlayableMoves();
            List<Position> focusGen = tree;
            //Find Evaluations For Final Positions
            for(int i = 0; i < tree.Count; i++)
            {
                int eval = tree[i].EvaluationForX();
                if (eval != tree[i].evaluation)
                {
                    Console.WriteLine("This should never happen x3");
                }
            }
            //Assemble Parent Nodes
            for (int i = 0; i <= depth; i++)
            {
                HashSet<Position> parents = new HashSet<Position>();
                List<Position> toFocusGen = new List<Position>();
                for (int j = 0; j < focusGen.Count; j++)
                {
                    if(focusGen[j].parent == null)
                    {
                        toFocusGen.Add(focusGen[j]);
                        continue;
                    }
                    if (parents.Add(focusGen[j].parent))
                    {
                        toFocusGen.Add(focusGen[j].parent);
                    }
                }
                for (int j = 0; j < parents.Count; j++)
                {
                    Position chosenOffShoot = null;
                    if (toFocusGen[j].lastPlayerIsX)
                    {
                       chosenOffShoot = minimizer(toFocusGen[j]);
                       //Console.Write("Min ");
                    }
                    else
                    {
                        chosenOffShoot = maximizer(toFocusGen[j]);
                        //Console.Write("Max ");
                    }
                    toFocusGen[j].evaluation = chosenOffShoot.evaluation;
                    toFocusGen[j].bestOffShoot = chosenOffShoot;
                    //Console.WriteLine($"Eval: {toFocusGen[j].evaluation} At {depth - i}");
                }
                focusGen = toFocusGen;
            }
            return inPosition.bestOffShoot.lastPlayedMoveX;
        }
        public static void Main(string[] args)
        {
            Position truePosition = new Position();
            bool turn = true;
            while (!truePosition.IsWinningMove(truePosition.lastPlayedMoveX, truePosition.lastPlayedMoveY, truePosition.lastPlayerIsX))
            {
                truePosition.PrintPosition();
                //Console.WriteLine($"Evaluation: {truePosition.evaluation}");
                //Console.WriteLine($"Connections O: {truePosition.totalNumConnections(false)} Connections X: {truePosition.totalNumConnections(true)}");
                Console.WriteLine("Play a Move");
                string response = Console.ReadLine();
                bool selectMode = false;
                if(response == "/setup")
                {
                    selectMode = true;
                    string inputPosition = Console.ReadLine();
                    string[] moves = inputPosition.Split(" ");
                    int[] intMoves = new int[moves.Length];
                    for (int i = 0; i < intMoves.Length; i++)
                    {
                        intMoves[i] = Int32.Parse(moves[i]);
                        truePosition.TogglePosition(intMoves[i], turn);
                        turn = !turn;
                    }
                }
                if(selectMode)
                {
                    truePosition.PrintPosition();
                    Console.WriteLine($"Dave Reccomends: {bestMoveInPosition(truePosition, turn, 5, truePosition.numMovesPlayed + 1)}");
                    Console.WriteLine($"Evaluation: +{truePosition.evaluation}");
                    response = Console.ReadLine();
                }
                int move = Int32.Parse(response);
                truePosition.TogglePosition(move, turn);
                turn = !turn;
                if(selectMode && truePosition.IsWinningMove(truePosition.lastPlayedMoveX, truePosition.lastPlayedMoveY, truePosition.lastPlayerIsX))
                {
                    truePosition.PrintPosition();
                    Console.WriteLine($"Evaluation: {truePosition.EvaluationForX()} Does The Engine Know The Game Is Over: {truePosition.DidJustWin()}");
                }
                if (!selectMode)
                {
                    truePosition.PrintPosition();
                    if (truePosition.IsWinningMove(truePosition.lastPlayedMoveX, truePosition.lastPlayedMoveY, truePosition.lastPlayerIsX))
                    {
                        Console.WriteLine("Game Over");
                        return;
                    }
                    if (turn == truePosition.lastPlayerIsX)
                    {
                        Console.WriteLine("This should never happen x1");
                    }
                    Console.WriteLine("The Bot Will Now Play Their Move");
                    move = bestMoveInPosition(truePosition, turn, 5, truePosition.numMovesPlayed + 1);
                    Console.WriteLine(move);
                    truePosition.TogglePosition(move, turn);
                    turn = !turn;
                    if (truePosition.IsWinningMove(truePosition.lastPlayedMoveX, truePosition.lastPlayedMoveY, truePosition.lastPlayerIsX))
                    {
                        truePosition.PrintPosition();
                        Console.WriteLine("Game Over");
                        return;
                    }
                }
            }
        }
    }
}