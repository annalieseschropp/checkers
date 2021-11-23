using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SahibjeetNode
{
    public CheckersState.State[,] curState = new CheckersState.State[8, 8];
    public SahibjeetNode parent = null;
    public List<SahibjeetNode> children = new List<SahibjeetNode>();
    public int stateEvaluation;
    public MoveController moveController;
    public CheckersMove.Move storedMove = new CheckersMove.Move();
    public CheckersMove.Turn moveTurn;

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

    /// <summary>
    /// Method
    /// Used to evaluate the state of the current board represented by the board,
    /// Currently just uses the material movement.
    /// </summary>
    public void EvaluateState(CheckersMove.Turn aiColour)
    {
        int state = 0;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (aiColour == CheckersMove.Turn.Black)
                {
                    if (curState[i, j] == CheckersState.State.Black)
                        state += 1;
                    else if (curState[i, j] == CheckersState.State.BlackKing)
                        state += 2;
                    else if (curState[i, j] == CheckersState.State.White)
                        state -= 1;
                    else if (curState[i, j] == CheckersState.State.WhiteKing)
                        state -= 2;
                    if (2 < i && i < 6)
                    {
                        if (2 < j && j < 6)
                        {
                            if (curState[i, j] == CheckersState.State.Black)
                                state += 2;
                            else if (curState[i, j] == CheckersState.State.BlackKing)
                                state += 3;
                        }
                    }
                }
                else if (aiColour == CheckersMove.Turn.White)
                {
                    if (curState[i, j] == CheckersState.State.White)
                        state += 1;
                    else if (curState[i, j] == CheckersState.State.WhiteKing)
                        state += 2;
                    else if (curState[i, j] == CheckersState.State.Black)
                        state -= 1;
                    else if (curState[i, j] == CheckersState.State.BlackKing)
                        state -= 2;
                    if (2 < i && i < 6)
                    {
                        if (2 < j && j < 6)
                        {
                            if (curState[i, j] == CheckersState.State.White)
                                state += 2;
                            else if (curState[i, j] == CheckersState.State.WhiteKing)
                                state += 3;
                        }
                    }
                }
                
            }
        }
        stateEvaluation = state;
    }
}

public class SahibjeetAI : MonoBehaviour
{
    public Board board;
    private SahibjeetNode rootNode = new SahibjeetNode();
    private CheckersMove.Turn aiColour = CheckersMove.Turn.Black;
    private MoveController controller;
    private SahibjeetNode currentBestMove;

    void Start()
    {
        board = board.GetComponent<Board>();
        Debug.Log(board.ToString());
        Debug.Log(board);
    }

    private void calculateNewMoves(SahibjeetNode node, CheckersMove.Turn currentTurn, int depth)
    {
        if (depth == 0)
            return;
        
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (currentTurn == CheckersMove.Turn.Black && (node.curState[i, j] == CheckersState.State.Black || node.curState[i, j] == CheckersState.State.BlackKing))
                {
                    CheckersMove.Square square = new CheckersMove.Square();
                    square.x = i;
                    square.y = j;
                    List<CheckersMove.Move> temp = node.moveController.SelectPiece(square);
                    // If there is no possible moves there is no point in continuing the tree.
                    if (temp.Count == 0)
                        return;
                    // Place all the possible moves as a child of the new node and recursively go down the tree to generate all possible game states.
                    foreach (CheckersMove.Move move in temp)
                    {
                        SahibjeetNode tempOne = new SahibjeetNode();

                        tempOne.curState = (CheckersState.State[,])node.curState.Clone();
                        tempOne.moveController = new MoveController(ref tempOne.curState, NameStaticClass.forcedMove);
                        
                        CheckersState.State tempState = tempOne.curState[move.src.x, move.src.y];
                        
                        // Calculate if the piece jumps over a piece,
                        // if it does remove that piece from the board
                        int moveDistance = Mathf.RoundToInt(Mathf.Sqrt(((move.src.x + move.dest.x) ^ 2) + ((move.src.y + move.dest.y) ^ 2)));
                        if (moveDistance > 1)
                        {
                            CheckersMove.Square removedElement = move.src + move.dest;
                            removedElement /= 2;
                            tempOne.curState[removedElement.x, removedElement.y] = CheckersState.State.Empty;
                        }
                        tempOne.curState[move.src.x, move.src.y] = CheckersState.State.Empty;
                        tempOne.curState[move.dest.x, move.dest.y] = tempState;
                        
                        tempOne.storedMove.src = move.src;
                        tempOne.storedMove.dest = move.dest;
                        tempOne.moveTurn = currentTurn;
                        
                        node.AddChild(tempOne);
                        
                        calculateNewMoves(tempOne, CheckersMove.Turn.White, depth - 1);
                    }
                }
                else if (currentTurn == CheckersMove.Turn.White && (node.curState[i, j] == CheckersState.State.White || node.curState[i, j] == CheckersState.State.WhiteKing))
                {
                    CheckersMove.Square square = new CheckersMove.Square();
                    square.x = i;
                    square.y = j;
                    List<CheckersMove.Move> temp = node.moveController.SelectPiece(square);
                    if (temp.Count == 0)
                        return;
                    
                    foreach (CheckersMove.Move move in temp)
                    {
                        SahibjeetNode tempOne = new SahibjeetNode();
                        
                        tempOne.curState = (CheckersState.State[,])node.curState.Clone();
                        tempOne.moveController = new MoveController(ref tempOne.curState, NameStaticClass.forcedMove);
                        
                        CheckersState.State tempState = tempOne.curState[move.src.x, move.src.y];
                        
                        // Calculate if the piece jumps over a piece,
                        // if it does remove that piece from the board
                        int moveDistance = Mathf.RoundToInt(Mathf.Sqrt(((move.src.x + move.dest.x) ^ 2) + ((move.src.y + move.dest.y) ^ 2)));
                        if (moveDistance > 1)
                        {
                            CheckersMove.Square removedElement = move.src + move.dest;
                            removedElement /= 2;
                            tempOne.curState[removedElement.x, removedElement.y] = CheckersState.State.Empty;
                        }
                        tempOne.curState[move.src.x, move.src.y] = CheckersState.State.Empty;
                        tempOne.curState[move.dest.x, move.dest.y] = tempState;
                        
                        tempOne.curState[move.src.x, move.src.y] = CheckersState.State.Empty;
                        tempOne.curState[move.dest.x, move.dest.y] = tempState;

                        tempOne.storedMove.src = move.src;
                        tempOne.storedMove.dest = move.dest;
                        tempOne.moveTurn = currentTurn;

                        node.AddChild(tempOne);

                        calculateNewMoves(tempOne, CheckersMove.Turn.Black, depth - 1);
                    }
                }
            }
        }
    }

    // Function to create the AI search tree for the movement.
    public CheckersMove.Move calculateAIMoves()
    {
        // Debug.Log(rootNode);
        // Debug.Log(board);
        // Debug.Log(rootNode.curState);
        // Debug.Log(board.curState);
        // rootNode.curState = (CheckersState.State[,])board.curState.Clone();
        // rootNode.moveController = new MoveController(ref rootNode.curState, NameStaticClass.forcedMove);
        // // Create the move tree with all movements
        // calculateNewMoves(rootNode, aiColour, 3);
        // evaluateAllBoardStates(rootNode);
        // SahibjeetNode bestMove = getBestMove(rootNode);
        // return bestMove.storedMove;
        return new CheckersMove.Move(2, 2, 3, 3);
    }

    public void destroyTree()
    {
        rootNode = null;
    }

    private void evaluateAllBoardStates(SahibjeetNode node)
    {
        foreach (SahibjeetNode child in node.children)
        {
            child.EvaluateState(aiColour);
            evaluateAllBoardStates(child);
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
        // for (int i = 0; i < element.Count; i++)
        // {
        //     if (element[i] > element[currentIndex])
        //         currentIndex = i;
        // }

        return root.children[0];
    }

    private int searchNodes(SahibjeetNode node, int finalAnswer)
    {
        if (node.children.Count == 0)
            return finalAnswer + node.stateEvaluation;

        finalAnswer += node.stateEvaluation;
        foreach(SahibjeetNode child in node.children)
        {
            finalAnswer = Mathf.Max(finalAnswer, searchNodes(child, finalAnswer));
        }
        return finalAnswer;
    }

    // Function to search the tree for the best move to make
    // private SahibjeetNode alphaBetaPrune(SahibjeetNode node, int depth, int alpha, int beta, CheckersMove.Turn maximizingPlayer)
    // {
        
    // }
}
