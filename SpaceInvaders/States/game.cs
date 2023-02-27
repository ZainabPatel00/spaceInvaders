using System; //finalPrototype
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;    //for keyboard input
using SpaceInvaders.controls;              //to use button control
using Microsoft.Xna.Framework.Audio;    //library for sound effects
using System.IO;    // file handling

namespace SpaceInvaders.States
{
    class game : state //game inherited by state
    {
        private SpriteBatch spriteBatch;
        bool scoreSaved = false;

        Texture2D enemy1, enemy2, enemy3, player, playerBullet, enemyBullet, life, pausedTexture, pauseBtn;
        Rectangle playerRect, playerBulletRect, enemyBulletRect;
        Rectangle[,] enemyRect;
        bool[,] enemyAlive;
        string[,] enemyType;
        int rows = 5, cols = 8, score = 0, enemySpeed = 2, lives = 5, rightside = 800, leftside = 0;
        string direction = "right";
        bool enemyBulletVisible = false, playerBulletVisible = false, paused = false, gameOver = false, resumebtnClicked = false;
        private Button pauseButton;
        private TextButton resumeButton, toMenuBtn, newGameBtn;
        SoundEffect shoot, playerdead, enemykilled;

        public void newWave()
        {
            for (int r = 0; r < rows; r++)
                for (int c = 0; c < cols; c++)
                {
                    if (r == 0)
                    {
                        enemyType[r, c] = "enemy3";
                    }
                    else if (r == 1 || r == 2)
                    {
                        enemyType[r, c] = "enemy2";
                    }
                    else
                    {
                        enemyType[r, c] = "enemy1";

                    }
                    enemyRect[r, c].Width = enemy1.Width;
                    enemyRect[r, c].Height = enemy1.Height;
                    enemyRect[r, c].X = 45 * c;
                    enemyRect[r, c].Y = 35 * r + 50;
                    enemyAlive[r, c] = true;
                }
        }

        public game(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            spriteBatch = new SpriteBatch(_graphicsDevice);
            Sprites(); //sprites method from parent class 'state'

            var buttonTexture = _content.Load<Texture2D>("sprites/button");

            enemy1 = _content.Load<Texture2D>("sprites/enemy1");
            enemy2 = _content.Load<Texture2D>("sprites/enemy2");
            enemy3 = _content.Load<Texture2D>("sprites/enemy3");
            enemyRect = new Rectangle[rows, cols];
            enemyAlive = new bool[rows, cols];
            enemyType = new string[rows, cols];

            newWave();

            player = _content.Load<Texture2D>("sprites/player");
            playerRect.Width = player.Width;
            playerRect.Height = player.Height;
            playerRect.X = 400;
            playerRect.Y = 400;

            playerBullet = _content.Load<Texture2D>("sprites/playerBullet");
            playerBulletRect.Width = playerBullet.Width;
            playerBulletRect.Height = playerBullet.Height;
            playerBulletRect.X = 0;
            playerBulletRect.Y = 0;

            enemyBullet = _content.Load<Texture2D>("sprites/enemyBullet");
            enemyBulletRect.Width = enemyBullet.Width;
            enemyBulletRect.Height = enemyBullet.Height;
            enemyBulletRect.X = 0;
            enemyBulletRect.Y = 0;

            life = _content.Load<Texture2D>("sprites/player");

            pauseBtn = _content.Load<Texture2D>("sprites/pauseBtn");
            pauseButton = new Button(pauseBtn, null)
            {
                Position = new Vector2(750, 10),
            };
            pauseButton.Click += pauseButton_Click;

            pausedTexture = _content.Load<Texture2D>("sprites/pauseBG");

            resumeButton = new TextButton(smallArcadeFont, _graphicsDevice)
            {
                Position = new Vector2(350, 175),
                Text = "resume",
            };
            resumeButton.Click += resumeButton_Click;

            newGameBtn = new TextButton(smallArcadeFont, _graphicsDevice)
            {
                Position = new Vector2(350, 185),
                Text = "new game",
            };
            newGameBtn.Click += newGameBtn_Click;

            toMenuBtn = new TextButton(smallArcadeFont, _graphicsDevice)
            {
                Position = new Vector2(325, 220),
                Text = "back to menu",
            };
            toMenuBtn.Click += backButton_Click; //as they've got same function ie back to main menu

            shoot = _content.Load<SoundEffect>("audio/shoot");              //effect when player shoots
            enemykilled = _content.Load<SoundEffect>("audio/enemykilled");  //when enemy is killed
            playerdead = _content.Load<SoundEffect>("audio/playerdead");    //when player has lost all lives (game over)

        }
        private void newGameBtn_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new game(_game, _graphicsDevice, _content));
        }
        private void pauseButton_Click(object sender, EventArgs e)
        {
            paused = true;
        }
        private void resumeButton_Click(object sender, EventArgs e)
        {
            resumebtnClicked = true;
        }

        public override void LoadContent() { }

        public override void Update(GameTime gameTime)
        {

            if (!paused && !gameOver)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    paused = true;
                    resumebtnClicked = false;
                }

                //moving all aliens
                for (int r = 0; r < rows; r++)
                    for (int c = 0; c < cols; c++)
                    {
                        if (direction.Equals("right"))
                            enemyRect[r, c].X = enemyRect[r, c].X + enemySpeed;
                        if (direction.Equals("left"))
                            enemyRect[r, c].X = enemyRect[r, c].X - enemySpeed;
                    }

                //check to see if any gone past right side
                char moveDown = 'n';
                for (int r = 0; r < rows; r++)
                    for (int c = 0; c < cols; c++)
                        if (enemyAlive[r, c] == true)
                        {
                            if (enemyRect[r, c].X + enemyRect[r, c].Width > rightside)
                            {
                                direction = "left";
                                moveDown = 'y';
                            }
                            if (enemyRect[r, c].X < leftside)
                            {
                                direction = "right";
                                moveDown = 'y';
                            }
                        }

                //if enemy hits sidescreen, so direction changes then move down
                if (moveDown == 'y')
                {
                    for (int r = 0; r < rows; r++)
                        for (int c = 0; c < cols; c++)
                            enemyRect[r, c].Y = enemyRect[r, c].Y + 10;
                }

                //move player as long as within screen bounds
                KeyboardState kb = Keyboard.GetState();
                if (kb.IsKeyDown(Keys.Left) && playerRect.X > leftside)
                    playerRect.X = playerRect.X - 3;
                if (kb.IsKeyDown(Keys.Right) && playerRect.X + playerRect.Width < rightside)
                    playerRect.X = playerRect.X + 3;

                //make player bullet visible
                if (kb.IsKeyDown(Keys.Up) && playerBulletVisible == false)
                {
                    playerBulletVisible = true;
                    playerBulletRect.X = playerRect.X + (playerRect.Width / 2) - (playerBulletRect.Width / 2);
                    playerBulletRect.Y = playerRect.Y - (playerRect.Height / 2);
                    shoot.Play();   //sound effect
                }
                //if player bullet visible, move bullet
                if (playerBulletVisible == true)
                    playerBulletRect.Y = playerBulletRect.Y - 5;

                //check if player bullet hits alien
                if (playerBulletVisible == true)
                    for (int r = 0; r < rows; r++)
                        for (int c = 0; c < cols; c++)
                            if (enemyAlive[r, c] == true)
                                if (playerBulletRect.Intersects(enemyRect[r, c]))
                                {
                                    enemykilled.Play(); //sound effect
                                    playerBulletVisible = false;
                                    enemyAlive[r, c] = false;
                                    if (enemyType[r, c].Equals("enemy1"))
                                        score += 10;
                                    else if (enemyType[r, c].Equals("enemy2"))
                                        score += 20;
                                    else
                                        score += 30;
                                }

                //if player bullet off screen, make invisible
                if (playerBulletRect.Y + playerBulletRect.Height < 0)
                    playerBulletVisible = false;

                //checking how many aliens alive
                int count = 0;
                for (int r = 0; r < rows; r++)
                    for (int c = 0; c < cols; c++)
                        if (enemyAlive[r, c] == true)
                            count++;

                //if no aliens left, create new wave
                if (count == 0)
                    newWave();

                //if one enemy left, move faster ie x co-ord when mving gets bigger
                if (count == 1)
                    enemySpeed = 3;
                else
                    enemySpeed = 2;

                //pick random enemy  for enemy bullet
                Random rnd = new Random();
                int rndR = rnd.Next(0, rows - 1);
                int rndC = rnd.Next(0, cols - 1);

                // make enemy bullet visible, only if enemy chosen alive
                if (enemyAlive[rndR, rndC] == true)
                    if (enemyBulletVisible == false)
                    {
                        enemyBulletVisible = true;
                        enemyBulletRect.X = enemyRect[rndR, rndC].X + (enemyRect[rndR, rndC].Width / 2) - (enemyBulletRect.Width / 2);
                        enemyBulletRect.Y = enemyRect[rndR, rndC].Y + enemyRect[rndR, rndC].Height;
                    }

                //make enemy bullet visible
                if (enemyBulletVisible == true)
                    enemyBulletRect.Y = enemyBulletRect.Y + 5;
                if (enemyBulletRect.Intersects(playerRect))
                {
                    lives -= 1;
                    enemyBulletVisible = false;
                }
                if (enemyBulletRect.Y + enemyBulletRect.Height > 480)
                    enemyBulletVisible = false;


                //check if game over
                for (int r = 0; r < rows; r++)
                    for (int c = 0; c < cols; c++)
                    {
                        if (enemyAlive[r, c] == true)
                            if (enemyRect[r, c].Y + enemyRect[r, c].Height > playerRect.Y)
                                gameOver = true;
                    }

                if (lives == 0)
                {
                    playerdead.Play();  //sound effect
                    gameOver = true;
                }

                pauseButton.Update(gameTime);
            }
            else if (paused)
            {
                if (resumebtnClicked == true || Keyboard.GetState().IsKeyDown(Keys.Space))
                    paused = false;

                resumeButton.Update(gameTime);
            }
            else if (gameOver)
            {
                newGameBtn.Update(gameTime);
                toMenuBtn.Update(gameTime);
                //back to main button has same functionality of back button used

                if (scoreSaved == false)
                {
                    using (StreamWriter SW = new StreamWriter(filename, true)) //appends text to file
                    {
                        SW.WriteLine(login.Username + "," + score); //username from login state and score appended to file
                    }
                    scoreSaved = true;
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            //game content
            for (int r = 0; r < rows; r++)
                for (int c = 0; c < cols; c++)
                    if (enemyAlive[r, c] == true)
                        if (r == 0)
                            spriteBatch.Draw(enemy3, enemyRect[r, c], Color.White);
                        else if (r == 1 || r == 2)
                            spriteBatch.Draw(enemy2, enemyRect[r, c], Color.White);
                        else
                            spriteBatch.Draw(enemy1, enemyRect[r, c], Color.White);

            spriteBatch.Draw(player, playerRect, Color.White);

            if (playerBulletVisible == true)
                spriteBatch.Draw(playerBullet, playerBulletRect, Color.White);

            if (enemyBulletVisible == true)
                spriteBatch.Draw(enemyBullet, enemyBulletRect, Color.White);

            spriteBatch.DrawString(smallArcadeFont, "score    " + Convert.ToString(score), new Vector2(10, 10), Color.White);

            spriteBatch.DrawString(smallArcadeFont, "lives ", new Vector2(200, 10), Color.White);
            int x = 300;
            for (int i = 1; i <= lives; i++)
            {
                spriteBatch.Draw(life, new Vector2(x, 10), Color.White);
                x += 40;
            }
            pauseButton.Draw(gameTime, spriteBatch);

            if (paused)
            {
                spriteBatch.Draw(pausedTexture, new Vector2(0, 0), Color.White);
                spriteBatch.DrawString(ArcadeFont, "game paused", new Vector2(225, 75), Color.White);
                resumeButton.Draw(gameTime, spriteBatch);
            }

            if (gameOver)
            {
                spriteBatch.Draw(pausedTexture, new Vector2(0, 0), Color.White);
                spriteBatch.DrawString(ArcadeFont, "game over", new Vector2(250, 75), Color.White);
                spriteBatch.DrawString(smallArcadeFont, "your  score     " + score, new Vector2(320, 150), Color.White);
                //spriteBatch.DrawString(smallArcadeFont, "your  high  score ", new Vector2(300, 185), Color.White);
                toMenuBtn.Draw(gameTime, spriteBatch);
                newGameBtn.Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();
        }
    }
}