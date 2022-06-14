using System;

namespace Assets.Scripts.Model
{
    public sealed class Square : IEquatable<Square>
    {
        public int x { get; set; }
        public int y { get; set; }

        public Square(int inX, int inY)
        {
            x = inX;
            y = inY;
        }

        public Square()
        {

        }

        public bool Equals(Square inSquare)
        {
            return x == inSquare.x && y == inSquare.y;
        }

        public static Square operator +(Square a, Square b) => new Square(a.x + b.x, a.y + b.y);
    }
}
