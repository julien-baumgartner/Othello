using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OthelloIA6
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
                            bestMove = Tuple.Create(i, j);
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
            if(bestMove == null)
            {
                bestMove = Tuple.Create(-1, -1);
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
            if(best == Int32.MinValue)
            {
                return eval(game);
            }
            return best;
        }

        int[,] matrix3Diag = {{ 1, 1, 1, 1 },
                                  { 1, 1, 1, 0 },
                                  { 1, 1, 0, 0 },
                                  { 1, 0, 0, 0 }};

        int[,] matrix2Diag = {{ 1, 1, 1, 0 },
                                  { 1, 1, 0, 0 },
                                  { 1, 0, 0, 0 },
                                  { 0, 0, 0, 0 }};

        int[,] matrix1Diag = {{ 1, 1, 0, 0 },
                                  { 1, 0, 0, 0 },
                                  { 0, 0, 0, 0 },
                                  { 0, 0, 0, 0 }};
    
        int[,] matrixValues= {{ 25, -3, 11,  8,  8, 11, -3, 25 },
                              { -3, -7,  1,  1,  1,  1, -7, -3 },
                              { 11,  1,  2,  2,  2,  2,  1, 11 },
                              {  8,  1,  2,  1,  1,  2,  1,  8 },
                              {  8,  1,  2,  1,  1,  2,  1,  8 },
                              { 11,  1,  2,  2,  2,  2,  1, 11 },
                              { -3, -7,  1,  1,  1,  1, -7, -3 },
                              { 25, -3, 11,  8,  8, 11, -3, 25 }};

        /*int[,] matrixValues = {{ 20, -3, 11,  8,  8, 11, -3, 20 },
                              { -3, -7, -4,  1,  1, -4, -7, -3 },
                              { 11, -4,  2,  2,  2,  2, -4, 11 },
                              {  8,  1,  2, -3, -3,  2,  1,  8 },
                              {  8,  1,  2, -3, -3,  2,  1,  8 },
                              { 11, -4,  2,  2,  2,  2, -4, 11 },
                              { -3, -7, -4,  1,  1, -4, -7, -3 },
                              { 20, -3, 11,  8,  8, 11, -3, 20 }};*/

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
                        value += matrixValues[i, j];
                    }else if(game[i, j] != -1)
                    {
                        value -= matrixValues[i, j];
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
            int[,] newBoard = new int[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    newBoard[i, j] = game[i, j];
                }
            }
            if (newBoard[column, line] == -1)
            {
                if (isWhite)
                    newBoard[column, line] = 0;
                else
                    newBoard[column, line] = 1;

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
                    if (newBoard[x, y] == otherPlayer)
                    {
                        int[] pos = { x, y };
                        capturedTiles.Add(pos);
                    }
                    else
                    {
                        if (newBoard[x, y] == actualPlayer)
                        {
                            foreach (int[] pos in capturedTiles)
                            {
                                newBoard[pos[0], pos[1]] = actualPlayer;
                            }
                        }
                        break;
                    }
                }
                capturedTiles.Clear();
            }
            return newBoard;
        }

    }
}

