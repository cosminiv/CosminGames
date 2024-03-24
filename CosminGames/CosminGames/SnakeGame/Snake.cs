namespace CosminGames.SnakeGame
{
    public partial class Snake
    {
        internal const int DefaultLength = 1;

        private List<SnakePiece> _pieces;
        private Direction _direction;

        public Snake(Direction direction, params Point[] pieces)
        {
            if (pieces == null) throw new ArgumentNullException(nameof(pieces));
            if (pieces.Length < 1) throw new ArgumentException(nameof(pieces));

            _direction = direction;

            _pieces = new(100);
            _pieces.AddRange(pieces.Select(position => new SnakePiece(position)));
        }

        private SnakePiece Head => _pieces[0];

        public int Length => _pieces.Count;

        public Direction Direction 
        {
            get => _direction;

            set 
            {
                if (!AreDirectionsOpposite(_direction, value))
                    _direction = value;
            }
        }

        internal Point HeadPosition => Head.Position;

        public IEnumerable<Point> PiecePositions => _pieces.Select(piece => piece.Position);

        internal void Move(Food? food, out bool isSelfCollision)
        {
            int headX = HeadPosition.X;
            int headY = HeadPosition.Y;

            int tailX = _pieces[^1].Position.X;
            int tailY = _pieces[^1].Position.Y;

            _pieces.RemoveAt(_pieces.Count - 1);    // remove tail
            SnakePiece head = MoveHead(headX, headY, out isSelfCollision);
            _pieces.Insert(0, head);                // add head

            // if found food, add it as tail
            if (food != null && food.Position == HeadPosition)
                _pieces.Add(new SnakePiece(new Point(tailX, tailY)));
        }

        private SnakePiece MoveHead(int x, int y, out bool isSelfCollision)
        {
            Point newHeadPosition = Direction switch
            {
                Direction.Left => new(x - 1, y),
                Direction.Right => new(x + 1, y),
                Direction.Up => new(x, y - 1),
                Direction.Down => new(x, y + 1),
                _ => throw new ArgumentOutOfRangeException(nameof(Direction)),
            };

            isSelfCollision = _pieces.Any(piece => piece.Position.Equals(newHeadPosition));

            return new SnakePiece(newHeadPosition);
        }

        private bool AreDirectionsOpposite(Direction dir1, Direction dir2)
        {
            if (dir1 == Direction.Left && dir2 == Direction.Right) return true;
            if (dir1 == Direction.Right && dir2 == Direction.Left) return true;
            if (dir1 == Direction.Up && dir2 == Direction.Down) return true;
            if (dir1 == Direction.Down && dir2 == Direction.Up) return true;

            return false;
        }
    }
}
