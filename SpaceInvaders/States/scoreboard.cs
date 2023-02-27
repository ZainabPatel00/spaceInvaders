using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.IO;    //to use files
using System.Linq;  //so they can be ordered


namespace SpaceInvaders.States
{
    class scoreboard : state //scoreboard inherited by state
    {
        public scoreboard(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            Sprites(); //sprites method from parent class 'state'
        }

        public override void LoadContent() { }
        public override void Update(GameTime gameTime)
        {
            backButton.Update(gameTime); //updating back button to see if it's clicked
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(bgTexture, new Vector2(0, 0), Color.White);                            //background texture loaded
            spriteBatch.DrawString(ArcadeFont, "scoreboard", new Vector2(200, 75), Color.White);    //title drawn in arcade font
            backButton.Draw(gameTime, spriteBatch);     //back button from parent class drawn

            string[] scores = File.ReadAllLines(filename);  //reads in each line of file as strings to an array
            var orderedScores = scores.OrderByDescending(x => int.Parse(x.Split(',')[1]));  //splits each line at the comma to get the score, orders them
            int count = 1;  //counting ranking of player
            int y = 200; //starting drawing point
            char delimiter = ','; //to split string at comma

            foreach (var player in orderedScores)    //loop through each line in ordered scores
            {
                if (count < 4)//only draw first 3
                {
                    string x = player;  //current line
                    string[] substring = x.Split(delimiter);    //array of split line
                    string username = substring[0];
                    string score = substring[1];

                    spriteBatch.DrawString(mainFont, Convert.ToString(count), new Vector2(250, y), Color.White);    //ranking
                    spriteBatch.DrawString(mainFont, username, new Vector2(325, y), Color.White);  //player
                    spriteBatch.DrawString(mainFont, score, new Vector2(425, y), Color.White);  //score

                    count++;    //next highest score
                    y += 50;    //next line
                }
                else
                    break;
            }

            spriteBatch.End();
        }
    }
}
