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
    class GrowApple : Apple
    {
        public GrowApple(Vector position) : base(Color.Red, position, "Snake.Sounds.growSound.wav")
        {
        }

        override public void action(Snake snake, List<Snake> enemy)
        {
            snake.grow();
            base.action(snake, enemy);
        }
    }
}