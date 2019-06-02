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
    public partial class Form1 : Form
    {
        PlayZone map = new PlayZone();
        List<Snake> snake = new List<Snake>();
        bool started = false;
        Random rnd1 = new Random();
        Dictionary<Keys, Vector> keyboardMapping1 = new Dictionary<Keys, Vector>();
        Dictionary<Keys, Vector> keyboardMapping2 = new Dictionary<Keys, Vector>();
        List<Alma> almalist = new List<Alma>();
        SoundPlayer endSound = new SoundPlayer("Mario Dying Sound.wav");
        public Color player1 = Color.Blue;
        public Color player2 = Color.Green;

        public Form1()
        {
            InitializeComponent();
            map.mapSize = Convert.ToInt32(textBox2.Text);
            map.mapUnit = 500 / map.mapSize;
            timer1.Interval = Convert.ToInt32(textBox1.Text);
            //Player 1
            keyboardMapping1.Add(Keys.W, new Vector("up"));
            keyboardMapping1.Add(Keys.S, new Vector("down"));
            keyboardMapping1.Add(Keys.A, new Vector("left"));
            keyboardMapping1.Add(Keys.D, new Vector("right"));
            //Player 2
            keyboardMapping2.Add(Keys.Up, new Vector("up"));
            keyboardMapping2.Add(Keys.Down, new Vector("down"));
            keyboardMapping2.Add(Keys.Left, new Vector("left"));
            keyboardMapping2.Add(Keys.Right, new Vector("right"));
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            int interval;
            bool result = Int32.TryParse(textBox1.Text, out interval);
            if (result)
            {
                timer1.Interval = Convert.ToInt32(interval);
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            int mapSize;
            bool result = Int32.TryParse(textBox2.Text, out mapSize);
            if (result)
            {
                map.mapSize = mapSize;
                map.mapUnit = 500 / mapSize;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) //Irányítás
        {
            if (started)
            {
                if (keyboardMapping1.ContainsKey(keyData))
                    snake[0].directionBuffer = keyboardMapping1[keyData];

                if (keyboardMapping2.ContainsKey(keyData))
                    snake[1].directionBuffer = keyboardMapping2[keyData];
            }
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
            for (int i = 0; i < snake.Count; i++)
            {
                if (snake[i].isDead)
                {
                    stop();
                    endSound.Play();
                    MessageBox.Show("Player " + (i + 1) + " meghalt");
                    return;
                }

                snake[i].moveParts(map, snake[i].directionBuffer);

                if (snake[i].direction.X != 0 || snake[i].direction.Y != 0)
                {
                    if (snake[i].bodyCollide(snake[i].bodyparts[0]) || snake[i].bodyparts[0].X < 0 || snake[i].bodyparts[0].Y < 0 || snake[i].bodyparts[0].X > map.mapSize - 1 || snake[i].bodyparts[0].Y > map.mapSize - 1)
                    {
                        snake[i].isDead = true;
                    }
                }

                if (!snake[i].isDead)
                {
                    snake[i].snakeRefresh(map);
                }

                for (int j = 0; j < almalist.Count; j++)
                {
                    if (snake[i].headCollide(almalist[j]))
                    {
                        almalist[j].action(snake[i], snake);
                        almalist[j].X = rnd1.Next(map.mapSize);
                        almalist[j].Y = rnd1.Next(map.mapSize);

                        repositionAlma(almalist[j]);
                    }
                }

                if (snake[Math.Abs(i-2+1)].bodyCollide(snake[i].bodyparts[0]))
                {
                    snake[Math.Abs(i - 2 + 1)].isDead = true;
                    endSound.Play();
                }
            }

            label3.Text = "Player1: " + Convert.ToInt32(snake[0].score);
            label4.Text = "Player2: " + Convert.ToInt32(snake[1].score);

            for (int i = 0; i < almalist.Count; i++)
            {
                almalist[i].draw(map);
            }

            pictureBox1.Image = map.kép;
        }

        public void start()
        {
            map.clearBoard();
            pictureBox1.Image = map.kép;

            snake.Clear();
            snake.Add(new Snake(map, new Vector(2, 5), new Vector(0, 0), player1));
            snake.Add(new Snake(map, new Vector(map.mapSize - 2, 5), new Vector(0, 0), player2));

            almalist.Clear();
            almalist.Add(new GrowAlma(new Vector(rnd1.Next(map.mapSize), rnd1.Next(map.mapSize))));
            almalist.Add(new BlackAlma(new Vector(rnd1.Next(map.mapSize), rnd1.Next(map.mapSize))));
            almalist.Add(new DoubleGrowAlma(new Vector(rnd1.Next(map.mapSize), rnd1.Next(map.mapSize))));
            almalist.Add(new SpeedBoostAlma(new Vector(rnd1.Next(map.mapSize), rnd1.Next(map.mapSize))));

            drawElements();

            for (int l = 0; l < almalist.Count; l++)
            {
                repositionAlma(almalist[l]);
            }

            timer1.Start();
            pictureBox1.Visible = true;
            started = !started;
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            button1.Text = "Stop";
        }

        public void stop()
        {
            timer1.Stop();
            started = false;
            button1.Text = "Start";
            textBox1.Enabled = true;
            textBox2.Enabled = true;
        }
        
        private void szinToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 frm = new Form2(this);
            frm.Show();
        }

        public void repositionAlma(Alma alma)
        {
            var temp = map.kép.GetPixel(alma.X * map.mapUnit + 1, alma.Y * map.mapUnit + 1);
            for (int i = 0; i < almalist.Count; i++)
            {
                while (!map.kép.GetPixel(alma.X * map.mapUnit + 1, alma.Y * map.mapUnit + 1).Equals(Color.White) && almalist[i].equals(alma) && almalist[i] != alma)
                {
                    map.teliNegyzet(alma.X, alma.Y, Color.White);
                    map.uresNegyzet(alma.X, alma.Y);
                    alma.X = rnd1.Next(map.mapSize);
                    alma.Y = rnd1.Next(map.mapSize);
                    map.teliNegyzet(alma.X, alma.Y, alma.color);
                }
            }
        }

        public void drawElements()
        {
            for (int i = 0; i < snake.Count; i++)
            {
                snake[i].snakeRefresh(map);
            }
            for (int i = 0; i < almalist.Count; i++)
            {
                map.teliNegyzet(almalist[i].X, almalist[i].Y, almalist[i].color);
            }
        }
    }

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

    public class Snake
    {
        public List<Vector> bodyparts = new List<Vector>();
        public Vector direction;
        public bool isDead;
        private Vector utolso = new Vector();
        public int score = 0;
        public Color color;
        Random rnd = new Random();
        public int velocity = 1;
        public int ticksToNextMove = 2;
        public int speedBoost = 0;
        public Vector directionBuffer = new Vector("null");
        List<Vector> removedBpsAt = new List<Vector>();

        public Snake(PlayZone map, Vector startingVector, Vector direction, Color color)
        {
            for (int i = 0; i < 3; i++)
            {
                addPart(new Vector(startingVector.X - direction.X * i, startingVector.Y - direction.Y * i));
                map.teliNegyzet(bodyparts[i].X, bodyparts[i].Y, color);
            }
            isDead = false;
            this.color = color;
            this.direction = direction;
        }

        public void snakeRefresh(PlayZone map)
        {
            if (!utolso.equals(bodyparts[bodyparts.Count - 1]))
            {
                map.teliNegyzet(utolso.X, utolso.Y, Color.White);
                map.uresNegyzet(utolso.X, utolso.Y);
            }
            foreach(Vector element in removedBpsAt)
            {
                map.teliNegyzet(element.X, element.Y, Color.White);
                map.uresNegyzet(element.X, element.Y);
            }
            removedBpsAt.Clear();
            map.teliNegyzet(bodyparts[0].X, bodyparts[0].Y, color);
        }

        public void moveParts(PlayZone map, Vector directionBuffer)
        {
            ticksToNextMove -= velocity;
            if (ticksToNextMove <= 0)
            {
                changeDirection(directionBuffer);
                utolso.X = bodyparts[bodyparts.Count - 1].X;
                utolso.Y = bodyparts[bodyparts.Count - 1].Y;

                for (int i = bodyparts.Count - 1; i > 0; i--)
                {
                    bodyparts[i].X = bodyparts[i - 1].X;
                    bodyparts[i].Y = bodyparts[i - 1].Y;
                }

                bodyparts[0].add(direction);
                ticksToNextMove = 2;
                speedBoost--;

            }

            if (speedBoost <= 0)
            {
                velocity = 1;
            }
        }

        public void addPart(Vector Vector)
        {
            bodyparts.Add(Vector);
        }

        public void changeDirection(Vector direction)
        {
            if (bodyparts[0].X - bodyparts[1].X + direction.X == 0 && bodyparts[0].Y - bodyparts[1].Y + direction.Y == 0)
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

        public void removeBodypart()
        {
            if (bodyparts.Count > 2)
            {
                removedBpsAt.Add(bodyparts[bodyparts.Count - 1]);
                bodyparts.RemoveAt(bodyparts.Count - 1);
            }
        }
    }

    public class Vector
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

        public void setVector(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }
    }

    public abstract class Alma : Vector
    {
        public Color color;
        SoundPlayer almaSound;

        public Alma(Color color, Vector position, string soundPath)
        {
            almaSound = new SoundPlayer(soundPath);
            this.color = color;
            X = position.X;
            Y = position.Y;
        }

        public void draw(PlayZone map)
        {
            map.teliNegyzet(X, Y, color);
        }

        virtual public void action(Snake snake, List<Snake> enemy)
        {
            almaSound.Play();
        }
    }

    class GrowAlma : Alma
    {
        public GrowAlma(Vector position) : base(Color.Red, position, "Air_Woosh_Underwater.wav")
        {
        }

        override public void action(Snake snake, List<Snake> enemy)
        {
            snake.grow();
            snake.score++;
            base.action(snake, enemy);
        }
    }

    class BlackAlma : Alma
    {
        public BlackAlma(Vector position) : base(Color.Black, position, "18V_Cordless_Drill_Switch.wav")
        {
        }

        override public void action(Snake snake, List<Snake> enemy)
        {
            foreach (Snake element in enemy)
            {
                if (element != snake)
                {
                    element.removeBodypart();
                    element.score--;
                }
            }
            base.action(snake, enemy);
        }
    }

    class DoubleGrowAlma : Alma
    {
        public DoubleGrowAlma(Vector position) : base(Color.Gold, position, "Air_Woosh_Underwater.wav")
        {
        }

        override public void action(Snake snake, List<Snake> enemy)
        {
            snake.grow();
            snake.grow();
            snake.score += 2;
            base.action(snake, enemy);
        }
    }

    class SpeedBoostAlma : Alma
    {
        public SpeedBoostAlma(Vector position) : base(Color.LightPink, position, "Super Mario Bros.-Coin Sound Effect.wav")
        {
        }

        override public void action(Snake snake, List<Snake> enemy)
        {
            snake.speedBoost = 50;
            snake.velocity = 2;
            base.action(snake, enemy);
        }
    }
}
