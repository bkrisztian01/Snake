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
    public abstract class Apple : Vector
    {
        public Color color;
        SoundPlayer eatSound;

        public Apple(Color color, Vector position, string soundPath)
        {
            //TODO: Sounds
            //eatSound = new SoundPlayer(soundPath);
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
            //try
            //{
            //    eatSound.Play();
            //}
            //catch (Exception e)
            //{

            //}
        }
    }
}