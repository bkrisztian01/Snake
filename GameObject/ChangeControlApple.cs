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
    class ChangeControlApple : Apple
    {
        public ChangeControlApple(Vector position) : base(Color.Purple, position, String.Empty)
        {
        }

        override public void action(Snake snake, List<Snake> enemy)
        {
            var tempEnemyControls = enemy[0].controls;
            var tempEnemyColor = enemy[0].color;
            for (int i = 0; i < enemy.Count; i++)
            {
                if (i == enemy.Count - 1)
                {
                    enemy[i].controls = tempEnemyControls;
                    enemy[i].color = tempEnemyColor;
                }

                else
                {
                    enemy[i].controls = enemy[i + 1].controls;
                    enemy[i].color = enemy[i + 1].color;
                }
            }

            var tempSnake = enemy[enemy.Count - 1];
            enemy.RemoveAt(enemy.Count - 1);
            enemy.Insert(0, tempSnake);

            base.action(snake, enemy);
        }
    }
}