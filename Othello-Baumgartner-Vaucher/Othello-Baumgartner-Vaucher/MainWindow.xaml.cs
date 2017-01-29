using Microsoft.Win32;
using OthelloConsole;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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
    public partial class MainWindow : IPlayable, INotifyPropertyChanged
    {
        
        private MyButton[,] listButtons = new MyButton[8, 8];
        private bool isWhite = true;

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public int scoreWhite
        {
            get
            {
                return getWhiteScore();
            }
        }
        public int scoreBlack { get
            {
                return getBlackScore();
            }
        }

        public TimeSpan timeWhite { get; set; }
        public TimeSpan timeBlack { get; set; }

        public Timer MyTimer { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
    
            MyTimer = new Timer(100);
            MyTimer.Elapsed += MyTimer_Elapsed;
            MyTimer.Enabled = true;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    listButtons[i, j] = new MyButton(this, i, j);
                    Grid.SetRow(listButtons[i, j], i);
                    Grid.SetColumn(listButtons[i, j], j);
                    
                    boardGrid.Children.Add(listButtons[i, j]);
                }
            }

            listButtons[3, 3].Type = 1;
            listButtons[4, 3].Type = 2;
            listButtons[3, 4].Type = 2;
            listButtons[4, 4].Type = 1;

            showPlayableTiles(isWhite);
        }

        private void MyTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (isWhite)
            {
                timeWhite = timeWhite.Add(new TimeSpan(0,0,0,0,100));
                NotifyPropertyChanged("timeWhite");
            }
            else
            {
                timeBlack = timeBlack.Add(new TimeSpan(0, 0, 0, 0, 100));
                NotifyPropertyChanged("timeBlack");
            }
        }

        public void play(int x, int y)
        {
            if (((IPlayable)this).isPlayable(x, y, isWhite) && ((IPlayable)this).playMove(x, y, isWhite))
            {
                isWhite = !isWhite;
                if (!showPlayableTiles(isWhite)){
                    isWhite = !isWhite;
                    if (!showPlayableTiles(isWhite)){
                        Console.WriteLine("Partie finie");
                    }
                }
                NotifyPropertyChanged("scoreWhite");
                NotifyPropertyChanged("scoreBlack");
                Console.WriteLine("White: " + getWhiteScore().ToString());
                Console.WriteLine("Black: " + getBlackScore().ToString());
                if (isWhite)
                {
                    label_Tour.Content = "Tour du joueur blanc";
                }else
                {
                    label_Tour.Content = "Tour du joueur noir";
                }
            }
        }


        public bool showPlayableTiles(bool isWhite)
        {
            bool turnPossible = false;
            for(int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (listButtons[x, y].Type == 0)
                    {
                        if (((IPlayable)this).isPlayable(x, y, isWhite))
                        {
                            listButtons[x, y].changeBackground(true);
                            turnPossible = true;
                        }else
                        {
                            listButtons[x, y].changeBackground(false);
                        }
                    }
                }          
            }
            return turnPossible;
        }

        private void saveGame(String fileName)
        {
            string[] data = new string[4];
            data[0] = isWhite.ToString();

            string boardStatus = "";
            for(int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    boardStatus += listButtons[i, j].Type + ";";
                }
            }
            data[1] = boardStatus;
            data[2] = timeWhite.TotalMilliseconds.ToString();
            data[3] = timeBlack.TotalMilliseconds.ToString();
            System.IO.File.WriteAllLines(fileName, data);
        }

        private void loadGame(String fileName)
        {
            string[] data = System.IO.File.ReadAllLines(fileName);

            if (data[0].Equals("True"))
            {
                isWhite = true;
            }else
            {
                isWhite = false;
            }
            string[] boardStatus = data[1].Split(';');
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    listButtons[i, j].Type = Int32.Parse(boardStatus[i*8 + j]);
                }
            }

            timeWhite = new TimeSpan(0);
            timeBlack = new TimeSpan(0);
            
            timeWhite = timeWhite.Add(new TimeSpan(0, 0, 0, 0, Int32.Parse(data[2])));
            NotifyPropertyChanged("timeWhite");
            timeBlack = timeBlack.Add(new TimeSpan(0, 0, 0, 0, Int32.Parse(data[3])));
            NotifyPropertyChanged("timeBlack");

            showPlayableTiles(isWhite);
        }


        public void myButton_Click(object sender, RoutedEventArgs e)
        {
            MyButton button = (MyButton)sender;
            play(button.Row, button.Col);
        }

        private void button_Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if ((bool)saveFileDialog.ShowDialog())
            {
                saveGame(saveFileDialog.FileName);
            }
        }

        private void button_Load_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if ((bool)openFileDialog.ShowDialog())
            {
                loadGame(openFileDialog.FileName);
            }
        }

        private void button_NewGame_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    listButtons[i, j].Type = 0;
                }
            }

            listButtons[3, 3].Type = 1;
            listButtons[4, 3].Type = 2;
            listButtons[3, 4].Type = 2;
            listButtons[4, 4].Type = 1;

            isWhite = true;
            showPlayableTiles(isWhite);
            label_Tour.Content = "Tour du joueur blanc";

            timeWhite = new TimeSpan(0);
            timeBlack = new TimeSpan(0);

            NotifyPropertyChanged("timeBlack");
            NotifyPropertyChanged("timeWhite");
        }


        public int getBlackScore()
        {
            int score = 0;
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (listButtons[x, y].Type == 2)
                    {
                        score++;
                    }
                }
            }
            return score;
        }

        public Tuple<char, int> getNextMove(int[,] game, int level, bool whiteTurn)
        {
            throw new NotImplementedException();
        }

        public int getWhiteScore()
        {
            int score = 0;
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (listButtons[x, y].Type == 1)
                    {
                        score++;
                    }
                }
            }
            return score;
        }

        public bool isPlayable(int column, int line, bool isWhite)
        {
            if (listButtons[column, line].Type != 0)
            {
                return false;
            }

            List<MyButton> capturedTiles = new List<MyButton>();
            int actualPlayer, otherPlayer;
            if (isWhite)
            {
                actualPlayer = 1;
                otherPlayer = 2;
            }
            else
            {
                actualPlayer = 2;
                otherPlayer = 1;
            }

            for (int i = 0; i < 8; i++)
            {
                int[] tabx = new int[] { column, column, column - 1, column + 1, column + 1, column + 1, column - 1, column - 1 };
                int[] taby = new int[] { line - 1, line + 1, line, line, line - 1, line + 1, line + 1, line - 1 };
                int[,] step = new int[,] { { 0, -1 }, { 0, 1 }, { -1, 0 }, { 1, 0 }, { 1, -1 }, { 1, 1 }, { -1, 1 }, { -1, -1 } };

                for (int x = tabx[i], y = taby[i]; x >= 0 && y >= 0 && x < 8 && y < 8; x += step[i, 0], y += step[i, 1])
                {
                    if (listButtons[x, y].Type == otherPlayer)
                    {
                        capturedTiles.Add(listButtons[x, y]);
                    }
                    else
                    {
                        if (listButtons[x, y].Type == actualPlayer)
                        {
                            if(capturedTiles.Count > 0)
                            {
                                return true;
                            }
                        }
                        break;
                    }
                }
                capturedTiles.Clear();
            }
            return false;
        }

        public bool playMove(int column, int line, bool isWhite)
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

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            
        }
    }
}
