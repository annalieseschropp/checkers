using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CheckersState;

/// <summary>
/// Class
/// Visual representation of a set of related 2D checker pieces.
/// </summary>
public class PieceSet : MonoBehaviour
{
    public GameObject whitePiecePrefab;
    public GameObject blackPiecePrefab;
    public GameObject whiteKingPrefab;
    public GameObject blackKingPrefab;

    private Piece[,] pieces;
    private GameObject pieceSetObject;

    /// <summary>
    /// Method
    /// Initializer performed before any script attempts to access the PieceSet.
    /// </summary>
    void Awake()
    {
        pieces = new Piece[8,8];
        pieceSetObject = Instantiate(new GameObject(), new Vector2(0,0), Quaternion.identity) as GameObject;
    }

    /// <summary>
    /// Method
    /// Visually updates the locations and types of the pieces based on the board state and move provided.
    /// </summary>
    public void MakeMove(CheckersState.State[,] boardState, Vector2 start, Vector2 end)
    {
        int endX = Mathf.RoundToInt(end.x);
        int endY = Mathf.RoundToInt(end.y);
        int startX = Mathf.RoundToInt(start.x);
        int startY = Mathf.RoundToInt(start.y);
        // Delete the new piece
        DestroyPiece(endX, endY);
        // make the current null and the next the new piece
        Piece toMove = this.pieces[startX, startY];
        this.pieces[startX, startY] = null;
        this.pieces[endX, endY] = toMove;
        // call the move function
        this.PiecesFromState(boardState);
        toMove.MovePiece(end);
    }

    /// <summary>
    /// Method
    /// Public interface for setting the initial positions of the pieces.
    /// </summary>
    public void SetInitialBoardState(CheckersState.State[,] boardState)
    {
        this.PiecesFromState(boardState);
    }

    /// <summary>
    /// Method
    /// Translates a board state into a visual set of pieces.
    /// </summary>
    private void PiecesFromState(CheckersState.State[,] boardState)
    {
        for (int i = 0; i < boardState.GetLength(1); i++)
        {
            for (int j = 0; j < boardState.GetLength(0); j++)
            {
                if(boardState[i,j] == 0)
                {
                    this.DestroyPiece(i,j);
                    continue;
                }
                else if (pieces[i,j] == null)
                {
                    this.CreatePiece(i,j, boardState[i,j]);
                }
                else
                {
                    this.UpdatePiece(i, j, boardState[i,j]);
                }
            }
        }
    }

    /// <summary>
    /// Method
    /// Helper method to create a piece.
    /// </summary>
    private void CreatePiece(int row, int column, CheckersState.State type)
    {
        Piece pieceComponent = pieceSetObject.AddComponent<Piece>();
        pieceComponent.InitializePiece(type, new Vector2(row,column), whitePiecePrefab, blackPiecePrefab, whiteKingPrefab, blackKingPrefab);
        pieces[row,column] = pieceComponent;
    }

    /// <summary>
    /// Method
    /// Helper method to update a piece.
    /// </summary>
    private void UpdatePiece(int row, int column, CheckersState.State type)
    {
        Piece toUpdate = pieces[row,column];
        toUpdate.SetPieceType(type);
    }

    /// <summary>
    /// Method
    /// Helper method to destroy a piece.
    /// </summary>
    private void DestroyPiece(int row, int column)
    {
        if (pieces[row,column] == null) return;
        else Destroy(pieces[row,column]);
        pieces[row, column] = null;
    }
}
