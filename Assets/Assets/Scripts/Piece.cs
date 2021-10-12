using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CheckersState;

/// <summary>
/// Class
/// Visual representation of a 2D checker piece.
/// </summary>
public class Piece : MonoBehaviour
{
    public GameObject whitePiecePrefab;
    public GameObject blackPiecePrefab;
    public GameObject whiteKingPrefab;
    public GameObject blackKingPrefab;

    private CheckersState.State type;
    private GameObject sprite;
    private Dictionary<CheckersState.State, GameObject> PieceMap;
    private int frames;
  
    /// <summary>
    /// Constructor for the piece class.
    /// Initializes all values to represent an empty piece.
    /// </summary>
    void Start()
    {
        this.type = 0;
        this.sprite = null;
        this.frames = 0;
        this.InitPieceMap();
    }

    // Update is called once per frame
    void Update()
    {
        //
    }

    /// <summary>
    /// Method
    /// Changes the piece's type to the given paramter.
    /// <params>newType</params>
    /// <returns>Void</returns>
    /// </summary>
    // FIXME the function parameters should reference a globally accessible enum
    public void SetPieceType(CheckersState.State newType)
    {
        if(newType == type) return;

        this.InitPieceMap();

        if (this.PieceMap[newType] == null)
        {
            this.ResetSprite();
            return;
        }

        Vector2 position = new Vector2(0, 0);
        this.ResetSprite();
        this.sprite = Instantiate(this.PieceMap[newType], position, Quaternion.identity) as GameObject;
        this.type = newType;
        this.sprite.transform.parent = this.transform;
        this.sprite.name = "sprite";
        this.sprite.GetComponent<SpriteRenderer>().sortingOrder = 1;

        return;
    }

    /// <summary>
    /// Method
    /// Getter for the piece's current type.
    /// <params>None</params>
    /// <returns>int</returns>
    /// </summary>
    public CheckersState.State GetPieceType()
    {
        return this.type;
    }

    /// <summary>
    /// Method
    /// Initializes a piece at a location.
    /// <params>int type, Vector2 position</params>
    /// <returns>int</returns>
    /// </summary>
    public void InitializePiece(CheckersState.State type, Vector2 position, GameObject whitePiecePrefab, GameObject blackPiecePrefab, GameObject whiteKingPrefab, GameObject blackKingPrefab)
    {
        this.SetPrefabs(whitePiecePrefab, blackPiecePrefab, whiteKingPrefab, blackKingPrefab);
        this.InitPieceMap();
        this.ResetSprite();
        this.SetPieceType(type);
        if(this.sprite != null) this.sprite.transform.position = position;
        return;
    }

    public void SetPrefabs(GameObject whitePiecePrefab, GameObject blackPiecePrefab, GameObject whiteKingPrefab, GameObject blackKingPrefab)
    {
        this.whitePiecePrefab = whitePiecePrefab;
        this.blackPiecePrefab = blackPiecePrefab;
        this.whiteKingPrefab = whiteKingPrefab;
        this.blackKingPrefab = blackKingPrefab;
        this.InitPieceMap();
    }

    /// <summary>
    /// Method
    /// Smoothly moves the piece to a new location
    /// <params>Vector2 newPosition</params>
    /// <returns>int</returns>
    /// </summary>
    private IEnumerator MovePieceCoroutine(Vector2 newPosition)
    {
        float totalTime = 0.75f;
        float elapsedTime = 0f;
        Vector2 startPosition = this.sprite.transform.position;
        Vector2 endPosition = newPosition;
        GameObject sprite = this.sprite;
    
        while (Vector2.Distance(sprite.transform.position, endPosition) > 0)
        {
            elapsedTime += Time.deltaTime;
            sprite.transform.position = Vector2.Lerp(startPosition, endPosition, elapsedTime/totalTime);
            yield return null;
        }
    }

    private void InitPieceMap()
    {
        this.PieceMap = new Dictionary<CheckersState.State, GameObject>();
        this.PieceMap[CheckersState.State.Empty] = null;
        this.PieceMap[CheckersState.State.White] = whitePiecePrefab;
        this.PieceMap[CheckersState.State.Black] = blackPiecePrefab;
        this.PieceMap[CheckersState.State.WhiteKing] = whiteKingPrefab;
        this.PieceMap[CheckersState.State.BlackKing] = blackKingPrefab;
    }

    /// <summary>
    /// Method
    /// Public interface for moving the piece.
    /// <params>Vector2 newPosition</params>
    /// <returns>int</returns>
    /// </summary>
    public void MovePiece(Vector2 newPosition)
    {
        StartCoroutine(this.MovePieceCoroutine(newPosition));
    }

    /// <summary>
    /// Method
    /// Clears the current sprite.
    /// <params>None</params>
    /// <returns>Void</returns>
    /// </summary>
    private void ResetSprite()
    {
        if(this.sprite == null) return;

        Destroy(this.sprite);
        this.sprite = null;
        this.type = 0;
    }
}
