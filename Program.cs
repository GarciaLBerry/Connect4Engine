namespace NotAnotherNameSpaceName
{   
    using System;
    using System.Collections.Generic;
    public class HelloWorld
    {
        //Position represents the board and tracks it's contents, including the locations of X's and O's
        public class Position
        {
            private char[,] grid = new char[7, 6];
            private int evaluation;
            private int lastPlayedMoveX;
            private int lastPlayedMoveY;
            private bool lastPlayerIsX = false;
            private int numMovesPlayed;
            private List<int> movesPlayed = new List<int>();
            private List<Position> children = new List<Position>();
            private Position parent;
            private Position bestOffShoot;
            
            //Declares a blank Position, using '_' as empty spaces
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
            
            //Declares a position that is a copy of the char[,] provided
            public Position(char[,] toGrid)
            {
                grid = toGrid;
            }
            
            //Declares a position with all data provided
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
            }
            
            //Returns an array of booleans representing the top slots in the board, with true for a move that is playable
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
            
            //Prints the boardstate, including numbers at the bottom representing each column
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
            
            //Takes in the x position, y position, and player of a move
            //Returns true if the move would win that player the game and false otherwise
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
            
            //Takes in the x position, y position, and player of a move
            //Returns the number of connections to other adjacent pieces that move would have
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
                return numDownLeft + numDownRight + numToLeft + numToRight + numUpwards + numDownwards + numRightUp + numUpLeft;
            }
            
            //Returns the total number of connections between pieces that a player has
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
            
            //Takes in the board slot and player of a move
            //Returns true if the move would win that player the game and false otherwise
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
            
            //Returns true if the last move just won that player the game
            public bool DidJustWin()
            {
                return IsWinningMove(lastPlayedMoveX, lastPlayedMoveY, lastPlayerIsX);
            }
            
            //Takes in the board slot and player of a move
            //Changes the bottom cell in that slot to the represent that player's piece
            //Returns false if the move is illegal and true otherwise
            public bool TogglePosition(int slot, bool isX)
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
                            return true;
                        }
                    }
                }
                return false;
            }
            
            //Returns a deep copy of the position
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
            
            //Returns a deep copy of the move transcript
            public List<int> CloneMovesPlayed()
            {
                List<int> toReturn = new List<int>();
                for(int i = 0; i < movesPlayed.Count; i++)
                {
                    toReturn.Add(movesPlayed[i]);
                }
                return toReturn;
            }
            
            //Returns true if the active player can win on the next move and false otherwise
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
            
            //Calculates and returns the evaluation for the X pieces in the current position (Positive is better)
            public int MakeEvaluation()
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
                    toReturn = (10000 * modifier) - (100 * numMovesPlayed * modifier);
                }
                else if (CanWinOnNextMove())
                {
                    toReturn = (-10000 * modifier) + (100 * numMovesPlayed * modifier);
                }
                int numConnectionsX = totalNumConnections(true);
                int numConnectionsO = totalNumConnections(false);
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
            
            //Returns the evaluation for the current position
            //(Make sure it has already been calculated before using this function)
            public int GetEvaluation()
            {
                return evaluation;
            }
            
            //Returns True if the last move was played by X and false otherwise
            public bool IsLastPlayerX()
            {
                return lastPlayerIsX;
            }
            
            //Returns the Position that represents the parent in the decision tree of this position
            public Position GetParent()
            {
                return parent;
            }
            
            //Returns the best child position of this position in the decision tree 
            public Position GetBestOffShoot()
            {
                return bestOffShoot;
            }
            
            //Returns the full list of children in the decision tree from this position
            public List<Position> GetChildren()
            {
                return children;
            }
            
            //Returns the last move that the X player played
            public int GetLastXMove()
            {
                return lastPlayedMoveX;
            }
            
            //Returns the last move that the Y player played
            public int GetLastYMove()
            {
                return lastPlayedMoveY;
            }
            
            //Returns the total number of moves that have been played
            public int GetNumMovesPlayed()
            {
                return numMovesPlayed;
            }
            
            //Sets the best offshoot of this position in the decision tree
            public void SetBestOffShoot(Position input)
            {
                bestOffShoot = input;
            }
            
            //Sets the evaluation of this position
            public void SetEvaluation(int eval)
            {
                evaluation = eval;
            }
            
            //Gives this position a reference to it's parent position
            public void SetParent(Position input)
            {
                parent = input;
            }
            
            //Gives this position a reference to the list of it's children
            public void SetChildren(List<Position> input)
            {
                children = input;
            }
        }
        
        //Takes in a position
        //Returns the Position that has the lowest evaluation out of that position's children
        public static Position minimizer(Position inPosition)
        {
            List<Position> branches = inPosition.GetChildren();
            Position lowest = branches[0];
            int lowestEval = int.MaxValue;
            for (int i = 0; i < branches.Count; i++)
            {
                int currentEval = branches[i].GetEvaluation();
                if (currentEval < lowestEval)
                {
                    lowest = branches[i];
                    lowestEval = currentEval;
                }
            }
            return lowest;
        }

        //Takes in a position
        //Returns the position that has the highest evaluation out of that position's children
        public static Position maximizer(Position inPosition)
        {
            List<Position> branches = inPosition.GetChildren();
            Position highest = branches[0];
            int highestEval = int.MinValue;
            for (int i = 0; i < branches.Count; i++)
            {
                int currentEval = branches[i].GetEvaluation();
                if (currentEval > highestEval)
                {
                    highest = branches[i];
                    highestEval = currentEval;
                }
            }
            return highest;
        }

        //Takes in List of Positions, the depth to iterate to, and the current player
        //Generates and returns all possible Positions that could occur after (depth) moves
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

        //Takes in a position and the current player's move
        //Returns every possible position that could occur after that move
        public static List<Position> newGeneration(Position inPosition, bool isX)
        {
            List<Position> toReturn = new List<Position>();
            if(inPosition.DidJustWin())
            {
                bool needsToBeAdded = true;
                for(int i = 0; i < inPosition.GetParent().GetChildren().Count; i++)
                {
                    if(inPosition == inPosition.GetParent().GetChildren()[i])
                    {
                        needsToBeAdded = false;
                    }
                }
                if(needsToBeAdded)
                {
                    inPosition.GetParent().GetChildren().Add(inPosition);
                }
                toReturn.Add(inPosition);
                return toReturn;
            }
            for (int i = 0; i < 7; i++)
            {
                if (inPosition.GetPlayableMoves()[i])
                {
                    Position toAdd = new Position(inPosition.ClonePosition(), inPosition.GetLastXMove(), inPosition.GetLastYMove(), !isX, inPosition.GetNumMovesPlayed(), inPosition.CloneMovesPlayed());
                    toAdd.TogglePosition(i, isX);
                    toAdd.SetParent(inPosition);
                    toReturn.Add(toAdd);
                }
            }
            inPosition.SetChildren(toReturn);
            return toReturn;
        }

        //Takes in a Position, the current player, and the amount of moves in the future to look 
        //Returns the best slot for the current player to play in
        public static int bestMoveInPosition(Position inPosition, bool isX, int depth)
        {
            //isActivePlayerAtEndOfTree
            List<Position> tree = fullNextGeneration(newGeneration(inPosition, isX), depth, !isX);
            bool[] possibilities = inPosition.GetPlayableMoves();
            List<Position> focusGen = tree;
            //Find Evaluations For Final Positions
            for(int i = 0; i < tree.Count; i++)
            {
                int eval = tree[i].MakeEvaluation();
            }
            //Assemble Parent Nodes
            for (int i = 0; i <= depth; i++)
            {
                HashSet<Position> parents = new HashSet<Position>();
                List<Position> toFocusGen = new List<Position>();
                for (int j = 0; j < focusGen.Count; j++)
                {
                    if(focusGen[j].GetParent() == null)
                    {
                        toFocusGen.Add(focusGen[j]);
                        continue;
                    }
                    if (parents.Add(focusGen[j].GetParent()))
                    {
                        toFocusGen.Add(focusGen[j].GetParent());
                    }
                }
                for (int j = 0; j < parents.Count; j++)
                {
                    Position chosenOffShoot = null;
                    if (toFocusGen[j].IsLastPlayerX())
                    {
                       chosenOffShoot = minimizer(toFocusGen[j]);
                    }
                    else
                    {
                        chosenOffShoot = maximizer(toFocusGen[j]);
                    }
                    toFocusGen[j].SetEvaluation(chosenOffShoot.GetEvaluation());
                    toFocusGen[j].SetBestOffShoot(chosenOffShoot);
                }
                focusGen = toFocusGen;
            }
            return inPosition.GetBestOffShoot().GetLastXMove();
        }
        
        //Runs the program
        public static void Main(string[] args)
        {
            Position truePosition = new Position();
            Position oldPosition = new Position();
            bool turn = true;
            while (!truePosition.IsWinningMove(truePosition.GetLastXMove(), truePosition.GetLastYMove(), truePosition.IsLastPlayerX()))
            {
                truePosition.PrintPosition();
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
                if (selectMode)
                {
                    truePosition.PrintPosition();
                    Console.WriteLine($"Dave Reccomends: {bestMoveInPosition(truePosition, turn, 5)}");
                    Console.WriteLine($"Evaluation: +{truePosition.GetEvaluation()}");
                    for (int i = 0; i < truePosition.GetChildren().Count; i++)
                    {
                        Console.WriteLine($"Move {truePosition.GetChildren()[i].GetLastXMove()} is worth {truePosition.GetChildren()[i].GetEvaluation()}");
                    }
                    truePosition.GetBestOffShoot().PrintPosition();
                    response = Console.ReadLine();
                }
                if (response == "/undo")
                {
                    truePosition = new Position(oldPosition.ClonePosition(), oldPosition.GetLastXMove(),
                                               oldPosition.GetLastYMove(), oldPosition.IsLastPlayerX(),
                                               oldPosition.GetNumMovesPlayed(), oldPosition.CloneMovesPlayed());
                }
                else if (response == "/explain")
                {
                    Console.WriteLine($"Dave Reccomends: {bestMoveInPosition(oldPosition, turn, 5)}");
                    Console.WriteLine($"Evaluation: +{oldPosition.GetEvaluation()}");
                }
                else
                {
                    int move = Int32.Parse(response);
                    oldPosition = new Position(truePosition.ClonePosition(), truePosition.GetLastXMove(),
                           truePosition.GetLastYMove(), truePosition.IsLastPlayerX(),
                           truePosition.GetNumMovesPlayed(), truePosition.CloneMovesPlayed());
                    truePosition.TogglePosition(move, turn);
                    turn = !turn;
                    if (selectMode && truePosition.IsWinningMove(truePosition.GetLastXMove(), truePosition.GetLastYMove(), truePosition.IsLastPlayerX()))
                    {
                        truePosition.PrintPosition();
                        Console.WriteLine($"Evaluation: {truePosition.MakeEvaluation()} Does The Engine Know The Game Is Over: {truePosition.DidJustWin()}");
                    }
                    if (!selectMode)
                    {
                        truePosition.PrintPosition();
                        if (truePosition.IsWinningMove(truePosition.GetLastXMove(), truePosition.GetLastYMove(), truePosition.IsLastPlayerX()))
                        {
                            Console.WriteLine("Game Over");
                            return;
                        }
                        if (turn == truePosition.IsLastPlayerX())
                        {
                            Console.WriteLine("This should never happen x1");
                        }
                        Console.WriteLine("The Bot Will Now Play Their Move");
                        move = bestMoveInPosition(truePosition, turn, 5);
                        Console.WriteLine(move);
                        truePosition.TogglePosition(move, turn);
                        turn = !turn;
                        if (truePosition.IsWinningMove(truePosition.GetLastXMove(), truePosition.GetLastYMove(), truePosition.IsLastPlayerX()))
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
}