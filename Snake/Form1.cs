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
        PlayZone map = new PlayZone();
        Snake snake;
        bool started = false;
        Random rnd1 = new Random();
        Dictionary<Keys, Vector> keyboardMapping = new Dictionary<Keys, Vector>();
        Vector alma;

        public Form1()
        {
            InitializeComponent();
            map.mapSize = Convert.ToInt32(textBox2.Text);
            map.mapUnit = 500 / map.mapSize;
            keyboardMapping.Add(Keys.Up, new Vector("up"));
            keyboardMapping.Add(Keys.Down, new Vector("down"));
            keyboardMapping.Add(Keys.Left, new Vector("left"));
            keyboardMapping.Add(Keys.Right, new Vector("right"));
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
                map.mapSize = Convert.ToInt32(textBox2.Text);
                map.mapUnit = 500 / map.mapSize;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) //Irányítás
        {
            if (keyboardMapping.ContainsKey(keyData))
                snake.changeDirection(keyboardMapping[keyData]);
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!started)
                start();

            else
                stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (snake.isDead)
            {
                stop();
                MessageBox.Show("Vesztettel");
                return;
            }
            snake.moveParts(map, ref alma);
            if (snake.direction.X != 0 || snake.direction.Y != 0)
            {
                if (snake.bodyCollide(snake.bodyparts[0]) || snake.bodyparts[0].X < 0 || snake.bodyparts[0].Y < 0 || snake.bodyparts[0].X > map.mapSize - 1 || snake.bodyparts[0].Y > map.mapSize - 1)
                {
                    snake.isDead = true;
                }
            }

            if (!snake.isDead)
            {
                snake.snakeRefresh(map);
            }

            label3.Text = "Score: " + Convert.ToInt32(snake.score);

            if (snake.headCollide(alma))
            {
                snake.grow();
                alma.X = rnd1.Next(map.mapSize);
                alma.Y = rnd1.Next(map.mapSize);
                snake.score++;
            }

            map.teliNegyzet(alma.X, alma.Y, Color.Red);

            pictureBox1.Image = map.kép;
        }

        public void start()
        {
            map.clearBoard();
            label3.Text = "Score: 0";
            pictureBox1.Image = map.kép;
            snake = new Snake(map, new Vector(5, 5), new Vector(0, 0));
            snake.score = 0;
            alma = new Vector(rnd1.Next(map.mapSize), rnd1.Next(map.mapSize));
            for (int i = 0; i < snake.bodyparts.Count; i++)
            {
                if (alma.X == snake.bodyparts[i].X && alma.Y == snake.bodyparts[i].Y)
                {
                    alma = new Vector(rnd1.Next(map.mapSize), rnd1.Next(map.mapSize));
                    i = 0;
                }
            }
            timer1.Start();
            started = !started;
            textBox2.Enabled = false;
            map.teliNegyzet(alma.X, alma.Y, Color.Red);
            button1.Text = "Stop";
        }

        public void stop()
        {
            timer1.Stop();
            started = false;
            button1.Text = "Start";
            textBox2.Enabled = true;
        }        
    }

    class PlayZone
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

    class Snake
    {
        public List<Vector> bodyparts = new List<Vector>();
        public Vector direction;
        public bool isDead;
        private Vector utolso = new Vector();
        public int score;
        Random rnd = new Random();

        public Snake(PlayZone map, Vector startingVector, Vector direction)
        {
            for (int i = 0; i < 3; i++)
            {
                addPart(new Vector(startingVector.X - direction.X * i, startingVector.Y - direction.Y * i));
                map.teliNegyzet(bodyparts[i].X, bodyparts[i].Y, Color.Black);
            }
            isDead = false;
            this.direction = direction;
        }

        public void snakeRefresh(PlayZone map)
        {
            if (!utolso.equals(bodyparts[bodyparts.Count - 1]))
            {
                map.teliNegyzet(utolso.X, utolso.Y, Color.White);
                map.uresNegyzet(utolso.X, utolso.Y);
            }
            map.teliNegyzet(bodyparts[0].X, bodyparts[0].Y, Color.Black);
        }

        public void moveParts(PlayZone map, ref Vector alma)
        {
            utolso.X = bodyparts[bodyparts.Count - 1].X;
            utolso.Y = bodyparts[bodyparts.Count - 1].Y;

            for (int i = bodyparts.Count - 1; i > 0; i--)
            {
                bodyparts[i].X = bodyparts[i - 1].X;
                bodyparts[i].Y = bodyparts[i - 1].Y;
            }

            bodyparts[0].add(direction);
        }

        public void addPart(Vector Vector)
        {
            bodyparts.Add(Vector);
        }

        public void changeDirection(Vector direction)
        {
            if (this.direction.X + direction.X == 0 && this.direction.Y + direction.Y == 0)
            {
                return;
            }
            this.direction.X = direction.X;
            this.direction.Y = direction.Y;
        }
        
        public bool headCollide(Vector vektor)
        {
            if (bodyparts[0].X == vektor.X && bodyparts[0].Y == vektor.Y)
            {
                return true;
            }
            return false;
        }

        public bool bodyCollide(Vector vektor)
        {
            for (int i = 1; i < bodyparts.Count; i++)
            {
                if (bodyparts[i].X == vektor.X && bodyparts[i].Y == vektor.Y)
                {
                    return true;
                }
            }
            return false;
        }

        public void grow()
        {
            addPart(new Vector(bodyparts[bodyparts.Count - 1].X, bodyparts[bodyparts.Count - 1].Y));
        }
    }

    class Vector
    {
        public int X;
        public int Y;

        public Vector() { }
        public void add(Vector vektor)
        {
            X += vektor.X;
            Y += vektor.Y;
        }
        public void sub(Vector vektor)
        {
            X -= vektor.X;
            Y -= vektor.Y;
        }
        public Vector(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }
        public Vector(string direction)
        {
            if (direction == "null")
            {
                X = 0;
                Y = 0;
            }
            else if (direction == "up")
            {
                X = 0;
                Y = -1;
            }
            else if (direction == "down")
            {
                X = 0;
                Y = 1;
            }
            else if (direction == "left")
            {
                X = -1;
                Y = 0;
            }
            else if (direction == "right")
            {
                X = 1;
                Y = 0;
            }
        }

        public bool equals(Vector vektor)
        {
            return (vektor.X == X && vektor.Y == Y);
        }
    }
}
