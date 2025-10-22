using System;
using System.Collections.Generic;

/// <summary>
/// Position represents a Board of Connect Four
/// </summary>
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

    /// <summary>
    /// The default Position constructor: Will make a new game
    /// </summary>
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

    /// <summary>
    /// Will make a Board thats grid is the same as the grid provided
    /// </summary>
    /// <param name="toGrid">char[,] - The grid for which to copy</param>
    public Position(char[,] toGrid)
    {
        grid = toGrid;
    }

    /// <summary>
    /// Creates a new Position with all data provided
    /// </summary>
    /// <param name="toGrid"> char[,] - The current Board of pieces</param>
    /// <param name="_lastPlayedMoveX">int - The last move the X player made</param>
    /// <param name="_lastPlayedMoveY">int - The last move the Y player made</param>
    /// <param name="_lastPlayerIsX">bool - Whether the last Player to move was X</param>
    /// <param name="_numMovesPlayed">int - The number of moves that have been played</param>
    /// <param name="_movesPlayed">List<int> - The moves that have been played, in order</param>
    public Position(char[,] toGrid, int _lastPlayedMoveX, int _lastPlayedMoveY, 
                    bool _lastPlayerIsX, int _numMovesPlayed, List<int> _movesPlayed)
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

    /// <summary>
    /// Finds which columns are possible to play in
    /// </summary>
    /// <returns>bool[] - True/False for each column whether it can be played in</returns>
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

    /// <summary>
    /// Prints an ASCII representation of the current Position in the console
    /// </summary>
    public void PrintPosition()
    {
        for (int i = 0; i < grid.GetLength(1); i++)
        {
            for (int j = 0; j < grid.GetLength(0); j++)
            {
                if (j == lastPlayedMoveX && i == lastPlayedMoveY)
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

    /// <summary>
    /// Finds if a move that could be played is the winning move
    /// </summary>
    /// <param name="xPos">int - the column of the potentially winning move</param>
    /// <param name="yPos">int - the row of the potentially winning move</param>
    /// <param name="isX">bool - whether the move is played by X</param>
    /// <returns>bool - Whether the move would result in a win</returns>
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

    /// <summary>
    /// Finds the number of connections that any given piece has
    /// </summary>
    /// <param name="xPos">int - The column of the piece in question</param>
    /// <param name="yPos">int - The row of the piece in question</param>
    /// <param name="isX">bool - Whether the piece belongs to X</param>
    /// <returns>int - The number of connections</returns>
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

    /// <summary>
    /// Finds the total number of connections that a player has
    /// </summary>
    /// <param name="isX">bool - whether the player in question is X</param>
    /// <returns>int - The total number of connections a player has</returns>
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
                if (grid[i, j] == charChecker)
                {
                    toReturn += numConnections(i, j, isX);
                }
            }
        }
        return toReturn;
    }

    /// <summary>
    /// Finds whether a possible move would be the winning move
    /// </summary>
    /// <param name="xPos">int - the column of the move</param>
    /// <param name="isX">bool - Whether the move would be played by X</param>
    /// <returns></returns>
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

    /// <summary>
    /// Finds if the last move won the game
    /// </summary>
    /// <returns>bool - Whether the last move won the game</returns>
    public bool DidJustWin()
    {
        return IsWinningMove(lastPlayedMoveX, lastPlayedMoveY, lastPlayerIsX);
    }

    /// <summary>
    /// Adds the given move to the board
    /// </summary>
    /// <param name="slot">int - The column which the move is played in</param>
    /// <param name="isX">bool - Whether the move was played by X</param>
    /// <returns></returns>
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

    /// <summary>
    /// Creates a deep copy of the current board
    /// </summary>
    /// <returns>char[,] - The copied board</returns>
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

    /// <summary>
    /// Creates a deep copy of the move transcript
    /// </summary>
    /// <returns>List<int> - The copy of the move transcript</returns>
    public List<int> CloneMovesPlayed()
    {
        List<int> toReturn = new List<int>();
        for (int i = 0; i < movesPlayed.Count; i++)
        {
            toReturn.Add(movesPlayed[i]);
        }
        return toReturn;
    }

    /// <summary>
    /// Finds whether the active player can win on the next move
    /// </summary>
    /// <returns>bool - Whether the active player can win on the next move</returns>
    public bool CanWinOnNextMove()
    {
        for (int i = 0; i < GetPlayableMoves().Length; i++)
        {
            Position tempPosition = new Position(ClonePosition(), lastPlayedMoveX, lastPlayedMoveY,
                                                lastPlayerIsX, numMovesPlayed, CloneMovesPlayed());
            if (tempPosition.GetPlayableMoves()[i])
            {
                tempPosition.TogglePosition(i, !lastPlayerIsX);
                if (tempPosition.DidJustWin())
                {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Creates an evaluation for the current board, if X is winning, the numbers are 
    /// positive and vice versa
    /// </summary>
    /// <returns>int - The evaluation of the position</returns>
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
            if ((grid[currentX + 2, currentY + 1] == 'X' && lastPlayerIsX) || 
                (grid[currentX + 2, currentY + 1] == 'O' && !lastPlayerIsX))
            {
                toReturn += (3 * modifier);
            }
            if ((grid[currentX + 2, currentY + 1] == 'X' && !lastPlayerIsX) || 
                (grid[currentX + 2, currentY + 1] == 'O' && lastPlayerIsX))
            {
                toReturn -= (3 * modifier);
            }
        }
        evaluation = toReturn;
        return toReturn;
    }

    /// <summary>
    /// Gets the evaluation of the position (Will return 0 if evaluation is uncalculated)
    /// </summary>
    /// <returns>int - The evaluation</returns>
    public int GetEvaluation()
    {
        return evaluation;
    }

    /// <summary>
    /// Gets whether the last player was X
    /// </summary>
    /// <returns>bool - Whether the last player was X</returns>
    public bool IsLastPlayerX()
    {
        return lastPlayerIsX;
    }

    /// <summary>
    /// Gets this Position's parent
    /// </summary>
    /// <returns>Position - The Parent of the Position</returns>
    public Position GetParent()
    {
        return parent;
    }

    /// <summary>
    /// Gets the Best Child of the Position (Will return null if uncalculated)
    /// </summary>
    /// <returns>Position - The Child with the Best Evaluation</returns>
    public Position GetBestOffShoot()
    {
        return bestOffShoot;
    }

    /// <summary>
    /// Gets the Children of the Position
    /// </summary>
    /// <returns>List<Position> - The Children of this Position</returns>
    public List<Position> GetChildren()
    {
        return children;
    }

    /// <summary>
    /// Gets the Column of the Last Move
    /// </summary>
    /// <returns>int - Column</returns>
    public int GetLastXMove()
    {
        return lastPlayedMoveX;
    }

    /// <summary>
    /// Gets the Row of the Last Move
    /// </summary>
    /// <returns>int - Row</returns>
    public int GetLastYMove()
    {
        return lastPlayedMoveY;
    }

    /// <summary>
    /// Gets the number of moves played
    /// </summary>
    /// <returns>int - Number of moves played</returns>
    public int GetNumMovesPlayed()
    {
        return numMovesPlayed;
    }

    /// <summary>
    /// Assigns the Position that is the best Child of this position
    /// </summary>
    /// <param name="input">Position - The best Child</param>
    public void SetBestOffShoot(Position input)
    {
        bestOffShoot = input;
    }

    /// <summary>
    /// Sets the evaluation of the Position
    /// </summary>
    /// <param name="eval">int - The position's new evaluation</param>
    public void SetEvaluation(int eval)
    {
        evaluation = eval;
    }

    /// <summary>
    /// Sets the reference to the Parent Position
    /// </summary>
    /// <param name="input">Position - The Parent Position</param>
    public void SetParent(Position input)
    {
        parent = input;
    }

    /// <summary>
    /// Sets the reference to the Children of this Position
    /// </summary>
    /// <param name="input">List<Position> - The Children of this Position</param>
    public void SetChildren(List<Position> input)
    {
        children = input;
    }
}