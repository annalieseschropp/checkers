namespace CheckersMove
{
    /// <summary>
    /// Struct
    /// Holds a representation of a single square (just and (x,y) coordinate).
    /// </summary>
    public struct Square {
        public Square(int X, int Y)
        {
            x = X;
            y = Y;
        }

        public static Square operator +(Square a)
        => a;

        public static Square operator -(Square a)
        => new Square(-a.x,-a.y);

        public static Square operator *(int n, Square a)
        => new Square(n * a.x, n * a.y);

        public static Square operator /(Square a, int b)
        => new Square(a.x / b, a.y / b);

        public static Square operator +(Square a, Square b)
        => new Square(a.x + b.x, a.y + b.y);

        public static Square operator -(Square a, Square b)
        => a + (-b);

        public int x;
        public int y;
    }

    /// <summary>
    /// Struct
    /// Holds a representation of a single move (src and dest square).
    /// </summary>
    public struct Move {
        public Move(Square srcSquare, Square destSquare)
        {
            src = srcSquare;
            dest = destSquare;
        }

        public Square src;
        public Square dest;
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
