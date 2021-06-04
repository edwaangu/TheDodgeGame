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
        // Hero Variables
        Rectangle hero = new Rectangle(50, 50, 20, 20);
        int speed = 5;

        // Column Variables
        List<Rectangle> leftColumns = new List<Rectangle>();
        List<Rectangle> rightColumns = new List<Rectangle>();
        int leftColumnSpeed = 6;
        int rightColumnSpeed = -4;
        int nextLeft = 0;
        int nextRight = 0;
        int runTimes = 120;

        // KeyPress Variables
        bool leftKeyDown = false;
        bool rightKeyDown = false;
        bool upKeyDown = false;
        bool downKeyDown = false;

        // Scene Variable
        string scene = "menu";

        // Drawing Variables
        SolidBrush playerBrush = new SolidBrush(Color.Blue);
        SolidBrush obstacleBrush = new SolidBrush(Color.White);

        public Form1()
        {
            InitializeComponent();
        }

        // This function is used to run the columns, multiple times at the beginning to make sure the player can't just go under the first left column and above the first right column since it takes time for them to go down.
        public void theColumns()
        {
            // When 18 frames have passed, spawn in another column on the left
            nextLeft++;
            if (nextLeft > 17)
            {
                nextLeft = 0;
                leftColumns.Add(new Rectangle(150, -50, 10, 50));
            }

            // When 24 frames have passed, spawn in another column on the right
            nextRight++;
            if (nextRight > 23)
            {
                nextRight = 0;
                rightColumns.Add(new Rectangle(400, this.Height, 10, 50));
            }

            // Use a for loop to determine new positions for all left columns
            for (int i = 0; i < leftColumns.Count(); i++)
            {
                int newY = leftColumns[i].Y + leftColumnSpeed;
                leftColumns[i] = new Rectangle(leftColumns[i].X, newY, leftColumns[i].Width, leftColumns[i].Height);

                // If the player's hitbox mixes with the column's hitbox, send them to the lose screen
                if (leftColumns[i].IntersectsWith(hero))
                {
                    gameTimer.Enabled = false;
                    scene = "lose";
                }

                // When a column has gone off down the screen, remove this specific column
                if (newY > this.Height)
                {
                    leftColumns.RemoveAt(i);
                }
            }

            // Use a for loop to determine new positions for all right columns
            for (int i = 0; i < rightColumns.Count(); i++)
            {
                int newY = rightColumns[i].Y + rightColumnSpeed;
                rightColumns[i] = new Rectangle(rightColumns[i].X, newY, rightColumns[i].Width, rightColumns[i].Height);


                // If the player's hitbox mixes with the column's hitbox, send them to the lose screen
                if (rightColumns[i].IntersectsWith(hero))
                {
                    gameTimer.Enabled = false;
                    scene = "lose";
                }

                // When a column has gone off up the screen, remove this specific column
                if (newY < 0 - rightColumns[i].Height)
                {
                    rightColumns.RemoveAt(i);
                }
            }
        }

        // This should tick 50 frames a second, but for some reason visual studio likes to lag on my computer for some reason, so it seems more like 20 - 30, even though I do have a pretty good computer to run it on?
        private void gameTimer_Tick(object sender, EventArgs e)
        {
            // Move the player in different directions depending if that certain key variable was set to true
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

            // If the hero tries to go off the screen on the left side, move them back to the edge. If they weren't, keep their position the same.
            hero.X = hero.X < 0 ? 0 : hero.X;

            // If the hero goes off the right side of the screen, send them to the win screen since they passed all obstacles
            if(hero.X + hero.Width > this.Width)
            {
                gameTimer.Enabled = false;
                scene = "win";
            }

            // If the hero tries to go off the bottom of the screen, send them back up to the edge. Otherwise if the hero is trying to go above the upwards limit, send them back to the upwards edge. If none of those conditions happened, the hero doesn't change position.
            hero.Y = hero.Y < 0 ? 0 : (hero.Y + hero.Height > this.Height ? this.Height - hero.Height : hero.Y);

            // Run 120 frames of the columns to boost them down the screen, by the time this runs, the first right column is around touching the top.
            while (runTimes > 0)
            {
                theColumns();
                runTimes--;
            }

            // Then regularly run the columns, techincally it runs 121 times the first frame.
            theColumns();

            Refresh();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (scene == "game")
            {
                // Fill rectangles for the players and column obstacles
                e.Graphics.FillRectangle(playerBrush, hero);

                for (int i = 0; i < leftColumns.Count(); i++)
                {
                    e.Graphics.FillRectangle(obstacleBrush, leftColumns[i]);
                }
                for (int i = 0; i < rightColumns.Count(); i++)
                {
                    e.Graphics.FillRectangle(obstacleBrush, rightColumns[i]);
                }
            } // Depending on the three remaining scenes, show and change the label messages as needed
            else if(scene == "menu")
            {
                titleLabel.Text = "DODGING GAME";
                extraLabel.Text = "Space to Start, Escape to Exit";
            }
            else if (scene == "lose")
            {
                titleLabel.Text = "YOU GOT HIT";
                extraLabel.Text = "Space to Try Again, Escape to Exit";
            }
            else if (scene == "win")
            {
                titleLabel.Text = "YOU WIN";
                extraLabel.Text = "Space to Play Again, Escape to Exit";
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            // When a key is released, update said corresponding variable
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
            // When a key is pressed, update said corresponding variable
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
                case Keys.Space:
                    if (scene != "game")
                    {
                        // If a player presses the space button on a menu that isn't the game, reset the game so the player can restart or play for their first time.
                        hero.X = 50;
                        hero.Y = 50;

                        runTimes = 120;

                        leftColumns.Clear();
                        rightColumns.Clear();

                        nextLeft = 0;
                        nextRight = 0;


                        // Make sure to hide the labels before the game starts
                        titleLabel.Text = "";
                        extraLabel.Text = "";


                        gameTimer.Enabled = true;
                        scene = "game";
                    }
                    break;
                case Keys.Escape:
                    // If the player isn't in the middle of a game, they can close the program by pressing the escape key to... escape the game.
                    if(scene != "game")
                    {
                        Application.Exit();
                    }
                    break;
            }
        }
    }
}
