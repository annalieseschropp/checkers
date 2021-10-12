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

    private int type;
    private GameObject sprite;
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
    }

    // Update is called once per frame
    void Update()
    {
        //Test();
    }

    void Test()
    {
        if (this.frames < 60) 
        {
            this.frames++;
            return;
        }
        else
        {
            this.frames = 0;
            this.SetPieceType((this.type + 1) % 5);
        }
    }

    /// <summary>
    /// Method
    /// Changes the piece's type to the given paramter.
    /// <params>newType</params>
    /// <returns>Void</returns>
    /// </summary>
    // FIXME the function parameters should reference a globally accessible enum
    public void SetPieceType(int newType)
    {
        if(newType == type) return;

        GameObject[] sprites = new GameObject[] {null, whitePiecePrefab, blackPiecePrefab, whiteKingPrefab, blackKingPrefab};

        if (newType < 0 || newType >= sprites.Length || sprites[newType] == null)
        {
            this.ResetSprite();
            return;
        }

        Vector2 position = new Vector2(0, 0);
        this.ResetSprite();
        this.sprite = Instantiate(sprites[newType], position, Quaternion.identity) as GameObject;
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
    public int GetPieceType()
    {
        return this.type;
    }

    /// <summary>
    /// Method
    /// Initializes a piece at a location.
    /// <params>int type, Vector2 position</params>
    /// <returns>int</returns>
    /// </summary>
    public void InitializePiece(int type, Vector2 position)
    {
        this.ResetSprite();
        this.SetPieceType(type);
        
        if(this.sprite != null) this.sprite.transform.position = position;
        return;
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
