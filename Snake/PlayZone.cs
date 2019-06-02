using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace Snake
{
    public class PlayZone
    {
        public Bitmap kép = new Bitmap(500, 500);
        public int mapSize;
        public int mapUnit;

        public void uresNegyzet(int x, int y)
        {
            for (int i = 0; i < mapUnit; i++)
            {
                kép.SetPixel(x * mapUnit + i, y * mapUnit, Color.LightGray);
                kép.SetPixel(x * mapUnit + i, y * mapUnit + (500 / mapSize - 1), Color.LightGray);
                kép.SetPixel(x * mapUnit, y * mapUnit + i, Color.LightGray);
                kép.SetPixel(x * mapUnit + (500 / mapSize - 1), y * mapUnit + i, Color.LightGray);
            }
        }

        public void teliNegyzet(int x, int y, Color szín)
        {
            for (int i = 0; i < mapUnit; i++)
            {
                for (int j = 0; j < mapUnit; j++)
                {
                    kép.SetPixel(x * mapUnit + i, y * mapUnit + j, szín);
                }
            }
        }

        public void clearBoard()
        {
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    teliNegyzet(i, j, Color.White);
                    uresNegyzet(i, j);
                }
            }
        }
    }
}