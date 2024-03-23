namespace CosminGames.SnakeGame
{
    public partial class Snake
    {
        internal const int DefaultLength = 1;

        private List<SnakePiece> _pieces;
        
        public Snake(Direction movementDirection, params Point[] pieces)
        {
            if (pieces == null) throw new ArgumentNullException(nameof(pieces));
            if (pieces.Length < 1) throw new ArgumentException(nameof(pieces));

            MovementDirection = movementDirection;

            _pieces = new(100);
            _pieces.AddRange(pieces.Select(position => new SnakePiece(position)));
        }

        private SnakePiece Head => _pieces[0];

        public int Length => _pieces.Count;

        public Direction MovementDirection { get; set; }

        internal Point HeadPosition => Head.Position;

        public IEnumerable<Point> PiecePositions => _pieces.Select(piece => piece.Position);

        internal void Move()
        {
            SnakePiece head = MoveHead();

            _pieces.RemoveAt(_pieces.Count - 1);    // remove tail
            _pieces.Insert(0, head);                // add head
        }

        private SnakePiece MoveHead()
        {
            Point newHeadPosition = MovementDirection switch
            {
                Direction.Left => new(Head.Position.X - 1, Head.Position.Y),
                Direction.Right => new(Head.Position.X + 1, Head.Position.Y),
                Direction.Up => new(Head.Position.X, Head.Position.Y - 1),
                Direction.Down => new(Head.Position.X, Head.Position.Y + 1),
                _ => throw new ArgumentOutOfRangeException(nameof(MovementDirection)),
            };
            return new SnakePiece(newHeadPosition);
        }
    }
}
