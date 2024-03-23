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
            if (initialGameState.IsFinal) 
                throw new InvalidOperationException("Can't move from final state");

            initialGameState.Snake.Move(out bool isSelfCollision);
            GameState nextGameState = new(initialGameState.Snake);

            if (IsSnakeOutOfField(nextGameState.Snake) || isSelfCollision)
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
