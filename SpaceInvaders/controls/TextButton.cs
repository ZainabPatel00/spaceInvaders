using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders.controls
{
    class TextButton : Component //class textButton inherits component
    {
        private MouseState currentMouse, previousMouse; //use both to determine if left clicked by being pressed(released) after being released (pressed)
        private SpriteFont font; //font of text specified when passed
        private bool isHovering; //mouse hovering over button
        public GraphicsDevice _graphicsDevice;
        public event EventHandler Click; //assigns method to click which is then called
        public bool Clicked { get; private set; }
        public Color PenColour { get; set; } //text colour
        public Vector2 Position { get; set; } //button position, passed when new button created
        public Rectangle Rectangle
        {
            get
            {
                Vector2 textSize = font.MeasureString(Text);
                return new Rectangle((int)Position.X, (int)Position.Y, (int)textSize.X, (int)textSize.Y);
            }
        }
        public string Text { get; set; } //button text overlay, passed when new button created
        public TextButton(SpriteFont font, GraphicsDevice graphicsDevice)
        {
            this.font = font;
            _graphicsDevice = graphicsDevice;
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var colour = Color.Black;

            if (isHovering)
                PenColour = Color.Gray; //colour when mouse over button
            else
                PenColour = Color.White; //normal colour

            Texture2D texture = new Texture2D(_graphicsDevice, 1, 1);
            texture.SetData<Color>(new Color[] { Color.Transparent });
            spriteBatch.Draw(texture, Rectangle, colour); //draw button

            if (!string.IsNullOrEmpty(Text)) //draws text in middle of texture
            {
                var x = (Rectangle.X + (Rectangle.Width / 2)) - (font.MeasureString(Text).X / 2);
                var y = (Rectangle.Y + (Rectangle.Height / 2)) - (font.MeasureString(Text).Y / 2);

                spriteBatch.DrawString(font, Text, new Vector2(x, y), PenColour);
            }
        }

        public override void Update(GameTime gameTime) //determining if hovering or clicked
        {
            previousMouse = currentMouse;
            currentMouse = Mouse.GetState();
            var mouseRectangle = new Rectangle(currentMouse.X, currentMouse.Y, 1, 1);  //mouse rectangle, so where mouse is
            isHovering = false; //not hoverng initially
            if (mouseRectangle.Intersects(Rectangle)) //if mouse intersects text button
            {
                isHovering = true;
                //determines if button is clicked 
                if (currentMouse.LeftButton == ButtonState.Released && previousMouse.LeftButton == ButtonState.Pressed)
                {
                    Click?.Invoke(this, new EventArgs()); //if click event handler not null use method assigned to button
                }
            }
        }
    }
}
