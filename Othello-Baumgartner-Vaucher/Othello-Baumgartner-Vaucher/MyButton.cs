using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Othello_Baumgartner_Vaucher
{
    class MyButton : Button
    {

        private MainWindow parent;
        private LinearGradientBrush defaultBackground;
        private LinearGradientBrush playableBackground;
        private int type = 0; // 0 = personne, 1 = joueur blanc, 2 = joueur noir
        public int Type
        {
            get { return type; }
            set
            {
                type = value;
                //Change de couleur en fonction du joueur qui a pris la case
                if(type == 1)
                {
                    Background = new LinearGradientBrush(Colors.White, Colors.White, 90);
                }
                else if (type == 2)
                {
                    Background = new LinearGradientBrush(Colors.Black, Colors.Black, 90);
                }
            }
        }

        private int row;
        public int Row
        {
            get { return row; }
        }

        private int col;
        public int Col
        {
            get { return col; }
        }

        public MyButton(MainWindow parent, int row, int col)
        {
            this.parent = parent;
            this.row = row;
            this.col = col;
            defaultBackground = new LinearGradientBrush(Colors.Green, Colors.DarkGreen, 90);
            playableBackground = new LinearGradientBrush(Colors.GreenYellow, Colors.GreenYellow, 90);
            this.Background = defaultBackground;
            this.Click += parent.myButton_Click;
        }

        public void changeBackground(bool isPlayable)
        {
            if (isPlayable)
            {
                this.Background = playableBackground;
            }else
            {
                this.Background = defaultBackground;
            }
        }

    }
}
