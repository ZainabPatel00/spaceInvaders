using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceInvaders.States
{
    class instructions : state //instructions inherited by state
    {
        public instructions(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
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
            spriteBatch.Draw(bgTexture, new Vector2(0, 0), Color.White);                                //background texture
            spriteBatch.DrawString(ArcadeFont, "instructions", new Vector2(200, 75), Color.White);      //title in arcade font

            //instructions using main font
            spriteBatch.DrawString(mainFont, " To move use the right and left arrows, shoot using the up arrow." +
                                             "\n To pause use the space bar. The aim of the game is to defeat all " +
                                             "\n the alien spaceships by shooting then. You lose a life each time " +
                                             "\n the enemy shoots you. the game ends when you've failed to defeat " +
                                             "\n all of them before they reach you or when all your lives have been " +
                                             "\n lost.",
                new Vector2(100, 200), Color.White);

            backButton.Draw(gameTime, spriteBatch);  //back button from parent class drawn
            spriteBatch.End();
        }
    }
}
