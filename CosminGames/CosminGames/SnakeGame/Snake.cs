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

        internal void Move(out bool isSelfCollision)
        {
            int headX = HeadPosition.X;
            int headY = HeadPosition.Y;

            _pieces.RemoveAt(_pieces.Count - 1);    // remove tail
            SnakePiece head = MoveHead(headX, headY, out isSelfCollision);
            _pieces.Insert(0, head);                // add head
        }

        private SnakePiece MoveHead(int x, int y, out bool isSelfCollision)
        {
            Point newHeadPosition = MovementDirection switch
            {
                Direction.Left => new(x - 1, y),
                Direction.Right => new(x + 1, y),
                Direction.Up => new(x, y - 1),
                Direction.Down => new(x, y + 1),
                _ => throw new ArgumentOutOfRangeException(nameof(MovementDirection)),
            };

            isSelfCollision = _pieces.Any(piece => piece.Position.Equals(newHeadPosition));

            return new SnakePiece(newHeadPosition);
        }
    }
}
