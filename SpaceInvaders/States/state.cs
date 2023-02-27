using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpaceInvaders.controls; //to access button controls

namespace SpaceInvaders.States
{
    public abstract class state
    {
        public static string filename = "scores.txt"; //file storing username with score

        protected ContentManager _content; //loads content
        protected GraphicsDevice _graphicsDevice; //instantiate texture without loading from content width of viewport etc
        protected Game1 _game;
        public Texture2D bgTexture, backBtnTexture; //buttons and background textures
        public SpriteFont ArcadeFont, smallArcadeFont, mainFont; //font variables declared
        public Button backButton;

        public state(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)//constructor
        {
            _game = game;
            _graphicsDevice = graphicsDevice;
            _content = content;
        }

        public abstract void LoadContent();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        public void Sprites()
        {
            ArcadeFont = _content.Load<SpriteFont>("font/Arcade"); //font for screen titles
            smallArcadeFont = _content.Load<SpriteFont>("font/smallArcade");
            mainFont = _content.Load<SpriteFont>("font/Font"); //main font for text

            bgTexture = _content.Load<Texture2D>("sprites/bg"); //background image for all screens, but game screen
            backBtnTexture = _content.Load<Texture2D>("sprites/backBtn"); //back button from instructions, scoreboard, settings to main menu

            backButton = new Button(backBtnTexture, null) //using same button control as main buttons, but has no text overlay
            {
                Position = new Vector2(10, 10), //all back buttons are positioned in top left corner
            };
            backButton.Click += backButton_Click; //method called when back button clicked
        }
        public void backButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new menu(_game, _graphicsDevice, _content)); //when back button clicked, a new menu appears
        }
    }
}
