﻿using System;
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
    public partial class GameplayForm : Form
    {
        PlayZone map = new PlayZone();
        List<Snake> snake = new List<Snake>();
        bool started = false;
        Random rnd1 = new Random();
        List<Apple> applelist = new List<Apple>();
        SoundPlayer endSound = new SoundPlayer("Mario Dying Sound.wav");
        public Color player1 = Color.Blue;
        public Color player2 = Color.Green;
        public int appleCount = 5;
        public int growAppleChance = 20;
        public int doubleGrowAppleChance = 20;
        public int changeControlAppleChance = 20;
        public int speedBoostAppleChance = 20;
        public int blackAppleChance = 20;


        public GameplayForm()
        {
            InitializeComponent();
            map.mapSize = Convert.ToInt32(textBox2.Text);
            map.mapUnit = 500 / map.mapSize;
            timer1.Interval = Convert.ToInt32(textBox1.Text);
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
                if (snake[0].controls.ContainsKey(keyData))
                    snake[0].directionBuffer = snake[0].controls[keyData];

                if (snake[1].controls.ContainsKey(keyData))
                    snake[1].directionBuffer = snake[1].controls[keyData];
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
                    MessageBox.Show("Player " + (i + 1) + " is dead");
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

                for (int j = 0; j < applelist.Count; j++)
                {
                    if (snake[i].headCollide(applelist[j]))
                    {
                        applelist[j].action(snake[i], snake);
                        applelist.RemoveAt(j);
                        generateRandomApple();

                        repositionApple(applelist[appleCount - 1]);
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

            label3.Text = "Player1: " + Convert.ToInt32(snake[0].score);
            label4.Text = "Player2: " + Convert.ToInt32(snake[1].score);

            for (int i = 0; i < applelist.Count; i++)
            {
                applelist[i].draw(map);
            }

            pictureBox1.Image = map.kép;
        }

        public void start()
        {
            map.clearBoard();
            pictureBox1.Image = map.kép;
            
            Dictionary<Keys, Vector> keyboardMapping1 = new Dictionary<Keys, Vector>();
            Dictionary<Keys, Vector> keyboardMapping2 = new Dictionary<Keys, Vector>();
            
            //Player 1
            keyboardMapping1.Add(Keys.W, new Vector("up"));
            keyboardMapping1.Add(Keys.S, new Vector("down"));
            keyboardMapping1.Add(Keys.A, new Vector("left"));
            keyboardMapping1.Add(Keys.D, new Vector("right"));
            //Player 2
            keyboardMapping2.Add(Keys.I, new Vector("up"));
            keyboardMapping2.Add(Keys.K, new Vector("down"));
            keyboardMapping2.Add(Keys.J, new Vector("left"));
            keyboardMapping2.Add(Keys.L, new Vector("right"));

            snake.Clear();
            snake.Add(new Snake(map, new Vector(2, 5), new Vector(0, 0), player1, keyboardMapping1));
            snake.Add(new Snake(map, new Vector(map.mapSize - 2, 5), new Vector(0, 0), player2, keyboardMapping2));

            applelist.Clear();
            for (int i = 0; i < appleCount; i++)
            {
                generateRandomApple();
            }

            drawElements();

            for (int l = 0; l < applelist.Count; l++)
            {
                repositionApple(applelist[l]);
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
            ColorSelectForm frm = new ColorSelectForm(this);
            frm.Show();
        }

        public void repositionApple(Apple apple)
        {
            var temp = map.kép.GetPixel(apple.X * map.mapUnit + 1, apple.Y * map.mapUnit + 1);
            for (int i = 0; i < applelist.Count; i++)
            {
                while (!map.kép.GetPixel(apple.X * map.mapUnit + 1, apple.Y * map.mapUnit + 1).Equals(Color.White) && applelist[i].equals(apple) && applelist[i] != apple)
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
            int rndNumber = rnd1.Next(1, 101);

            //GrowApple
            if (rndNumber <= growAppleChance)
            {
                applelist.Add(new GrowApple(new Vector(rnd1.Next(map.mapSize), rnd1.Next(map.mapSize))));
            }
            //DoubleGrowApple
            else if (growAppleChance < rndNumber && rndNumber <= growAppleChance + doubleGrowAppleChance)
            {
                applelist.Add(new DoubleGrowApple(new Vector(rnd1.Next(map.mapSize), rnd1.Next(map.mapSize))));
            }
            //ChangeControlApple
            else if (growAppleChance + doubleGrowAppleChance < rndNumber && rndNumber <= growAppleChance + doubleGrowAppleChance + changeControlAppleChance)
            {
                applelist.Add(new ChangeControlApple(new Vector(rnd1.Next(map.mapSize), rnd1.Next(map.mapSize))));
            }
            //SpeedBoostApple
            else if (growAppleChance + doubleGrowAppleChance + changeControlAppleChance < rndNumber && rndNumber <= growAppleChance + doubleGrowAppleChance + changeControlAppleChance + speedBoostAppleChance)
            {
                applelist.Add(new SpeedBoostApple(new Vector(rnd1.Next(map.mapSize), rnd1.Next(map.mapSize))));
            }
            //BlackApple
            else if (growAppleChance + doubleGrowAppleChance + changeControlAppleChance + speedBoostAppleChance < rndNumber && rndNumber <= growAppleChance + doubleGrowAppleChance + changeControlAppleChance + speedBoostAppleChance + blackAppleChance)
            {
                applelist.Add(new BlackApple(new Vector(rnd1.Next(map.mapSize), rnd1.Next(map.mapSize))));
            }
        }

        public void drawElements()
        {
            for (int i = 0; i < snake.Count; i++)
            {
                snake[i].snakeRefresh(map);
            }
            for (int i = 0; i < applelist.Count; i++)
            {
                map.teliNegyzet(applelist[i].X, applelist[i].Y, applelist[i].color);
            }
        }

        private void appleSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AppleSettingsForm frm = new AppleSettingsForm(this);
            frm.Show();
        }
    }
}
