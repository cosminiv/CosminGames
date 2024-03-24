namespace CosminGames.SnakeGame
{
    public class Point
    {
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; private set; } = 0;
        public int Y { get; private set; } = 0;

        public override string ToString() => $"({X}, {Y})";

        public override bool Equals(object? obj)
        {
            if (obj is not Point other) return false;

            return X == other.X && Y == other.Y;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        public static bool operator== (Point a, Point b) => a.Equals(b);

        public static bool operator!= (Point a, Point b) => !a.Equals(b);
    }
}
