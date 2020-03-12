using ConnectFour.Logic;
using System;
using Xunit;

namespace ConnectFour.Tests
{
    public class GameBoardTests
    {
        [Fact]
        public void SetStoneInInvalidColumn()
        {
            var b = new GameBoard();

            var previousPlayer = b.playerOne;
            Assert.Throws<ArgumentOutOfRangeException>(() => b.SetStone(99));
            Assert.Equal(previousPlayer, b.playerOne);
        }

        [Fact]
        public void NextPlayerAfterSuccessfulMove()
        {
            var b = new GameBoard();

            var previousPlayer = b.playerOne;
            b.SetStone(0);
            Assert.NotEqual(previousPlayer, b.playerOne);
        }

        [Fact]
        public void NoWinnerAfterSingleMove()
        {
            var b = new GameBoard();
            var winner = b.SetStone(0);
            Assert.Equal(0, winner);
        }

        [Fact]
        public void SetStoneInFullColumn()
        {
            var b = new GameBoard();
            for (var i = 0; i < 6; i++)
            {
                b.SetStone(0);
            }

            var previousPlayer = b.playerOne;
            Assert.Throws<InvalidOperationException>(() => b.SetStone(0));
            Assert.Equal(previousPlayer, b.playerOne);
        }

        [Fact]
        public void VerticalWin()
        {
            var b = new GameBoard();
            for (var i = 0; i < 3; i++)
            {
                b.SetStone(0);
                b.SetStone(1);
            }

            var result = b.SetStone(0);
            Assert.Equal(1, result);
        }

        [Fact]
        public void HorizontalWin()
        {
            var b = new GameBoard();
            for (byte i = 0; i < 3; i++)
            {
                b.SetStone(i);
                b.SetStone(i);
            }

            var result = b.SetStone(3);
            Assert.Equal(1, result);
        }

        //LowerLeftToUpperRight
        [Fact]
        public void DiagonalWinLLTUR()
        {
            var b = new GameBoard();

            b.SetStone(0);
            b.SetStone(1);
            b.SetStone(1);
            b.SetStone(2);
            b.SetStone(2);
            b.SetStone(3);
            b.SetStone(2);
            b.SetStone(3);
            b.SetStone(3);
            b.SetStone(5);

            var result = b.SetStone(3);
            Assert.Equal(1, result);
        }

        //UpperLeftToLowerRight
        [Fact]
        public void DiagonalWinULTLR()
        {
            var b = new GameBoard();

            b.SetStone(3);
            b.SetStone(2);
            b.SetStone(2);
            b.SetStone(1);
            b.SetStone(1);
            b.SetStone(0);
            b.SetStone(1);
            b.SetStone(0);
            b.SetStone(0);
            b.SetStone(5);

            var result = b.SetStone(0);
            Assert.Equal(1, result);
        }

        [Fact]
        public void CheckFullBoard()
        {
            var b = new GameBoard();
            for (byte i = 0; i < 3; i++)
            {
                for (var j = 0; j < 6; j++)
                {
                    b.SetStone(i);
                }
            }
            b.SetStone(6);
            for (byte i = 3; i < 6; i++)
            {
                for (var j = 0; j < 6; j++)
                {
                    b.SetStone(i);
                }
            }
            for (var j = 0; j < 4; j++)
            {
                b.SetStone(6);
            }
            Assert.Throws<InvalidOperationException>(() => b.SetStone(6));
        }

        [Fact]
        public void CheckGameOver()
        {
            var b = new GameBoard();
            for (byte i = 0; i < 3; i++)
            {
                b.SetStone(i);
                b.SetStone(i);
            }

            b.SetStone(3);

            Assert.Throws<InvalidOperationException>(() => b.SetStone(6));
        }
    }
}
