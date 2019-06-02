using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    public partial class Form1 : Form
    {
        List<Point> snakeBodies = new List<Point>();
        Bitmap kép = new Bitmap(500, 500);
        string direction = "null";
        Random rnd = new Random();
        bool started = false;
        int mapSize = 30;
        int mapUnit;
        Point alma;

        public Form1()
        {
            InitializeComponent();
            mapUnit = 500 / mapSize;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) //Irányítás
        {
            if (keyData == Keys.Left)
            {
                if (direction == "right")
                    return true;
                direction = "left";
                return true;
            }
            if (keyData == Keys.Up)
            {
                if (direction == "down")
                    return true;
                direction = "up";
                return true;
            }
            if (keyData == Keys.Right)
            {
                if (direction == "left")
                    return true;
                direction = "right";
                return true;
            }
            if (keyData == Keys.Down)
            {
                if (direction == "up")
                    return true;
                direction = "down";
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        void clearBoard()
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

        void stop()
        {
            timer1.Stop();
            started = false;
            button1.Text = "Start";
            textBox2.Enabled = true;
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            alma = new Point(rnd.Next(0, mapSize), rnd.Next(0, mapSize));
            direction = "null";
            snakeBodies.Clear();
            snakeBodies.Add(new Point(rnd.Next(0, mapSize), rnd.Next(0, mapSize)));
            snakeBodies.Add(new Point(1, 1));
            if (!started)
            {
                clearBoard();
                timer1.Start();
                started = true;
                button1.Text = "Stop";
                textBox2.Enabled = false;
                teliNegyzet(alma.X, alma.Y, Color.Red);
            }
            else if (started)
            {
                stop();
            }
        }

        void uresNegyzet(int x, int y)
        {
            for (int i = 0; i < mapUnit; i++)
            {
                kép.SetPixel(x * mapUnit + i, y * mapUnit, Color.LightGray);
                kép.SetPixel(x * mapUnit + i, y * mapUnit + (500 / mapSize - 1), Color.LightGray);
                kép.SetPixel(x * mapUnit, y * mapUnit + i, Color.LightGray);
                kép.SetPixel(x * mapUnit + (500 / mapSize - 1), y * mapUnit + i, Color.LightGray);
            }
        }

        void teliNegyzet(int x, int y, Color szín)
        {
            for (int i = 0; i < mapUnit; i++)
            {
                for (int j = 0; j < mapUnit; j++)
                {
                    kép.SetPixel(x * mapUnit + i, y * mapUnit + j, szín);
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            teliNegyzet(snakeBodies[snakeBodies.Count - 1].X, snakeBodies[snakeBodies.Count - 1].Y, Color.White);
            uresNegyzet(snakeBodies[snakeBodies.Count - 1].X, snakeBodies[snakeBodies.Count - 1].Y);
            for (int i = snakeBodies.Count - 1; i > 0; i--)
            {
                snakeBodies[i] = snakeBodies[i - 1];
                //teliNegyzet(snakeBodies[i].X, snakeBodies[i].Y, Color.Black);
            }
            if (direction == "up")
                snakeBodies[0] = new Point(snakeBodies[0].X, snakeBodies[0].Y - 1);
            else if (direction == "right")
                snakeBodies[0] = new Point(snakeBodies[0].X + 1, snakeBodies[0].Y);
            else if (direction == "down")
                snakeBodies[0] = new Point(snakeBodies[0].X, snakeBodies[0].Y + 1);
            else if (direction == "left")
                snakeBodies[0] = new Point(snakeBodies[0].X - 1, snakeBodies[0].Y);
            else if (direction == "null")
                snakeBodies[0] = new Point(snakeBodies[0].X, snakeBodies[0].Y);

            if (direction != "null")
            {
                if ((snakeBodies[0].X < 0 || snakeBodies[0].X > mapSize - 1 || snakeBodies[0].Y < 0 || snakeBodies[0].Y > mapSize - 1))
                {
                    stop();
                    MessageBox.Show("VESZTETTÉL XAXAXAXA");
                    return;
                }
            }
            teliNegyzet(snakeBodies[0].X, snakeBodies[0].Y, Color.Black);

            if (snakeBodies[0] == alma)
            {
                snakeBodies.Add(new Point(0, 0));
                alma = new Point(rnd.Next(0, mapSize), rnd.Next(0, mapSize));
                teliNegyzet(alma.X, alma.Y, Color.Red);
            }

            for (int i = snakeBodies.Count - 1; i > 1; i--)
            {
                if (snakeBodies[i] == snakeBodies[0])
                {
                    stop();
                    MessageBox.Show("rip");
                    break;
                }
            }

            pictureBox1.Image = kép;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && Convert.ToInt32(textBox1.Text) != 0)
            {
                timer1.Interval = Convert.ToInt32(textBox1.Text);
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text != "" && Convert.ToInt32(textBox2.Text) != 0)
            {
                mapSize = Convert.ToInt32(textBox2.Text);
                mapUnit = 500 / mapSize;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            clearBoard();
            pictureBox1.Image = kép;
        }
    }
}
