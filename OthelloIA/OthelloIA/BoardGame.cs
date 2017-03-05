using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IPlayable;

namespace OthelloIA6
{
    public class BoardGame : IPlayable.IPlayable
    {

        public int[,] boardstate = new int[8,8];
        public IAv1 ia;

        public BoardGame()
        {
            for(int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    boardstate[i, j] = -1;
                }
            }

            boardstate[3, 3] = 0;
            boardstate[3, 4] = 1;
            boardstate[4, 4] = 0;
            boardstate[4, 3] = 1;

            ia = new IAv1();
        }

        public int[,] GetBoard()
        {
            return boardstate;
        }

        public string GetName()
        {
            return "Vaucher-Baumgartner";
        }

        public Tuple<int, int> GetNextMove(int[,] game, int level, bool whiteTurn)
        {
            int[,]  testBoard =new int[8,8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    testBoard[i, j] = game[i, j];
                }
            }
            return ia.getNextMove(testBoard, level, whiteTurn);
        }

        public int GetBlackScore()
        {
            int score = 0;
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (boardstate[x,y] == 1)
                    {
                        score++;
                    }
                }
            }
            return score;
        }

        public int GetWhiteScore()
        {
            int score = 0;
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (boardstate[x, y] == 0)
                    {
                        score++;
                    }
                }
            }
            return score;
        }

        public bool IsPlayable(int column, int line, bool isWhite)
        {
            if (boardstate[column, line] != -1)
            {
                return false;
            }
            
            int actualPlayer, otherPlayer;
            if (isWhite)
            {
                actualPlayer = 0;
                otherPlayer = 1;
            }
            else
            {
                actualPlayer = 1;
                otherPlayer = 0;
            }

            for (int i = 0; i < 8; i++)
            {
                int[] tabx = new int[] { column, column, column - 1, column + 1, column + 1, column + 1, column - 1, column - 1 };
                int[] taby = new int[] { line - 1, line + 1, line, line, line - 1, line + 1, line + 1, line - 1 };
                int[,] step = new int[,] { { 0, -1 }, { 0, 1 }, { -1, 0 }, { 1, 0 }, { 1, -1 }, { 1, 1 }, { -1, 1 }, { -1, -1 } };

                bool isCatching = false;

                for (int x = tabx[i], y = taby[i]; x >= 0 && y >= 0 && x < 8 && y < 8; x += step[i, 0], y += step[i, 1])
                {
                    if (boardstate[x, y] == otherPlayer)
                    {
                        isCatching = true;
                    }
                    else
                    {
                        if (boardstate[x, y] == actualPlayer)
                        {
                            if (isCatching)
                            {
                                return true;
                            }
                        }
                        break;
                    }
                }
            }
            return false;
        }

        public bool PlayMove(int column, int line, bool isWhite)
        {
            if (boardstate[column, line] == -1)
            {
                if (isWhite)
                    boardstate[column, line] = 0;
                else
                    boardstate[column, line] = 1;

            }

            int actualPlayer, otherPlayer;
            if (isWhite)
            {
                actualPlayer = 0;
                otherPlayer = 1;
            }
            else
            {
                actualPlayer = 1;
                otherPlayer = 0;
            }

            for (int i = 0; i < 8; i++)
            {
                int[] tabx = new int[] { column, column, column - 1, column + 1, column + 1, column + 1, column - 1, column - 1 };
                int[] taby = new int[] { line - 1, line + 1, line, line, line - 1, line + 1, line + 1, line - 1 };
                int[,] step = new int[,] { { 0, -1 }, { 0, 1 }, { -1, 0 }, { 1, 0 }, { 1, -1 }, { 1, 1 }, { -1, 1 }, { -1, -1 } };

                List<Tuple<int,int>> capturedTiles = new List<Tuple<int,int>>();

                for (int x = tabx[i], y = taby[i]; x >= 0 && y >= 0 && x < 8 && y < 8; x += step[i, 0], y += step[i, 1])
                {
                    if (boardstate[x, y] == otherPlayer)
                    {
                        capturedTiles.Add(new Tuple<int, int>(x, y));
                    }
                    else
                    {
                        if (boardstate[x, y] == actualPlayer)
                        {
                            foreach (Tuple<int, int> coord in capturedTiles)
                            {
                                boardstate[coord.Item1,  coord.Item2] = actualPlayer;
                            }
                        }
                        break;
                    }
                }
                capturedTiles.Clear();
            }
            return true;
        }
    }
}
