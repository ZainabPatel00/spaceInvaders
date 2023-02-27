using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
using SpaceInvaders.controls;

namespace SpaceInvaders.States
{
    class login : state //login class inherits state
    {
        private Keys[] lastPressedKeys = new Keys[5];   //array of last pressed
        public static string Username = string.Empty;         //initialising string as empty
        private Button submitButton;

        Texture2D pixel; //texture for textbox
        Rectangle textBox; //rectangle for textbox

        public login(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            Sprites();
            var buttonTexture = _content.Load<Texture2D>("sprites/button");

            pixel = new Texture2D(graphicsDevice, 1, 1);    //texture for textbox
            pixel.SetData<Color>(new Color[] { Color.White });  //setting colour of texture
            textBox = new Rectangle(275, 165, 250, 40); //properties of textbox rectangle

            submitButton = new Button(buttonTexture, mainFont)  //creating submit button
            {
                Position = new Vector2(300, 240),
                Text = "submit",
            };
            submitButton.Click += submitButton_Click;   //procedure call when button clicked
        }
        private void submitButton_Click(object sender, EventArgs e) //action for when button clicked
        {
            if (Username.Length > 0)
                _game.ChangeState(new menu(_game, _graphicsDevice, _content));  //go to menu
        }
        public override void LoadContent() { }
        public override void Update(GameTime gameTime)
        {
            GetKeys();  //caling procedure to read in keys
            submitButton.Update(gameTime);  //update button
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(bgTexture, new Vector2(0, 0), Color.White);                        //background
            spriteBatch.Draw(pixel, textBox, Color.White);                                      //textbox
            spriteBatch.DrawString(ArcadeFont, "enter  username", new Vector2(150, 75), Color.White);     //title
            spriteBatch.DrawString(mainFont, Username, new Vector2(280, 175), Color.Black);     //text in box
            submitButton.Draw(gameTime, spriteBatch);                                            //submit button            
            spriteBatch.End();
        }

        public void GetKeys()   //procedure to read in keys
        {
            KeyboardState kbState = Keyboard.GetState();    //key user is inputting
            Keys[] pressedKeys = kbState.GetPressedKeys();  //key added to array

            foreach (Keys key in pressedKeys)       //key that is being pressed
            {
                if (!lastPressedKeys.Contains(key)) //if key is not already in username
                {
                    onKeyDown(key); //add key to username
                }
            }
            lastPressedKeys = pressedKeys;
        }

        public void onKeyDown(Keys key)
        {
            int keyLen = Convert.ToString(key).Length;  //get length of username array as string

            if (Username.Length < 26 || key == Keys.Back)           //sets max length
                if (key == Keys.Back && Username.Length > 0)            //can only go back if not empty
                    Username = Username.Remove(Username.Length - 1);    //backspace so letter removed
                else if (keyLen == 1)               //if key inputted is length of 1
                    Username += key.ToString();     //add key as a string to username
        }
    }
}
