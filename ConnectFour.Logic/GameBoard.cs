using System;

namespace ConnectFour.Logic
{
    public class GameBoard
    {
        private readonly byte[,] board = new byte[7, 6];
        private byte numMoves = 0;
        private bool gameOver = false;

        internal bool playerOne = true;

        private byte CheckForVerticalWin(byte column, byte row)
        {
            if (row < 3)
            {
                return 0;
            }

            var player = board[column, row];
            for (var i = 0; i < 3; i++)
            {
                if (board[column, row - i - 1] != player)
                {
                    return 0;
                }
            }

            return player;
        }

        private byte CheckForHorizontalWin(byte row)
        {
            var player = board[0, row];
            var counter = 1;
            for (byte i = 1; i < 7; i++)
            {
                if (board[i, row] != player)
                {
                    player = board[i, row];
                    counter = 1;
                }
                else
                {
                    counter++;
                    if (counter == 4)
                    {
                        return player;
                    }
                }
            }
            return 0;
        }

        //LowerLeftToUpperRight
        private byte CheckForDiagonalLLTURWin(byte column, byte row)
        {
            var diff = Math.Min(column, row);
            var startColumn = column - diff;
            var startRow = row - diff;

            var player = board[startColumn, startRow];
            var counter = 1;
            for (var i = 1; startColumn + i < 7 && startRow + i < 6; i++)
            {
                if (board[startColumn + i, startRow + i] != player)
                {
                    player = board[startColumn + i, startRow + i];
                    counter = 1;
                }
                else
                {
                    counter++;
                    if (counter == 4)
                    {
                        return player;
                    }
                }
            }
            return 0;
        }

        //UpperLeftToLowerRight
        private byte CheckForDiagonalULTLRWin(byte column, byte row)
        {
            var diff = Math.Min(column, 5 - row);
            var startColumn = column - diff;
            var startRow = row + diff;

            var player = board[startColumn, startRow];
            var counter = 1;
            for (var i = 1; startColumn + i < 7 && startRow - i >= 0; i++)
            {
                if (board[startColumn + i, startRow - i] != player)
                {
                    player = board[startColumn + i, startRow - i];
                    counter = 1;
                }
                else
                {
                    counter++;
                    if (counter == 4)
                    {
                        return player;
                    }
                }
            }
            return 0;
        }

        private byte CheckForWin(byte column, byte row)
        {
            var crossWin = Math.Max(CheckForVerticalWin(column, row), CheckForHorizontalWin(row));
            var diagonalWin = Math.Max(CheckForDiagonalLLTURWin(column, row), CheckForDiagonalULTLRWin(column, row));

            var winner = Math.Max(crossWin, diagonalWin);
            return winner;
        }

        public byte SetStone(byte column)
        {
            if (gameOver)
            {
                throw new InvalidOperationException("Game already over");
            }

            if (column > 6)
            {
                throw new ArgumentOutOfRangeException(nameof(column));
            }

            for (byte row = 0; row < 6; row++)
            {
                if (board[column, row] == 0)
                {
                    board[column, row] = playerOne ? (byte)1 : (byte)2;
                    playerOne = !playerOne;
                    numMoves++;
                    var winner = CheckForWin(column, row);
                    if (winner == 0)
                    {
                        if (numMoves == 42)
                        {
                            gameOver = true;
                            throw new InvalidOperationException("Table is full");
                        }
                        return winner;
                    }
                    gameOver = true;
                    return winner;
                }
            }

            throw new InvalidOperationException("Column is full");
        }

        public byte[,] GetBoard()
        {
            var destination = new byte[7, 6];
            Array.Copy(board, 0, destination, 0, board.Length);
            return destination;
        }
    }
}
