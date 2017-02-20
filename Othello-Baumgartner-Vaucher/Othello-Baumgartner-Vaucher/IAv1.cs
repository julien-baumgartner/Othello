using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello_Baumgartner_Vaucher
{
    public class IAv1
    {

        private bool isWhite;

        public IAv1()
        {

        }

        public Tuple<int, int> getNextMove(int[,] game, int level, bool whiteTurn)
        {
            int a = Int32.MinValue;
            int b = Int32.MaxValue;
            isWhite = whiteTurn;

            int best = Int32.MinValue;
            Tuple<int, int> bestMove = null;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (isPlayableIA(game, i, j, whiteTurn))
                    {
                        int[,] newGame = playMoveIA(game, i, j, whiteTurn);
                        int val = -alphabeta(newGame, level - 1, -b, -a, !whiteTurn);
                        if (val > best)
                        {
                            best = val;
                            bestMove = Tuple.Create(1, 2);
                            if (best > a)
                            {
                                a = best;
                                if (a >= b) {
                                    return bestMove;
                                }
                            }
                        }
                    }
                }
            }
            return bestMove;
        }

        public int alphabeta(int[,] game, int depth, int a, int b, bool white)
        {
            if (depth == 0)
            {
                return eval(game);
            }

            int best = Int32.MinValue;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (isPlayableIA(game, i, j, white))
                    {
                        int[,] newGame = playMoveIA(game, i, j, white);
                        int val = -alphabeta(newGame, depth-1, -b, -a, !white);
                        if(val > best)
                        {
                            best = val;
                            if(best > a)
                            {
                                a = best;
                                if(a >= b)
                                {
                                    return best;
                                }
                            }
                        }
                    }
                }
             }
            return best;
        }

        public int eval(int[,] game)
        {
            int value = 0;
            int type = 0;
            if (!isWhite)
            {
                type = 1;
            }
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if(game[i,j] == type)
                    {
                        value++;
                    }
                }
           }
           return value;
        }

        //Indique si une case est jouable
        public bool isPlayableIA(int[,] game, int column, int line, bool isWhite)
        {
            if (game[column, line] != -1)
            {
                return false;
            }

            bool capturedTiles = false;
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

                for (int x = tabx[i], y = taby[i]; x >= 0 && y >= 0 && x < 8 && y < 8; x += step[i, 0], y += step[i, 1])
                {
                    if (game[x, y] == otherPlayer)
                    {
                        capturedTiles = true;
                    }
                    else
                    {
                        if (game[x, y] == actualPlayer)
                        {
                            if (capturedTiles)
                            {
                                return true;
                            }
                        }
                        break;
                    }
                }
                capturedTiles = false;
            }
            return false;
        }


        //Joue un pion
        public int[,] playMoveIA(int[,] game, int column, int line, bool isWhite)
        {
            if (game[column, line] == -1)
            {
                if (isWhite)
                    game[column, line] = 0;
                else
                    game[column, line] = 1;

            }

            List<int[]> capturedTiles = new List<int[]>();
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

                for (int x = tabx[i], y = taby[i]; x >= 0 && y >= 0 && x < 8 && y < 8; x += step[i, 0], y += step[i, 1])
                {
                    if (game[x, y] == otherPlayer)
                    {
                        int[] pos = { i, j };
                        capturedTiles.Add(pos);
                    }
                    else
                    {
                        if (game[x, y] == actualPlayer)
                        {
                            foreach (int[] pos in capturedTiles)
                            {
                                game[pos[0], pos[1]] = actualPlayer;
                            }
                        }
                        break;
                    }
                }
                capturedTiles.Clear();
            }
            return game;
        }

    }
}
}
