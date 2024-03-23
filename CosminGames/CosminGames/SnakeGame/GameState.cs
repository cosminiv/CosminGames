namespace CosminGames.SnakeGame
{
    public class GameState
    {
        public GameState(Snake snake)
        {
            Snake = snake;
        }

        public Snake Snake { get; }

        public bool IsFinal { get; set; } = false;
    }
}
