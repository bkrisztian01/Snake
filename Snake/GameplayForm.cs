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
using System.IO;
using System.Xml;
using System.Xml.Serialization;


namespace Snake
{
    public partial class GameplayForm : Form
    {
        PlayZone map = new PlayZone();
        List<Snake> snake = new List<Snake>();
        bool started = false;
        Random rnd1 = new Random();
        List<Apple> appleList = new List<Apple>();
        SoundPlayer endSound = new SoundPlayer("Mario Dying Sound.wav");
        public int appleCount = 5;
        
        public GameplayForm()
        {
            InitializeComponent();

            map.mapSize = Convert.ToInt32(textBox2.Text);
            map.mapUnit = 500 / map.mapSize;
            timer1.Interval = Convert.ToInt32(textBox1.Text);

            PlayerCountForm form = new PlayerCountForm();
            form.ShowDialog();
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
                for (int i = 0; i < Convert.ToInt32(Config.Instance.Get("PlayerCount")); i++)
                {
                    if (snake[i].controls.ContainsKey(keyData))
                        snake[i].directionBuffer = snake[i].controls[keyData];

                }
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
                    //endSound.Play();
                    MessageBox.Show("Player "+ (i + 1) + " is dead");
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

                for (int j = 0; j < appleList.Count; j++)
                {
                    if (snake[i].headCollide(appleList[j]))
                    {
                        appleList[j].action(snake[i], snake);
                        appleList.RemoveAt(j);
                        generateRandomApple();

                        repositionApple(appleList[appleCount - 1]);
                    }
                }

                //if (snake[Math.Abs(i-2+1)].bodyCollide(snake[i].bodyparts[0]))
                //{
                //    snake[Math.Abs(i - 2 + 1)].isDead = true;
                //    //endSound.Play();
                //}

                for (int j = 0; j < snake.Count; j++)
                {
                    if (snake[j].bodyCollide(snake[i].bodyparts[0]) && i != j)
                    {
                        snake[i].isDead = true;
                        //endSound.Play();
                    }
                }
            }

            for (int i = 0; i < appleList.Count; i++)
            {
                appleList[i].draw(map);
            }

            pictureBox1.Image = map.kép;
        }

        public void start()
        {
            map.clearBoard();
            pictureBox1.Image = map.kép;

            snake.Clear();

            string[] directions = new string[]
            {
                "Up", "Down", "Left", "Right"
            };

            for (int i = 0; i < Convert.ToInt32(Config.Instance.Get("PlayerCount")); i++)
            {
                Dictionary<Keys, Vector> keyboardMapping = new Dictionary<Keys, Vector>();
                for (int j = 0; j < directions.Length; j++)
                {
                    if (Config.Instance.IDExists(directions[j] + (i + 1)) && Config.Instance.Get(directions[j] + (i + 1)) != "")
                    {

                        keyboardMapping.Add((Keys)Enum.Parse(typeof(Keys), Config.Instance.Get(directions[j] + (i + 1))), new Vector(directions[j]));
                    }
                    else
                    {
                        MessageBox.Show("Keybind " + (directions[j] + (i + 1)) + " was not set!", directions[j] + (i + 1));
                        return;
                    }
                }
                if (Config.Instance.IDExists("Color" + (i + 1)))
                {
                    snake.Add(new Snake(map, new Vector(rnd1.Next(map.mapSize), rnd1.Next(map.mapSize)), Color.FromArgb(Convert.ToInt32(Config.Instance.Get("Color" + (i + 1)))), keyboardMapping));
                }
                else
                {
                    MessageBox.Show("Color" + (i + 1) + " was not set!");
                    return;
                }
            }

            appleCount = Convert.ToInt32(Config.Instance.Get("AppleCount"));

            appleList.Clear();
            for (int i = 0; i < appleCount; i++)
            {
                generateRandomApple();
            }

            drawElements();

            for (int l = 0; l < appleList.Count; l++)
            {
                repositionApple(appleList[l]);
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

        public void repositionApple(Apple apple)
        {
            var temp = map.kép.GetPixel(apple.X * map.mapUnit + 1, apple.Y * map.mapUnit + 1);
            for (int i = 0; i < appleList.Count; i++)
            {
                while (!map.kép.GetPixel(apple.X * map.mapUnit + 1, apple.Y * map.mapUnit + 1).Equals(Color.White) && appleList[i].equals(apple) && appleList[i] != apple)
                {
                    map.teliNegyzet(apple.X, apple.Y, Color.White);
                    map.uresNegyzet(apple.X, apple.Y);
                    apple.X = rnd1.Next(map.mapSize);
                    apple.Y = rnd1.Next(map.mapSize);
                    map.teliNegyzet(apple.X, apple.Y, apple.color);
                }
            }
        }

        public void generateRandomApple()
        {
            List<Type> appleTypes = typeof(Apple).Assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(Apple))).ToList();
            List<int> appleChances = new List<int>();
            for (int i = 0; i < appleTypes.Count; i++)
            {
                appleChances.Add(Convert.ToInt32(Config.Instance.Get(appleTypes[i].ToString().Substring(6) + "Chance")));
            }

            float rndNumber = rnd1.Next(1, Convert.ToInt32(appleChances.Sum()));

            int appleChanceCounter = 0;
            for (int i = 0; i < appleTypes.Count; i++)
            {
                appleChanceCounter += appleChances[i];
                if(appleChanceCounter >= rndNumber)
                {
                    appleList.Add((Apple)Activator.CreateInstance(appleTypes[i], new Vector(rnd1.Next(map.mapSize), rnd1.Next(map.mapSize))));
                    break;
                }
            }
        }

        public void drawElements()
        {
            for (int i = 0; i < snake.Count; i++)
            {
                snake[i].snakeRefresh(map);
            }
            for (int i = 0; i < appleList.Count; i++)
            {
                map.teliNegyzet(appleList[i].X, appleList[i].Y, appleList[i].color);
            }
        }
        
        private void playerColorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorSelectForm frm = new ColorSelectForm();
            frm.ShowDialog();
        }

        private void appleSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AppleSettingsForm frm = new AppleSettingsForm();
            frm.ShowDialog();
        }

        private void controlsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ControlSettingsForm frm = new ControlSettingsForm();
            frm.ShowDialog();
        }
    }
}
