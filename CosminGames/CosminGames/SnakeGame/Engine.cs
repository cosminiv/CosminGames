namespace CosminGames.SnakeGame
{
    internal class Engine
    {
        public Field Field { get; }

        public Engine(Field field)
        {
            Field = field;
        }

        internal GameState GetNextState(GameState initialGameState)
        {
            initialGameState.Snake.Move();
            GameState nextGameState = new(initialGameState.Snake);

            if (IsSnakeOutOfField(nextGameState.Snake))
                nextGameState.IsFinal = true;

            return nextGameState;
        }

        private bool IsSnakeOutOfField(Snake snake)
        {
            bool result = 
                snake.HeadPosition.X < 0 || 
                snake.HeadPosition.Y < 0 ||
                snake.HeadPosition.X >= Field.Width ||
                snake.HeadPosition.Y >= Field.Height;

            return result;
        }
    }
}
