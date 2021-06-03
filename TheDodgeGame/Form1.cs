using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TheDodgeGame
{
    public partial class Form1 : Form
    {

        Rectangle hero = new Rectangle(50, 50, 20, 20);
        int speed = 5;
        List<Rectangle> leftColumns = new List<Rectangle>();
        List<Rectangle> rightColumns = new List<Rectangle>();
        int leftColumnSpeed = 6;
        int rightColumnSpeed = -4;

        int nextLeft = 0;
        int nextRight = 0;

        bool leftKeyDown = false;
        bool rightKeyDown = false;
        bool upKeyDown = false;
        bool downKeyDown = false;

        SolidBrush playerBrush = new SolidBrush(Color.Blue);
        SolidBrush obstacleBrush = new SolidBrush(Color.White);

        public Form1()
        {
            InitializeComponent();
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            if (leftKeyDown)
            {
                hero.X -= speed;
            }
            if (rightKeyDown)
            {
                hero.X += speed;
            }
            if (upKeyDown)
            {
                hero.Y -= speed;
            }
            if (downKeyDown)
            {
                hero.Y += speed;
            }

            hero.X = hero.X < 0 ? 0 : hero.X;
            if(hero.X + hero.Width > this.Width)
            {
                gameTimer.Enabled = false;
            }
            hero.Y = hero.Y < 0 ? 0 : (hero.Y + hero.Height > this.Height ? this.Height - hero.Height : hero.Y);

            nextLeft++;
            if(nextLeft > 17)
            {
                nextLeft = 0;
                leftColumns.Add(new Rectangle(150, -50, 10, 50));
            }

            nextRight++;
            if (nextRight > 23)
            {
                nextRight = 0;
                rightColumns.Add(new Rectangle(400, this.Height, 10, 50));
            }



            for (int i = 0;i < leftColumns.Count();i++)
            {
                int newY = leftColumns[i].Y + leftColumnSpeed;
                leftColumns[i] = new Rectangle(leftColumns[i].X, newY, leftColumns[i].Width, leftColumns[i].Height);

                if (leftColumns[i].IntersectsWith(hero))
                {
                    gameTimer.Enabled = false;
                }

                if(newY > this.Height)
                {
                    leftColumns.RemoveAt(i);
                }
            }
            for (int i = 0; i < rightColumns.Count(); i++)
            {
                int newY = rightColumns[i].Y + rightColumnSpeed;
                rightColumns[i] = new Rectangle(rightColumns[i].X, newY, rightColumns[i].Width, rightColumns[i].Height);


                if (rightColumns[i].IntersectsWith(hero))
                {
                    gameTimer.Enabled = false;
                }

                if (newY < 0 - rightColumns[i].Height)
                {
                    rightColumns.RemoveAt(i);
                }
            }

            Refresh();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(playerBrush, hero);

            for (int i = 0; i < leftColumns.Count(); i++)
            {
                e.Graphics.FillRectangle(obstacleBrush, leftColumns[i]);
            }
            for (int i = 0; i < rightColumns.Count(); i++)
            {
                e.Graphics.FillRectangle(obstacleBrush, rightColumns[i]);
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftKeyDown = false;
                    break;
                case Keys.Right:
                    rightKeyDown = false;
                    break;
                case Keys.Up:
                    upKeyDown = false;
                    break;
                case Keys.Down:
                    downKeyDown = false;
                    break;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftKeyDown = true;
                    break;
                case Keys.Right:
                    rightKeyDown = true;
                    break;
                case Keys.Up:
                    upKeyDown = true;
                    break;
                case Keys.Down:
                    downKeyDown = true;
                    break;
            }
        }
    }
}
