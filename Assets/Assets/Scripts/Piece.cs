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
  
    /// <summary>
    /// Constructor for the piece class.
    /// Initializes all values to represent an empty piece.
    /// </summary>
    void Awake()
    {
        this.type = 0;
        this.sprite = null;
        this.InitPieceMap();
    }

    /// <summary>
    /// Method
    /// Changes the piece's type to the given paramter.
    /// </summary>
    public void SetPieceType(CheckersState.State newType)
    {
        if(newType == type) return;

        this.InitPieceMap();

        if (this.PieceMap[newType] == null)
        {
            this.ResetSprite();
            return;
        }

        Vector2 position = this.sprite == null ? new Vector3(0, 0, 0) : this.sprite.transform.position;
        this.ResetSprite();
        this.sprite = Instantiate(this.PieceMap[newType], position, Quaternion.identity) as GameObject;
        this.type = newType;
        this.sprite.transform.parent = this.transform;
        this.sprite.name = "sprite";
        this.sprite.GetComponent<SpriteRenderer>().sortingOrder = 1;
        this.AlignRotationToCamera();
        return;
    }

    /// <summary>
    /// Method
    /// Getter for the piece's current type.
    /// </summary>
    public CheckersState.State GetPieceType()
    {
        return this.type;
    }

    /// <summary>
    /// Method
    /// Rotates the piece so that kings will not appear upside down.
    /// </summary>
    public void AlignRotationToCamera()
    {
        if(this.sprite != null)
        {
            Debug.Log("Rotation: " + Camera.main.transform.localEulerAngles);
            this.sprite.transform.rotation = Quaternion.Euler(0.0f, 0.0f, Camera.main.transform.localEulerAngles.z);
        }
    }

    /// <summary>
    /// Method
    /// Initializes a piece at a location.
    /// </summary>
    public void InitializePiece(CheckersState.State type, Vector2 position, GameObject whitePiecePrefab, GameObject blackPiecePrefab, GameObject whiteKingPrefab, GameObject blackKingPrefab)
    {
        this.SetPrefabs(whitePiecePrefab, blackPiecePrefab, whiteKingPrefab, blackKingPrefab);
        this.InitPieceMap();
        this.ResetSprite();
        this.SetPieceType(type);
        if(this.sprite != null)
        {
            this.sprite.transform.position = position;
            this.AlignRotationToCamera();
        }
        return;
    }

    /// <summary>
    /// Method
    /// Setter for this script's prefabs: prefered over setting the public class member.
    /// </summary>
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
    /// </summary>
    /// <param name=newPosition>new position to move to</param>
    private IEnumerator MovePieceCoroutine(Vector2 newPosition)
    {
        float totalTime = (1 - GameOptionsStaticClass.moveSpeed);
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

    /// <summary>
    /// Method
    /// Initializes the mapping of prefabs to CheckerState.State values.
    /// </summary>
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
    /// </summary>
    /// <params name=newPosition>the new position to move to</params>
    public IEnumerator MovePiece(Vector2 newPosition)
    {
        yield return StartCoroutine(this.MovePieceCoroutine(newPosition));
    }

    /// <summary>
    /// Method
    /// Ensure sprite gets destroyed with the piece.
    /// </summary>
    private void OnDestroy()
    {
        this.ResetSprite();
    }

    /// <summary>
    /// Method
    /// Clears the current sprite.
    /// </summary>
    private void ResetSprite()
    {
        if(this.sprite == null) return;

        Destroy(this.sprite);
        this.sprite = null;
        this.type = 0;
    }
}
