using System;
using System.Collections.Generic;
public class MainClass
{

    /// <summary>
    /// Main Runs the program
    /// </summary>
    /// <param name="args"></param>
    public static void Main(string[] args)
    {
        Position truePosition = new Position();
        Position oldPosition = new Position();
        bool turn = true;
        while (!truePosition.IsWinningMove(truePosition.GetLastXMove(), 
                truePosition.GetLastYMove(), truePosition.IsLastPlayerX()))
        {
            truePosition.PrintPosition();
            Console.WriteLine("Play a Move");
            string response = Console.ReadLine();
            bool selectMode = false;
            if (response == "/setup")
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
                    Console.WriteLine($"Move {truePosition.GetChildren()[i].GetLastXMove()} is worth " +
                                      $"{truePosition.GetChildren()[i].GetEvaluation()}");
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
                if (selectMode && truePosition.IsWinningMove(truePosition.GetLastXMove(), 
                    truePosition.GetLastYMove(), truePosition.IsLastPlayerX()))
                {
                    truePosition.PrintPosition();
                    Console.WriteLine($"Evaluation: {truePosition.MakeEvaluation()} " +
                                      $"Does The Engine Know The Game Is Over: " +
                                      $"{truePosition.DidJustWin()}");
                }
                if (!selectMode)
                {
                    truePosition.PrintPosition();
                    if (truePosition.IsWinningMove(truePosition.GetLastXMove(), 
                        truePosition.GetLastYMove(), truePosition.IsLastPlayerX()))
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
                    if (truePosition.IsWinningMove(truePosition.GetLastXMove(), 
                        truePosition.GetLastYMove(), truePosition.IsLastPlayerX()))
                    {
                        truePosition.PrintPosition();
                        Console.WriteLine("Game Over");
                        return;
                    }
                }
            }
        }
    }
    /// <summary>
    /// Finds the child of the Position with the lowest rating and returns that Position
    /// </summary>
    /// <param name="inPosition"> Position - The Position for which to find the worst Child </param>
    /// <returns> Position - The Child of this Position with the lowest evaluation </returns>
    private static Position minimizer(Position inPosition)
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

    /// <summary>
    /// Finds the child of the Position with the highest rating and returns that Position
    /// </summary>
    /// <param name="inPosition"> Position - The Position for which to find the best Child </param>
    /// <returns> Position - The Child of this Position with the highest evaluation </returns>
    private static Position maximizer(Position inPosition)
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

    /// <summary>
    /// Creates a List of Positions that represents all possible offshoots [depth] moves from each 
    /// Position in the tree
    /// </summary>
    /// <param name="tree"> List<Position> - The Positions for which to find all possible 
    /// offshoots of</param>
    /// <param name="depth"> int - The number of steps to look in the future when finding 
    /// Positions</param>
    /// <param name="isX"> bool - Whether the active player is X at the inputted generation of 
    /// Positions</param>
    /// <returns> List<Position> - All possible Positions in [depth] moves </returns>
    private static List<Position> fullNextGeneration(List<Position> tree, int depth, bool isX)
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

    /// <summary>
    /// Creates a List of Positions that represents all possible offshoots after the next move
    /// </summary>
    /// <param name="inPosition"> Position - The Position to find the children of</param>
    /// <param name="isX"> bool - Whether the active player at the inputted Position is X</param>
    /// <returns> List<Positions> - All Possible positions after the next move </returns>
    private static List<Position> newGeneration(Position inPosition, bool isX)
    {
        List<Position> toReturn = new List<Position>();
        if (inPosition.DidJustWin())
        {
            bool needsToBeAdded = true;
            for (int i = 0; i < inPosition.GetParent().GetChildren().Count; i++)
            {
                if (inPosition == inPosition.GetParent().GetChildren()[i])
                {
                    needsToBeAdded = false;
                }
            }
            if (needsToBeAdded)
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
                Position toAdd = new Position(inPosition.ClonePosition(), 
                                              inPosition.GetLastXMove(), inPosition.GetLastYMove(),
                                              !isX, inPosition.GetNumMovesPlayed(), 
                                              inPosition.CloneMovesPlayed());
                toAdd.TogglePosition(i, isX);
                toAdd.SetParent(inPosition);
                toReturn.Add(toAdd);
            }
        }
        inPosition.SetChildren(toReturn);
        return toReturn;
    }

    /// <summary>
    /// Finds the best move for the active player to play
    /// </summary>
    /// <param name="inPosition"> Position - The Position for which to find the best move</param>
    /// <param name="isX"> bool - Whether the active player at the inputted position is X</param>
    /// <param name="depth"> int - The number of moves in the future to look</param>
    /// <returns>int - The best move in the position</returns>
    private static int bestMoveInPosition(Position inPosition, bool isX, int depth)
    {
        List<Position> tree = fullNextGeneration(newGeneration(inPosition, isX), depth, !isX);
        bool[] possibilities = inPosition.GetPlayableMoves();
        List<Position> focusGen = tree;
        //Find Evaluations For Final Positions
        for (int i = 0; i < tree.Count; i++)
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
                if (focusGen[j].GetParent() == null)
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

}