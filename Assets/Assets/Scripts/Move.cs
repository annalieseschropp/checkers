using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CheckersState;

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

        public static bool operator ==(Square a, Square b)
        => (a.x == b.x) && (a.y == b.y);

        public static bool operator !=(Square a, Square b)
        => !(a==b);

        public static Square operator *(int n, Square a)
        => new Square(n * a.x, n * a.y);

        public static Square operator /(Square a, int b)
        => new Square(a.x / b, a.y / b);

        public static Square operator +(Square a, Square b)
        => new Square(a.x + b.x, a.y + b.y);

        public static Square operator -(Square a, Square b)
        => a + (-b);

        override public string ToString()
        {
            return "{x: " + x + ", y: " + y + "}"; 
        }

        /// <summary>
        /// Method
        /// Checks if one square equals another.
        /// </summary>
        override public bool Equals(object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || ! this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Square s = (Square) obj;
                return this == s;
            }
        }

        /// <summary>
        /// Method
        /// Makes a hash code for the square. This code simply corresponds to
        /// the square's number, from 0 to 63.
        /// </summary>
        override public int GetHashCode()
        {
            return x*8 + y;
        }

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

        public Move(int srcX, int srcY, int destX, int destY)
        {
            src = new Square(srcX, srcY);
            dest = new Square(destX, destY);
        }

        public static bool operator ==(Move a, Move b)
        =>(a.src == b.src) && (a.dest == b.dest);

        public static bool operator !=(Move a, Move b)
        => !(a==b);

        override public string ToString()
        {
            return "{src: " + src.ToString() + ", dest: " + dest.ToString() + "}"; 
        }
        
        /// <summary>
        /// Method
        /// Checks if one move equals another.
        /// </summary>
        override public bool Equals(object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || ! this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Move m = (Move) obj;
                return this == m;
            }
        }

        /// <summary>
        /// Method
        /// Makes a hash code for the move. This code simply corresponds to
        /// the lexicographic ordering of all possible combinations of squares.
        /// It is equivalent to a 2-digit base 64 number, where the first digit
        /// is the number of the src square, and the last, the dest.
        /// </summary>
        override public int GetHashCode()
        {
            return src.GetHashCode() * 64 + dest.GetHashCode();
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
        White = 0,
        Black = 1,
    };

    /// <summary>
    /// Enum
    /// Represents the status of the game (ie. who won / is it a draw / is it still going).
    /// </summary>
    public enum GameStatus
    {
        InProgress = 0,
        WhiteWin = 1,
        BlackWin = 2,
        Draw = 3,
    };
}
