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
}