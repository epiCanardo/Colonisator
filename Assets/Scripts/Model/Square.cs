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

        public static Square operator *(Square a, int coef) => new Square(a.x * coef, a.y * coef);

        public static Square East => new Square(1, 0);
        public static Square West => new Square(-1, 0);
        public static Square North => new Square(0, 1);
        public static Square South => new Square(0, -1);

        public override string ToString()
        {
            return $"{x},{y}";
        }
    }
}
