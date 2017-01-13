using OthelloConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Othello_Baumgartner_Vaucher
{



    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IPlayable
    {
        
        private MyButton[,] listButtons = new MyButton[8, 8];
        private bool isWhite = true;

        public MainWindow()
        {
            InitializeComponent();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    listButtons[i, j] = new MyButton(this, i, j);
                    Grid.SetRow(listButtons[i, j], i);
                    Grid.SetColumn(listButtons[i, j], j);
                    
                    mainGrid.Children.Add(listButtons[i, j]);
                }
            }

            listButtons[3, 3].Type = 1;
            listButtons[4, 3].Type = 2;
            listButtons[3, 4].Type = 2;
            listButtons[4, 4].Type = 1;
        }

        public void myButton_Click(object sender, RoutedEventArgs e)
        {
            MyButton button = (MyButton)sender;

            play(button.Row, button.Col);
            

        }

        public void play(int x, int y)
        {
            if(((IPlayable)this).playMove(x, y, isWhite))
                isWhite = !isWhite;
        }

        int IPlayable.getBlackScore()
        {
            throw new NotImplementedException();
        }

        Tuple<char, int> IPlayable.getNextMove(int[,] game, int level, bool whiteTurn)
        {
            throw new NotImplementedException();
        }

        int IPlayable.getWhiteScore()
        {
            throw new NotImplementedException();
        }

        bool IPlayable.isPlayable(int column, int line, bool isWhite)
        {
            throw new NotImplementedException();
        }

        bool IPlayable.playMove(int column, int line, bool isWhite)
        {
            if (listButtons[column, line].Type == 0)
            {
                if (isWhite)
                    listButtons[column, line].Type = 1;
                else
                    listButtons[column, line].Type = 2;

            }
            List<MyButton> capturedTiles = new List<MyButton>();
            int actualPlayer, otherPlayer;
            if (isWhite)
            {
                actualPlayer = 1;
                otherPlayer = 2;
            }else
            {
                actualPlayer = 2;
                otherPlayer = 1;
            }

            for(int i = 0; i < 8; i++)
            {
                int[] tabx = new int[] { column, column, column - 1, column + 1, column + 1, column + 1, column - 1, column - 1 };
                int[] taby = new int[] { line - 1, line + 1, line, line, line - 1, line + 1, line + 1, line - 1 };
                int[,] step = new int[,] { { 0, -1 }, { 0, 1 }, { -1, 0 }, { 1, 0 }, { 1, -1 }, { 1, 1 }, { -1, 1 }, { -1, -1 } };

                for (int x = tabx[i], y = taby[i]; x >= 0 && y >= 0 && x < 8 && y < 8; x += step[i,0], y += step[i,1])
                {
                    if (listButtons[x, y].Type == otherPlayer)
                    {
                        capturedTiles.Add(listButtons[x, y]);
                    }
                    else
                    {
                        if (listButtons[x, y].Type == actualPlayer)
                        {
                            foreach (MyButton tile in capturedTiles)
                            {
                                tile.Type = actualPlayer;
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
