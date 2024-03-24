namespace CosminGames.SnakeGame
{
    public class GameState
    {
        public GameState(Snake snake)
        {
            Snake = snake;
        }

        public GameState(Snake snake, Food food) : this(snake)
        {
            Food = food;
        }

        public Snake Snake { get; }

        public Food? Food { get; }

        public bool IsFinal { get; set; } = false;
    }
}
