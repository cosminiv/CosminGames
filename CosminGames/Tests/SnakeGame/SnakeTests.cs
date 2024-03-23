using NUnit.Framework;
using System;
using CosminGames.SnakeGame;
using System.Collections.Generic;
using System.Linq;

namespace Tests.SnakeGame
{
    public class SnakeTests
    {
        private const int DefaultFieldWidth = 20;
        private const int DefaultFieldHeight = 20;
        private const int DefaultPositionX = 10;
        private const int DefaultPositionY = 10;

        private Engine _engine = new(new Field(DefaultFieldWidth, DefaultFieldHeight));
        private Point _defaultPosition = new Point(10, 10);

        [TestCase(-20, -30)]
        [TestCase(-20, 30)]
        [TestCase(20, -30)]
        [TestCase(20, 0)]
        [TestCase(0, 50)]
        [TestCase(0, 0)]
        public void GameFieldInitialization_WithNonPositiveArguments_ShouldThrow(int fieldWidth, int fieldHeight)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Field(fieldWidth, fieldHeight));
        }

        [TestCase(20, 30)]
        [TestCase(30, 20)]
        [TestCase(20, 20)]
        [TestCase(44, 77)]
        [TestCase(1789, 4567)]
        public void GameFieldInitialization_WithPositiveArguments_ShouldWork(int fieldWidth, int fieldHeight)
        {
            Field field = new(fieldWidth, fieldHeight);

            Assert.That(field.Width, Is.EqualTo(fieldWidth));
            Assert.That(field.Height, Is.EqualTo(fieldHeight));
        }

        [TestCase(-20, -30)]
        [TestCase(-20, 30)]
        [TestCase(20, -30)]
        [TestCase(44, 77)]
        [TestCase(1789, 4567)]
        public void PointInitialization_ShouldWork(int x, int y)
        {
            Point point = new(x, y);

            Assert.That(point.X, Is.EqualTo(x));
            Assert.That(point.Y, Is.EqualTo(y));
        }

        [TestCase]
        public void SnakeInitialization_ShouldWork()
        {
            Snake snake = new Snake(Direction.Up, new Point(10, 10));
            Assert.That(snake.Length, Is.EqualTo(Snake.DefaultLength));
        }

        [TestCase(Direction.Right, DefaultPositionX + 1, DefaultPositionY)]
        [TestCase(Direction.Left, DefaultPositionX - 1, DefaultPositionY)]
        [TestCase(Direction.Up, DefaultPositionX, DefaultPositionY - 1)]
        [TestCase(Direction.Down, DefaultPositionX, DefaultPositionY + 1)]
        public void DefaultSnake_MovesCorrectly(Direction movementDirection, int expectedFinalPositionX, int expectedFinalPositionY)
        {
            // arrange
            GameState initialGameState = new(new Snake(movementDirection, _defaultPosition));

            // act
            GameState nextGameState = _engine.GetNextState(initialGameState);

            // assert
            Assert.That(nextGameState.IsFinal, Is.False);
            Assert.That(nextGameState.Snake.HeadPosition.X, Is.EqualTo(expectedFinalPositionX));
            Assert.That(nextGameState.Snake.HeadPosition.Y, Is.EqualTo(expectedFinalPositionY));
            Assert.That(nextGameState.Snake.Length, Is.EqualTo(1));
        }

        [TestCase(DefaultFieldWidth - 1, 10, Direction.Right)]
        [TestCase(0, 10, Direction.Left)]
        [TestCase(0, DefaultFieldHeight, Direction.Down)]
        [TestCase(3, 0, Direction.Up)]
        public void DefaultSnake_MovesCorrectlyOutOfField(int X, int Y, Direction direction)
        {
            // arrange
            Point initialPosition = new(X, Y);
            GameState initialGameState = new(new Snake(direction, initialPosition));

            // act
            GameState nextGameState = _engine.GetNextState(initialGameState);

            // assert
            Assert.That(nextGameState.IsFinal, Is.True);
        }

        [Test]
        public void GetNextState_FromFinalState_ShouldFail()
        {
            GameState state = new(new Snake(Direction.Right, new Point(0, 0)));
            state.IsFinal = true;
            
            Assert.Throws<InvalidOperationException>(() => _engine.GetNextState(state));
        }

        [Test]
        public void ThreePieceSnake_MovesCorrectly_Down()
        {
            // arrange
            Point[] pieces = new Point[] { new(10, 10), new(9, 10), new(8, 10) };
            GameState initialGameState = new(new Snake(Direction.Down, pieces));

            // act
            GameState nextGameState = _engine.GetNextState(initialGameState);

            // assert
            Point[] expectedNewPosition = new Point[] { new(10, 11), new(10, 10), new(9, 10) };
            CollectionAssert.AreEqual(expectedNewPosition, nextGameState.Snake.PiecePositions);
        }

        [Test]
        public void ThreePieceSnake_MovesCorrectly_Up()
        {
            // arrange
            Point[] pieces = new Point[] { new(10, 10), new(11, 10), new(11, 11) };
            GameState initialGameState = new(new Snake(Direction.Up, pieces));

            // act
            GameState nextGameState = _engine.GetNextState(initialGameState);

            // assert
            Point[] expectedNewPosition = new Point[] { new(10, 9), new(10, 10), new(11, 10) };
            CollectionAssert.AreEqual(expectedNewPosition, nextGameState.Snake.PiecePositions);
        }

        [Test]
        public void ThreePieceSnake_MovesCorrectly_Left()
        {
            // arrange
            Point[] pieces = new Point[] { new(10, 10), new(11, 10), new(12, 10) };
            GameState initialGameState = new(new Snake(Direction.Left, pieces));

            // act
            GameState nextGameState = _engine.GetNextState(initialGameState);

            // assert
            Point[] expectedNewPosition = new Point[] { new(9, 10), new(10, 10), new(11, 10) };
            CollectionAssert.AreEqual(expectedNewPosition, nextGameState.Snake.PiecePositions);
        }

        [Test]
        public void Snake_DetectsSelfCollision_Left()
        {
            // arrange
            Point[] pieces = GetConsecutivePoints((10, 10), Direction.Down, Direction.Left, Direction.Up, Direction.Up);
            GameState initialGameState = new(new Snake(Direction.Left, pieces));

            // act
            GameState nextGameState = _engine.GetNextState(initialGameState);

            // assert
            Assert.That(nextGameState.IsFinal, Is.True);
        }

        [Test]
        public void Snake_DetectsSelfCollision_Right()
        {
            // arrange
            Point[] pieces = GetConsecutivePoints((10, 10), Direction.Left, Direction.Down, 
                Direction.Right, Direction.Right,
                Direction.Up, Direction.Up);
            GameState initialGameState = new(new Snake(Direction.Right, pieces));

            // act
            GameState nextGameState = _engine.GetNextState(initialGameState);

            // assert
            Assert.That(nextGameState.IsFinal, Is.True);
        }

        [Test]
        public void Snake_DetectsSelfCollision_Up()
        {
            // arrange
            Point[] pieces = GetConsecutivePoints((10, 10), Direction.Right,
                Direction.Up, Direction.Left, Direction.Left);
            GameState initialGameState = new(new Snake(Direction.Up, pieces));

            // act
            GameState nextGameState = _engine.GetNextState(initialGameState);

            // assert
            Assert.That(nextGameState.IsFinal, Is.True);
        }

        [Test]
        public void Snake_DetectsSelfCollision_Down()
        {
            // arrange
            Point[] pieces = GetConsecutivePoints((10, 10), Direction.Right,
                Direction.Down, Direction.Left, Direction.Left);
            GameState initialGameState = new(new Snake(Direction.Down, pieces));

            // act
            GameState nextGameState = _engine.GetNextState(initialGameState);

            // assert
            Assert.That(nextGameState.IsFinal, Is.True);
        }

        [Test]
        public void Snake_Can_Chase_ItsTail()
        {
            // arrange
            Point[] pieces = GetConsecutivePoints((10, 10), Direction.Right,
                Direction.Down, Direction.Left);
            GameState initialGameState = new(new Snake(Direction.Down, pieces));

            // act
            GameState nextGameState = _engine.GetNextState(initialGameState);

            // assert
            Assert.That(nextGameState.IsFinal, Is.False);
        }

        #region Helpers

        public static Point[] GetPointArray(params (int, int)[] tuples)
        {
            return tuples.Select(tuple => new Point(tuple.Item1, tuple.Item2)).ToArray();
        }

        public static Point[] GetConsecutivePoints((int x, int y) initial, params Direction[] directions)
        {
            Point[] result = new Point[1 + directions.Length];

            result[0] = new(initial.x, initial.y);

            for (int dirIndex = 0; dirIndex < directions.Length; dirIndex++)
            {
                Direction direction = directions[dirIndex];
                if (dirIndex > 0)
                    ValidateDirection(direction, directions[dirIndex - 1]);

                Point prevPoint = result[dirIndex];
                int x = prevPoint.X;
                int y = prevPoint.Y;

                if (direction == Direction.Up) y--;
                else if (direction == Direction.Down) y++;
                else if (direction == Direction.Left) x--;
                else if (direction == Direction.Right) x++;

                int pointIndex = dirIndex + 1;
                result[pointIndex] = new(x, y);
            }

            return result;
        }

        private static void ValidateDirection(Direction direction, Direction prevDirection)
        {
            bool isInvalid =
                prevDirection == Direction.Up && direction == Direction.Down ||
                prevDirection == Direction.Down && direction == Direction.Up ||
                prevDirection == Direction.Left && direction == Direction.Right ||
                prevDirection == Direction.Right && direction == Direction.Left;

            if (isInvalid)
                throw new InvalidOperationException($"Incorrect: prevDirection = {prevDirection}, direction = {direction}");
        }

        #endregion
    }
}