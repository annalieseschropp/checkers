namespace CheckersMove
{
    /// <summary>
    /// Struct
    /// Holds a representation of a single square (just and (x,y) coordinate).
    /// </summary>
    public struct Square {
        int x;
        int y;
    }

    /// <summary>
    /// Struct
    /// Holds a representation of a single move (src and dest square).
    /// </summary>
    public struct Move {
        Square src;
        Square dest;
    }
}
