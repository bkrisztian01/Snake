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
    class BlackApple : Apple
    {
        public BlackApple(Vector position) : base(Color.Black, position, "18V_Cordless_Drill_Switch.wav")
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
}