using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Desktop_Balls
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        System.Windows.Forms.NotifyIcon notifyIcon;
        Dictionary<Ball, Ellipse> BallElipseDictionary;
        DispatcherTimer timer;
        System.Windows.Forms.MenuItem colisionMode;

        #region my own methods

        void CreateNotifyIcon()
        {
            notifyIcon = new System.Windows.Forms.NotifyIcon();
            notifyIcon.Icon = Properties.Resources.Ball;
            notifyIcon.Visible = true;
            notifyIcon.DoubleClick += notifyIcon_DoubleClick;
            notifyIcon.Text = "Desktop Balls";

            colisionMode = new System.Windows.Forms.MenuItem("Colision mode", ColisionModeChange);
            colisionMode.Checked = true;
            notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu(new System.Windows.Forms.MenuItem[]
            {
                colisionMode,
                new System.Windows.Forms.MenuItem("Add Ball", Add_Ball),
                new System.Windows.Forms.MenuItem("Add a few Balls", Add_FewBalls),
                new System.Windows.Forms.MenuItem("Mind Off Mode", Add_LotBalls),
                new System.Windows.Forms.MenuItem("Increase Speed", Increase_Speed),
                new System.Windows.Forms.MenuItem("Decease Speed", Decrese_Speed),
                new System.Windows.Forms.MenuItem("Remove Balls",Remove_Balls),
                new System.Windows.Forms.MenuItem("Exit", Exit_Click) });
        }

        void SetEllipseProperties(Ball ball, Ellipse ellipse)
        {
            ellipse.VerticalAlignment = VerticalAlignment.Top;
            ellipse.HorizontalAlignment = HorizontalAlignment.Left;
            ellipse.Height = ball.Size;
            ellipse.Width = ball.Size;
            ellipse.Margin = new Thickness(ball.CoordinateX, ball.CoordinateY, 0, 0);

            if (ball.Color == Colors.Black) ellipse.Fill = new RadialGradientBrush(Color.FromRgb(34, 34, 34), Color.FromRgb(0, 0, 0));
            if (ball.Color == Colors.White) ellipse.Fill = new RadialGradientBrush(Color.FromRgb(255, 255, 255), Color.FromRgb(0, 0, 0));
            if (ball.Color == Colors.Red) ellipse.Fill = new RadialGradientBrush(Color.FromRgb(255, 0, 0), Color.FromRgb(0, 0, 0));
            if (ball.Color == Colors.Green) ellipse.Fill = new RadialGradientBrush(Color.FromRgb(0, 255, 0), Color.FromRgb(0, 0, 0));
            if (ball.Color == Colors.Yellow) ellipse.Fill = new RadialGradientBrush(Color.FromRgb(255, 255, 0), Color.FromRgb(0, 0, 0));
            if (ball.Color == Colors.Blue) ellipse.Fill = new RadialGradientBrush(Color.FromRgb(0, 0, 255), Color.FromRgb(0, 0, 0));
        }

        private void CheckColision(Ball b1, Ball b2)
        {

            double x1 = b1.CoordinateX + (b1.Size / 2);
            double x2 = b2.CoordinateX + (b2.Size / 2);
            double y1 = b1.CoordinateY + (b1.Size / 2);
            double y2 = b2.CoordinateY + (b2.Size / 2);
            double distance = Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));

            if (distance <= (b1.Size / 2 + b2.Size / 2) && distance > 10)
            {

                OnColision(b1, b2);
                double cons = ((b1.Size / 2 + b2.Size / 2) - distance);

                if (x2 - x1 > 0)
                {
                    b2.CoordinateX += cons;
                    b1.CoordinateX -= cons;
                }
                else
                {
                    b2.CoordinateX -= cons;
                    b1.CoordinateX += cons;
                }
                if (y2 - y1 > 0)
                {
                    b2.CoordinateY += cons;
                    b1.CoordinateY -= cons;
                }
                else
                {
                    b2.CoordinateY -= cons;
                    b1.CoordinateY += cons;
                }
            }
        }


        private void CheckColisions()
        {
            for (int i = 0; i < BallElipseDictionary.Count - 1; i++)
            {
                for (int j = i + 1; j < BallElipseDictionary.Count; j++) CheckColision(BallElipseDictionary.Keys.ElementAt(i), BallElipseDictionary.Keys.ElementAt(j));
            }
        }


        void OnColision(Ball b1, Ball b2) 
        {
            double m21, dvx2, a, xd, yd, vxd, vyd, fy21, sign;

            m21 = b2.Weight / b1.Weight;
            xd = b2.CoordinateX - b1.CoordinateX;
            yd = b2.CoordinateY - b1.CoordinateY;
            vxd = b2.XSpeed - b1.XSpeed;
            vyd = b2.YSpeed - b1.YSpeed;

            fy21 = 1.0E-12 * Math.Abs(yd);

            if (Math.Abs(xd) < fy21)
            {
                if (xd < 0) { sign = -1; } else { sign = 1; }
                xd = fy21 * sign;
            }

            a = yd / xd;
            dvx2 = -2 * (vxd + a * vyd) / ((1 + a * a) * (1 + m21));
            b2.XSpeed = b2.XSpeed + dvx2;
            b2.YSpeed = b2.YSpeed + a * dvx2;
            b1.XSpeed = b1.XSpeed - m21 * dvx2;
            b1.YSpeed = b1.YSpeed - a * m21 * dvx2;

        }

        #endregion



        public MainWindow()
        {
            InitializeComponent();
            CreateNotifyIcon();
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 20);
            timer.Tick += new EventHandler(timer_Tick);
            BallElipseDictionary = new Dictionary<Ball, Ellipse>();
            timer.Start();

        }

        #region count of balls control event methods

        private void Add_Ball(object sender, EventArgs e)
        {
            if (BallElipseDictionary.Count < 150)
            {
                Thread.Sleep(30);
                Ball newBall = new Ball((int)Width, (int)Height);
                Ellipse newEllipse = new Ellipse();
                SetEllipseProperties(newBall, newEllipse);
                MainGrid.Children.Add(newEllipse);
                BallElipseDictionary.Add(newBall, newEllipse);
            }
        }

        private void Remove_Balls(object sender, EventArgs e)
        {
            foreach (var ellipse in BallElipseDictionary.Values)
            {
                MainGrid.Children.Remove(ellipse);
            }
            BallElipseDictionary.Clear();

        }

        private void Add_LotBalls(object sender, EventArgs e)
        {

            for (int i = 0; i < 50; i++)
            {
                Add_Ball(null, null);
            }

        }

        private void Add_FewBalls(object sender, EventArgs e)
        {

            for (int i = 0; i < 10; i++)
            {
                Add_Ball(null, null);
            }

        }

        #endregion

        #region balls speed controls event methods

        private void Decrese_Speed(object sender, EventArgs e)
        {
            foreach (var ball in BallElipseDictionary.Keys)
            {
                ball.DecreaseSpeed();
            }
        }

        private void Increase_Speed(object sender, EventArgs e)
        {
            foreach (var ball in BallElipseDictionary.Keys)
            {
                ball.IncreaseSpeed();
            }
        }
        #endregion


        private void ColisionModeChange(object sender, EventArgs e) 
        {
            (sender as System.Windows.Forms.MenuItem).Checked = !(sender as System.Windows.Forms.MenuItem).Checked;
        }

        private void timer_Tick(object sender, EventArgs e)
        {

            foreach (var BallElipsePair in BallElipseDictionary)
            {
                BallElipsePair.Key.UpdatePosition();
                BallElipsePair.Value.Margin = new Thickness(BallElipsePair.Key.CoordinateX, BallElipsePair.Key.CoordinateY, 0, 0);
            }

            foreach (var ball in BallElipseDictionary.Keys)
            {
                if (ball.CoordinateX <= 0 || ball.CoordinateX + ball.Size >= Width)
                {
                    ball.XSpeed = -ball.XSpeed;

                    if (ball.CoordinateX <= 0) ball.CoordinateX = 0;
                    else ball.CoordinateX = Width - ball.Size;
                }
                if (ball.CoordinateY <= 0 || ball.CoordinateY + ball.Size >= Height)
                {
                    ball.YSpeed = -ball.YSpeed;
                    if (ball.CoordinateY <= 0) ball.CoordinateY = 0;
                    else ball.CoordinateY = Height - ball.Size;
                }
            }

            if (colisionMode.Checked) CheckColisions();



        }

        private void Exit_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            Activate();
        }

        





    }
}
