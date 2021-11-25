using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SahibjeetNode
{
    public CheckersState.State[,] curState = new CheckersState.State[8, 8];
    public SahibjeetNode parent = null;
    public List<SahibjeetNode> children = new List<SahibjeetNode>();
    public int stateEvaluation;
    public CheckersMove.Move storedMove = new CheckersMove.Move();
    public CheckersMove.Turn moveTurn;
    private int numberOfBlackPieces = 0;
    private int numberOfBlackKings = 0;
    private int numberOfWhitePieces = 0;
    private int numberOfWhiteKings = 0;
    public SahibjeetNode()
    {
        curState = new CheckersState.State[8, 8];
    }

    public void AddChild(SahibjeetNode newNode)
    {
        newNode.parent = this;
        children.Add(newNode);
    }

    public bool IsRoot()
    {
        if (parent == null)
            return true;
        return false;
    }

    public int Depth()
    {
        int deepest = 0;
        foreach(SahibjeetNode child in children)
        {
            int depth = 1 + child.Depth();
            if (deepest < depth)
                deepest = depth;
        }
        return deepest;
    }

    public void countPieces()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (curState[i, j] == CheckersState.State.Black)
                    numberOfBlackPieces++;
                else if (curState[i, j] == CheckersState.State.White)
                    numberOfWhitePieces++;
                else if (curState[i, j] == CheckersState.State.BlackKing)
                    numberOfBlackKings++;
                else if (curState[i, j] ==CheckersState.State.WhiteKing)
                    numberOfWhiteKings++;
            }
        }
        foreach (SahibjeetNode child in children)
        {
            child.countPieces();
        }
    }

    /// <summary>
    /// Method
    /// Used to evaluate the state of the current board represented by the board,
    /// Currently just uses the material movement.
    /// </summary>
    public void EvaluateState(CheckersMove.Turn aiColour)
    {
        int state = 0;
        if (aiColour == CheckersMove.Turn.Black)
        {
            state = numberOfBlackPieces + numberOfBlackKings - numberOfWhitePieces - numberOfWhiteKings;
            if (parent.numberOfWhitePieces > this.numberOfWhitePieces)
                state *= 2;
            if (parent.numberOfWhiteKings > this.numberOfWhiteKings)
                state *= 3;
        }
        else if (aiColour == CheckersMove.Turn.White)
        {
            state = numberOfWhitePieces + numberOfWhiteKings - numberOfBlackPieces - numberOfBlackKings;
            if (parent.numberOfBlackPieces > this.numberOfBlackPieces)
                state *= 2;
            if (parent.numberOfBlackKings > this.numberOfBlackKings)
                state *= 3;    
        }
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                int xVar = i + 1;
                int xVarM = j - 1;
                int yVar = j + 1;
                int yVarM = j - 1;
                        
                if (aiColour == CheckersMove.Turn.Black)
                {
                    if (curState[i, j] == CheckersState.State.Black)
                    {
                        if (xVar != 8 && yVar != 8)
                            if (curState[xVar, yVar] == CheckersState.State.White || curState[xVar, yVar] == CheckersState.State.WhiteKing)
                                state += 1;
                        if (xVar != 8 && yVarM != -1)
                            if (curState[xVar, yVarM] == CheckersState.State.White || curState[xVar, yVarM] == CheckersState.State.WhiteKing)
                                state += 1;
                    }
                    if (curState[i, j] == CheckersState.State.BlackKing)
                    {
                        if (xVar != 8 && yVar != 8)
                            if (curState[xVar, yVar] == CheckersState.State.White || curState[xVar, yVar] == CheckersState.State.WhiteKing)
                                state += 1;
                        if (xVarM != -1 && yVar != 8)
                            if (curState[xVarM, yVar] == CheckersState.State.White || curState[xVarM, yVar] == CheckersState.State.WhiteKing)
                                state += 1;
                        if (xVar != 8 && yVarM != -1)
                            if (curState[xVar, yVarM] == CheckersState.State.White || curState[xVar, yVarM] == CheckersState.State.WhiteKing)
                                state += 1;
                        if (xVarM != -1 && yVarM != -1)
                            if (curState[xVarM, yVarM] == CheckersState.State.White || curState[xVarM, yVarM] == CheckersState.State.WhiteKing)
                                state += 1;
                    }
                    if (2 < i && i < 6)
                    {
                        if (2 < j && j < 6)
                        {
                            if (curState[i, j] == CheckersState.State.Black)
                                state += 1;
                            else if (curState[i, j] == CheckersState.State.BlackKing)
                                state += 1;
                        }
                    }
                }
                else if (aiColour == CheckersMove.Turn.White)
                {
                    if (curState[i, j] == CheckersState.State.White)
                    {
                        if (xVar != 8 && yVar != 8)
                            if (curState[xVar, yVar] == CheckersState.State.Black || curState[xVar, yVar] == CheckersState.State.BlackKing)
                                state += 1;
                        if (xVar != 8 && yVarM != -1)
                            if (curState[xVar, yVarM] == CheckersState.State.Black || curState[xVar, yVarM] == CheckersState.State.BlackKing)
                                state += 1;
                    }
                    if (curState[i, j] == CheckersState.State.WhiteKing)
                    {
                        if (xVar != 8 && yVar != 8)
                            if (curState[xVar, yVar] == CheckersState.State.Black || curState[xVar, yVar] == CheckersState.State.BlackKing)
                                state += 1;
                        if (xVarM != -1 && yVar != 8)
                            if (curState[xVarM, yVar] == CheckersState.State.Black || curState[xVarM, yVar] == CheckersState.State.BlackKing)
                                state += 1;
                        if (xVar != 8 && yVarM != -1)
                            if (curState[xVar, yVarM] == CheckersState.State.Black || curState[xVar, yVarM] == CheckersState.State.BlackKing)
                                state += 1;
                        if (xVarM != -1 && yVarM != -1)
                            if (curState[xVarM, yVarM] == CheckersState.State.Black || curState[xVarM, yVarM] == CheckersState.State.BlackKing)
                                state += 1;
                    }
                    if (2 < i && i < 6)
                    {
                        if (2 < j && j < 6)
                        {
                            if (curState[i, j] == CheckersState.State.White)
                                state += 1;
                            else if (curState[i, j] == CheckersState.State.WhiteKing)
                                state += 1;
                        }
                    }
                }
            }
        }
        Debug.Log(storedMove.src + ":" + storedMove.dest + " = " + state);
        stateEvaluation = state;
    }
}

public class SahibjeetAI : AIPlayer
{
    private SahibjeetNode rootNode = new SahibjeetNode();
    private CheckersMove.Turn aiColour = CheckersMove.Turn.White;
    private MoveController controller;
    private SahibjeetNode currentBestMove;

    private void calculateNewMoves(SahibjeetNode node, CheckersMove.Turn currentTurn, bool forcedMove, int depth)
    {
        if (depth == 0)
            return;
        
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Debug.Log(i + " , " + j);
                if (currentTurn == CheckersMove.Turn.Black && (node.curState[i, j] == CheckersState.State.Black || node.curState[i, j] == CheckersState.State.BlackKing))
                {
                    CheckersMove.Square square = new CheckersMove.Square();
                    square.x = i;
                    square.y = j;
                    // List<CheckersMove.Move> temp = node.moveController.SelectPiece(square);
                    List<CheckersMove.Move> temp = LegalMoveGenerator.GetLegalMoves(square, node.curState, currentTurn, forcedMove);
                    
                    // If there is no possible moves there is no point in continuing the tree.
                    // if (temp.Count == 0)
                    //     break;
                    // Place all the possible moves as a child of the new node and recursively go down the tree to generate all possible game states.
                    foreach (CheckersMove.Move move in temp)
                    {
                        SahibjeetNode tempOne = new SahibjeetNode();

                        tempOne.curState = (CheckersState.State[,])node.curState.Clone();
                        LegalMoveGenerator.MakeLegalMove(move, ref tempOne.curState, currentTurn);
                        
                        tempOne.storedMove.src = move.src;
                        tempOne.storedMove.dest = move.dest;
                        tempOne.moveTurn = currentTurn;
                        
                        node.AddChild(tempOne);
                        
                        calculateNewMoves(tempOne, CheckersMove.Turn.White, forcedMove, depth - 1);
                    }
                }
                else if (currentTurn == CheckersMove.Turn.White && (node.curState[i, j] == CheckersState.State.White || node.curState[i, j] == CheckersState.State.WhiteKing))
                {
                    CheckersMove.Square square = new CheckersMove.Square();
                    square.x = i;
                    square.y = j;
                    //List<CheckersMove.Move> temp = node.moveController.SelectPiece(square);
                    List<CheckersMove.Move> temp = LegalMoveGenerator.GetLegalMoves(square, node.curState, currentTurn, forcedMove);
                    
                    // if (temp.Count == 0)
                    //     break;
                    
                    foreach (CheckersMove.Move move in temp)
                    {
                        SahibjeetNode tempOne = new SahibjeetNode();
                        
                        tempOne.curState = (CheckersState.State[,])node.curState.Clone();
                        LegalMoveGenerator.MakeLegalMove(move, ref tempOne.curState, currentTurn);

                        tempOne.storedMove.src = move.src;
                        tempOne.storedMove.dest = move.dest;
                        tempOne.moveTurn = currentTurn;

                        node.AddChild(tempOne);

                        calculateNewMoves(tempOne, CheckersMove.Turn.Black, forcedMove, depth - 1);
                    }
                }
            }
        }
    }

    // Function to create the AI search tree for the movement.
    override public CheckersMove.Move? GetAIMove(CheckersState.State[,] boardState, CheckersMove.Turn currentTurn, bool forceCapture, CheckersMove.Square? multicaptureSquare = null)
    {
        rootNode.curState = boardState;
        calculateNewMoves(rootNode, currentTurn, forceCapture, 3);
        countAllPieces();
        evaluateAllBoardStates(rootNode, currentTurn);
        SahibjeetNode bestMove = getBestMove(rootNode);
        destroyTree();
        Debug.Log("Best Move = " + bestMove.storedMove.src + ":" + bestMove.storedMove.dest + " = " + bestMove.stateEvaluation);
        return bestMove.storedMove;
    }

    private void countAllPieces()
    {
        rootNode.countPieces();
    }

    public void destroyTree()
    {
        rootNode = null;
        rootNode = new SahibjeetNode();
    }

    private void evaluateAllBoardStates(SahibjeetNode node, CheckersMove.Turn currentTurn)
    {
        foreach (SahibjeetNode child in node.children)
        {
            child.EvaluateState(currentTurn);
            evaluateAllBoardStates(child, currentTurn);
            // if (currentTurn == CheckersMove.Turn.Black)
            //     evaluateAllBoardStates(child, CheckersMove.Turn.White);
            // else
            //     evaluateAllBoardStates(child, CheckersMove.Turn.Black);
        }
    }

    private SahibjeetNode getBestMove(SahibjeetNode root)
    {
        List<int> element = new List<int>();
        foreach(SahibjeetNode child in root.children)
        {
            int final = child.stateEvaluation;
            final = searchNodes(child, final);
            element.Add(final);
        }
        
        int currentIndex = 0;
        
        // Find the highest integer value in the list of integer.
        for (int i = 0; i < element.Count; i++)
        {
            if (element[i] > element[currentIndex])
                currentIndex = i;
        }

        return root.children[currentIndex];
    }

    private int searchNodes(SahibjeetNode node, int finalAnswer)
    {
        if (node.children.Count == 0)
            return finalAnswer + node.stateEvaluation;

        finalAnswer += node.stateEvaluation;
        foreach(SahibjeetNode child in node.children)
        {
            // finalAnswer = Mathf.Max(finalAnswer, searchNodes(child, finalAnswer));
            finalAnswer = searchNodes(child, finalAnswer);
        }
        return finalAnswer;
    }

    // Function to search the tree for the best move to make
    // private SahibjeetNode alphaBetaPrune(SahibjeetNode node, int depth, int alpha, int beta, CheckersMove.Turn maximizingPlayer)
    // {
        
    // }
}
