using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MatchingPairsGame
{
    public partial class MatchingPairsForm : Form
    {
        tile firstClickedTile = null;
        tile secondClickedTile = null;
        Random random = new Random();
        List<string> tileIcons = new List<string>();
        static List<tile> tiles = new List<tile>();
        public Control firstControlTile;
        public Control secondControlTile;

        public MatchingPairsForm()
        {
            InitializeComponent(); //system generated code
            
            GenerateRandomIconsSet();
            ShuffleIcons(tileIcons);
            CreateTiles();
            AssignIconstoTiles();

            //Game starts here!
        }
        private void GenerateRandomIconsSet()
        {
            int tileCount = gameBoard.Controls.Count;
            int aSCIILowerLimit = 48;
            int aSCIIUpperLimit = 122;
            int i = 0;
            int pairsPerTile = 2;

            while (i < tileCount)
            {
                int randomNumber = random.Next(aSCIILowerLimit, aSCIIUpperLimit);
                string newSymbol = Convert.ToChar(randomNumber).ToString();
                if (!tileIcons.Contains(newSymbol))
                    for (int j = 0; j < pairsPerTile * 2; j++)
                    {
                        tileIcons.Add(newSymbol);
                        i++;
                    }
            }
        }

        private static List<string> ShuffleIcons(List<string> list)
        {
            Random random = new Random();
            int i = list.Count;

            // Fisher-Yates shuffle algorithm
            while (i > 0)
            {
                i--;
                int randomIndex = random.Next(i);
                listSwap(list, i, randomIndex);
            }
            return list;

            void listSwap(List<string> listForSwapping, int pos1, int pos2)
            {
                var tempValue = listForSwapping[pos1];
                listForSwapping[pos1] = listForSwapping[pos2];
                listForSwapping[pos2] = tempValue;
            }
        }

        private void CreateTiles()
        {
            for (int i = 0; i< tileIcons.Count; i++)
            {
                tile tile = new tile();
                tile.number = i;
                tile.symbol = tileIcons[i];
                tile.open = false;
                tiles.Add(tile);
            }
        }

        class tile
        {
            public int number;
            public string symbol;
            public bool open;
        }

        private void CheckForWin()
        {
            foreach (var tile in tiles)
            {
                if (!tile.open)
                    return;
            }
            
            MessageBox.Show("Congratulations, dear Samurai! Your quest ends here");
            Close();
        }
        //Still coupled
        private void AssignIconstoTiles()
        {
            int i = 0;
            foreach (Control tile in gameBoard.Controls)
            {
                if (tile != null)
                {
                    tile.Name = tiles[i].number.ToString();
                    tile.Text = tiles[i].symbol;
                    HideTile(tile);
                    i++;
                }
                
            }
        }

      //Still coupled
        private void tile_click(object sender, EventArgs e)
        {

            if (GameTimer.Enabled == true)
                return;

            Label clickedTile = sender as Label;
            if (clickedTile != null)
            {
                if (clickedTile.ForeColor == Color.Black)
                    return;

                ClickTile(clickedTile);

                CheckForMatch(ref firstClickedTile, ref secondClickedTile);

                CheckForWin();
            }
        }

        //Still coupled
        private void GameTimer_Tick(object sender, EventArgs e)
        {
            GameTimer.Stop();
            HideTile(firstControlTile);
            HideTile(secondControlTile);
            firstClickedTile = null;
            secondClickedTile = null;
            firstControlTile = null;
            secondControlTile = null;

        }

        //Still coupled
        private void ClickTile(Label clickedTile)
        {
            if (firstClickedTile == null)
            {
                firstClickedTile = tiles[Convert.ToInt32(clickedTile.Name)];
                firstControlTile = clickedTile;
                RevealTile(clickedTile);
                return;
            }

            secondClickedTile = tiles[Convert.ToInt32(clickedTile.Name)];
            secondControlTile = clickedTile;
            RevealTile(clickedTile);
        }


        private void CheckForMatch(ref tile firstClickedTile, ref tile secondClickedTile)
        {
            if (firstClickedTile == null || secondClickedTile == null)
                return;

            if (firstClickedTile.symbol == secondClickedTile.symbol)
            {
                firstClickedTile.open = true;
                secondClickedTile.open = true;
                firstClickedTile = null;
                secondClickedTile = null;
                return;
            }

            GameTimer.Start();
        }

        //Still coupled
        private void RevealTile(Control tile)
        {
            tile.ForeColor = Color.Black;
        }

        //Still coupled
        private void HideTile(Control tile)
        {
            tile.ForeColor = tile.BackColor;
        }

    }
}