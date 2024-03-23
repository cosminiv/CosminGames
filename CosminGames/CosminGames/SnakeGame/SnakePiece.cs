using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosminGames.SnakeGame
{
    internal class SnakePiece
    {
        public SnakePiece(Point position)
        {
            Position = position;
        }

        public SnakePiece(SnakePiece snakePiece)
        {
            Position = new Point(snakePiece.Position.X, snakePiece.Position.Y);
        }

        public Point Position { get; set; }
    }
}
