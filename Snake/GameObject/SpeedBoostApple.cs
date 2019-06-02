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
    class SpeedBoostApple : Apple
    {
        public SpeedBoostApple(Vector position) : base(Color.LightPink, position, "Super Mario Bros.-Coin Sound Effect.wav")
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