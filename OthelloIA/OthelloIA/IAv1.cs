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
            double a = Double.MinValue;
            double b = Double.MaxValue;
            isWhite = whiteTurn;

            double best = Double.MinValue;
            Tuple<int, int> bestMove = null;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (isPlayableIA(game, i, j, whiteTurn))
                    {
                        int[,] newGame = playMoveIA(game, i, j, whiteTurn);
                        double val = -alphabeta(newGame, level - 1, -b, -a, !whiteTurn);
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

        public double alphabeta(int[,] game, int depth, double a, double b, bool white)
        {
            if (depth == 0)
            {
                return eval(game);
            }

            double best = Double.MinValue;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (isPlayableIA(game, i, j, white))
                    {
                        int[,] newGame = playMoveIA(game, i, j, white);
                        double val = -alphabeta(newGame, depth-1, -b, -a, !white);
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

    
        int[,] matrixValues= {{ 25, -3, 11,  8,  8, 11, -3, 25 },
                              { -3, -7,  1,  1,  1,  1, -7, -3 },
                              { 11,  1,  2,  2,  2,  2,  1, 11 },
                              {  8,  1,  2,  1,  1,  2,  1,  8 },
                              {  8,  1,  2,  1,  1,  2,  1,  8 },
                              { 11,  1,  2,  2,  2,  2,  1, 11 },
                              { -3, -7,  1,  1,  1,  1, -7, -3 },
                              { 25, -3, 11,  8,  8, 11, -3, 25 }};


        public double eval(int[,] game)
        {
            int type = 0;

            int nbPionPlayer = 0;
            int nbPionOtherPlayer = 0;

            int nbMovePlayer = 0;
            int nbMoveOtherPlayer = 0;

            int allyCorner = 0;
            int ennemiCorner = 0;

            if (!isWhite)
            {
                type = 1;
            }

            allyCorner += (game[0, 0] == type) ? 1 : 0;
            ennemiCorner += (game[0, 0] != type && game[0, 0] != -1) ? 1 : 0;
            allyCorner += (game[7, 0] == type) ? 1 : 0;
            ennemiCorner += (game[7, 0] != type && game[7, 0] != -1) ? 1 : 0;
            allyCorner += (game[7, 7] == type) ? 1 : 0;
            ennemiCorner += (game[7, 7] != type && game[7, 7] != -1) ? 1 : 0;
            allyCorner += (game[0, 7] == type) ? 1 : 0;
            ennemiCorner += (game[0, 7] != type && game[0, 7] != -1) ? 1 : 0;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if(game[i,j] == type)
                    {
                        nbPionPlayer++;
                    }else if(game[i, j] != -1)
                    {
                        nbPionOtherPlayer++;
                    }else
                    {
                        if(isPlayableIA(game, i, j, isWhite))
                        {
                            nbMovePlayer++;
                        }
                        if (isPlayableIA(game, i, j, !isWhite))
                        {
                            nbMoveOtherPlayer++;
                        }
                    }
                }
           }

           double diffPion = (double)(nbPionPlayer - nbMoveOtherPlayer)/ (double)(nbPionPlayer + nbMoveOtherPlayer);
           double diffMove = (double)(nbMovePlayer - nbMoveOtherPlayer) / (double)(nbPionPlayer + nbMoveOtherPlayer);
           double diffCorner = ((double)allyCorner + (double)ennemiCorner) / ((double)allyCorner + (double)ennemiCorner);

            double value = diffMove + diffPion + diffCorner;

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

