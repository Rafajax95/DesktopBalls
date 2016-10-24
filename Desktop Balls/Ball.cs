using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Desktop_Balls
{
    class Ball
    {
        private Random random;
        public double CoordinateX { get; set; }
        public double CoordinateY { get; set; }
        public Colors Color { get; set; }
        public double Size { get; set; }
        public double XSpeed { get; set; }
        public double YSpeed { get; set; }
        public double Weight { get; set; }


        public Ball(double MaxCoordinateX, double MaxCoordinateY)
        {
            random = new Random();
            ChooseCoordinates(MaxCoordinateX, MaxCoordinateY);
            ChooseSize();
            Weight = 3.14 * Math.Pow(Size / 2, 2);
            
            ChooseColor();
            ChooseSpeed();
            
        }

        private void ChooseCoordinates(double MaxCoordinateX, double MaxCoordinateY)
        {
            CoordinateX = random.Next(1, (int)MaxCoordinateX - 101);
            CoordinateY = random.Next(1, (int)MaxCoordinateY - 101);

        }

        private void ChooseSize()
        {
            this.Size = random.Next(50, 100);
        }

        private void ChooseColor()
        {
            this.Color = (Colors)random.Next(0, 6);
        }

        private void ChooseSpeed()
        {
            this.XSpeed = random.Next(-3, 3);
            this.YSpeed = random.Next(-3, 3);
        }

        public void UpdatePosition()
        {
            CoordinateX += XSpeed;
            CoordinateY += YSpeed;
        }

        public void IncreaseSpeed()
        {
            XSpeed *= 1.5;
            YSpeed *= 1.5;
        }
        public void DecreaseSpeed()
        {
            XSpeed /= 1.5;
            YSpeed /= 1.5;
        }
    }

    public enum Colors
    {
        Black,
        Red,
        Green,
        Yellow,
        White,
        Blue,
    }
}
