using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace SpaceInvaders.controls
{
    public class Button : Component //class Button inherits component
    {

        private MouseState _currentMouse, _previousMouse; //use both to determine if left clicked by being pressed(released) after being released (pressed)
        private SpriteFont _font; //font of text specified when passed
        private bool _isHovering; //mouse hovering over button
        private Texture2D _texture; //button texture

        public event EventHandler Click; //assigns method to click which is then called

        public Color PenColour = Color.Black; //text colour
        public Vector2 Position { get; set; } //button position, passed when new button created
        public Rectangle Rectangle //determines if mouse on top of button
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
            }
        }
        public string Text { get; set; } //button text overlay, passed when new button created

        public Button(Texture2D texture, SpriteFont font) //assigns texture, font for new button
        {
            _texture = texture;
            _font = font;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var colour = Color.White;

            if (_isHovering)
                colour = Color.Gray; //when mouse over button, more interactive

            spriteBatch.Draw(_texture, Rectangle, colour); //draw button

            if (!string.IsNullOrEmpty(Text))//draws font in middle of texture
            {
                var x = (Rectangle.X + (Rectangle.Width / 2)) - (_font.MeasureString(Text).X / 2);
                var y = (Rectangle.Y + (Rectangle.Height / 2)) - (_font.MeasureString(Text).Y / 2);

                spriteBatch.DrawString(_font, Text, new Vector2(x, y), PenColour); //draw text
            }
        }

        public override void Update(GameTime gameTime) //determining if hovering or clicked
        {
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();
            var mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1); //mouse rectangle, so where mouse is
            _isHovering = false; //hovering set as false to start with

            if (mouseRectangle.Intersects(Rectangle))//if mouse intersects button
            {
                _isHovering = true;

                //determines if clicled buttonx
                if (_currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed) //pressed then let go
                {
                    Click?.Invoke(this, new EventArgs()); //if click event handler not null use method assigned to button
                }
            }
        }
    }
}
