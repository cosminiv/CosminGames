namespace CosminGames.SnakeGame
{
    public class Food
    {
        public Food(int x, int y)
        {
            Position = new(x, y);
        }

        public Point Position { get; }
    }
}