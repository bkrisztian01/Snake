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
        public Dictionary<Keys, Vector> controls = new Dictionary<Keys, Vector>();
        List<Vector> removedBpsAt = new List<Vector>();

        public Snake(PlayZone map, Vector startingVector, Vector direction, Color color, Dictionary<Keys, Vector> controls)
        {
            for (int i = 0; i < 3; i++)
            {
                addPart(new Vector(startingVector.X - direction.X * i, startingVector.Y - direction.Y * i));
                map.teliNegyzet(bodyparts[i].X, bodyparts[i].Y, color);
            }
            isDead = false;
            this.color = color;
            this.direction = direction;
            this.controls = controls;
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
}