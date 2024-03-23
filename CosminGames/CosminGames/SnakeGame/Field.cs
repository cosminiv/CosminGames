namespace CosminGames.SnakeGame
{
    internal class Field
    {
        public Field(int fieldWidth, int fieldHeight)
        {
            if (fieldWidth <= 0) throw new ArgumentOutOfRangeException(nameof(fieldWidth), "Should be a positive number");
            if (fieldHeight <= 0) throw new ArgumentOutOfRangeException(nameof(fieldHeight), "Should be a positive number");

            Width = fieldWidth;
            Height = fieldHeight;
        }

        public int Width { get; }
        public int Height { get; }
    }
}
