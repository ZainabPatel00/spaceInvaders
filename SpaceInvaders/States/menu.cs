using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpaceInvaders.controls;

namespace SpaceInvaders.States
{
    class menu : state //menu inherited by state
    {
        public List<Component> _components; //list for buttons - uses the component.cs for update

        public menu(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
          : base(game, graphicsDevice, content)
        {
            Sprites();  //calling procedure from parent class 'state'
            var buttonTexture = _content.Load<Texture2D>("sprites/button");

            //creating new buttons all using same background texture and font 'mainFont'
            var newGameButton = new Button(buttonTexture, mainFont)
            {
                Position = new Vector2(300, 175), //position of button
                Text = "New Game", //text overlay for button
            };
            newGameButton.Click += newGameButton_Click; //if call click method, itll call whatever is assigned to it

            var InstructionButton = new Button(buttonTexture, mainFont)
            {
                Position = new Vector2(300, 225),
                Text = "Instructions",
            };
            InstructionButton.Click += InstructionButton_Click;

            var settingsButton = new Button(buttonTexture, mainFont)
            {
                Position = new Vector2(300, 275),
                Text = "settings",
            };
            settingsButton.Click += settingsButton_Click;

            var scoreboardButton = new Button(buttonTexture, mainFont)
            {
                Position = new Vector2(300, 325),
                Text = "scoreboard",
            };
            scoreboardButton.Click += scoreboardButton_Click;

            var quitGameButton = new Button(buttonTexture, mainFont)
            {
                Position = new Vector2(300, 375),
                Text = "Quit Game",
            };
            quitGameButton.Click += QuitGameButton_Click;

            _components = new List<Component>() //all buttons added to component list
            {
                newGameButton,
                InstructionButton,
                settingsButton,
                scoreboardButton,
                quitGameButton,
            };
        }

        //events for buttons
        private void InstructionButton_Click(object sender, EventArgs e)            //new instructions page
        {
            _game.ChangeState(new instructions(_game, _graphicsDevice, _content));
        }

        private void settingsButton_Click(object sender, EventArgs e)               //new settings page
        {
            _game.ChangeState(new settings(_game, _graphicsDevice, _content));
        }
        private void scoreboardButton_Click(object sender, EventArgs e)            //new scoreboard page
        {
            _game.ChangeState(new scoreboard(_game, _graphicsDevice, _content));
        }

        private void newGameButton_Click(object sender, EventArgs e)                 //new game
        {
            _game.ChangeState(new game(_game, _graphicsDevice, _content));
        }

        private void QuitGameButton_Click(object sender, EventArgs e)               //this closes the whole program
        {
            _game.Exit();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var component in _components)  //for each button in the list of components
                component.Update(gameTime);         // update them to check if theyre clicked
        }


        public override void LoadContent() { }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(bgTexture, new Vector2(0, 0), Color.White);  //drawing background and title
            spriteBatch.DrawString(ArcadeFont, "shooter game", new Vector2(200, 75), Color.White);

            foreach (var component in _components)      //for each button in the list of components
                component.Draw(gameTime, spriteBatch);  //update them to check if theyre clicked

            spriteBatch.DrawString(smallArcadeFont, "welcome  " + login.Username, new Vector2(10, 10), Color.White);

            spriteBatch.End();
        }
    }
}