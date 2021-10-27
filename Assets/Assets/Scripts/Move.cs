namespace CheckersMove
{
    /// <summary>
    /// Struct
    /// Holds a representation of a single square (just and (x,y) coordinate).
    /// </summary>
    public struct Square {
        public int x;
        public int y;
    }

    /// <summary>
    /// Struct
    /// Holds a representation of a single move (src and dest square).
    /// </summary>
    public struct Move {
        Square src;
        Square dest;
    }

    /// <summary>
    /// Enum
    /// Represents the colour of the current turn.
    /// </summary>
    public enum Turn
    {
        GameOver = 0,
        White = 1,
        Black = 2,
    };
}
