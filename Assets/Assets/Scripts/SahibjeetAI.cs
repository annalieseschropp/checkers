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
                }
                else if (aiColour == CheckersMove.Turn.Black)
                {
                    if (curState[i, j] == CheckersState.State.White)
                        state += 1;
                    else if (curState[i, j] == CheckersState.State.WhiteKing)
                        state += 2;
                    else if (curState[i, j] == CheckersState.State.Black)
                        state -= 1;
                    else if (curState[i, j] == CheckersState.State.BlackKing)
                        state -= 2;
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

    private void calculateNewMoves(SahibjeetNode node, int depth)
    {
        if (depth == 0)
            return;
        
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (aiColour == CheckersMove.Turn.Black && (node.curState[i, j] == CheckersState.State.Black || node.curState[i, j] == CheckersState.State.BlackKing))
                {
                    CheckersMove.Square square = new CheckersMove.Square();
                    square.x = i;
                    square.y = j;
                    //List<CheckersMove.Move> moves = new List<CheckersMove.Move>();
                    List<CheckersMove.Move> temp = controller.SelectPiece(square);
                    
                    // Place all the possible moves as a child of the new node and recursively go down the tree to generate all possible game states.
                    foreach (CheckersMove.Move move in temp)
                    {
                        SahibjeetNode tempOne = new SahibjeetNode();
                        tempOne.curState = (CheckersState.State[,])node.curState.Clone();
                        tempOne.moveController = new MoveController(ref tempOne.curState, NameStaticClass.forcedMove);
                        CheckersState.State tempState = tempOne.curState[move.src.x, move.src.y];
                        tempOne.curState[move.src.x, move.src.y] = CheckersState.State.Empty;
                        tempOne.curState[move.dest.x, move.dest.y] = tempState;
                        node.AddChild(tempOne);
                        calculateNewMoves(tempOne, depth - 1);
                    }
                }
            }
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        calculateAIMoves();
    }

    public void calculateAIMoves()
    {
        rootNode.curState = board.curState;
        rootNode.moveController = new MoveController(ref rootNode.curState, NameStaticClass.forcedMove);
        calculateNewMoves(rootNode, 3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
