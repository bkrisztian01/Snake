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
    class DoubleGrowApple : Apple
    {
        public DoubleGrowApple(Vector position) : base(Color.Gold, position, "Air_Woosh_Underwater.wav")
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
}