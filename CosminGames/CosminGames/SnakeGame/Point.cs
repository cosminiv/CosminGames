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
            Point other = obj as Point;
            if (other == null) return false;
            return X == other.X && Y == other.Y;
        }
    }
}
