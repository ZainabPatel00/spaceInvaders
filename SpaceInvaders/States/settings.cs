using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio; //for sound effects
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media; //for text buttons
using SpaceInvaders;
using System.Collections.Generic;
using SpaceInvaders.controls;              //for text buttons

namespace SpaceInvaders.States
{
    class settings : state //settings inherited by state
    {
        private TextButton onMusic, offMusic, onEffects, offEffects, volumeUp, volumeDown;
        public List<Component> VolumeControls;
        float vol = 10;
        //problem when change vol then go off, then returns to page stays 10 instead of changed value

        public settings(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            Sprites(); //sprites method from parent class 'state'

            //button controls
            onMusic = new TextButton(mainFont, graphicsDevice)  //background music
            {
                Position = new Vector2(500, 200),
                Text = "on",
            };
            onMusic.Click += OnMusic_Click;
            offMusic = new TextButton(mainFont, graphicsDevice)
            {
                Position = new Vector2(525, 200),
                Text = "off",
            };
            offMusic.Click += OffMusic_Click;
            onEffects = new TextButton(mainFont, graphicsDevice)    //sound effects
            {
                Position = new Vector2(500, 250),
                Text = "on",
            };
            onEffects.Click += OnEffects_Click;
            offEffects = new TextButton(mainFont, graphicsDevice)
            {
                Position = new Vector2(525, 250),
                Text = "off",
            };
            offEffects.Click += OffEffects_Click;
            volumeDown = new TextButton(mainFont, graphicsDevice)   //volume changes
            {
                Position = new Vector2(475, 300),
                Text = "-",
            };
            volumeDown.Click += VolumeDown_Click;
            volumeUp = new TextButton(mainFont, graphicsDevice)
            {
                Position = new Vector2(535, 300),
                Text = "+"
            };
            volumeUp.Click += VolumeUp_Click;

            VolumeControls = new List<Component>()  //buttons add to list
            {
                onMusic, offMusic, onEffects, offEffects, volumeDown, volumeUp,
            };
        }

        private void OnMusic_Click(object sender, EventArgs e)
        {
            MediaPlayer.IsMuted = false;
        }
        private void OffMusic_Click(object sender, EventArgs e)
        {
            MediaPlayer.IsMuted = true;
        }
        private void OnEffects_Click(object sender, EventArgs e)
        {
            SoundEffect.MasterVolume = vol / 10f;
        }
        private void OffEffects_Click(object sender, EventArgs e)
        {
            SoundEffect.MasterVolume = 0.0f;
        }
        private void VolumeDown_Click(object sender, EventArgs e)
        {
            if (vol > 0)    //volume only goes down if its more than 0
            {
                SoundEffect.MasterVolume = (vol / 10) - 0.1f;
                MediaPlayer.Volume = (vol / 10) - 0.1f;
                vol--;
            }
        }
        private void VolumeUp_Click(object sender, EventArgs e)
        {
            if (vol < 10)   //volume only goes up if its less than 10
            {
                SoundEffect.MasterVolume = (vol / 10) + 0.1f;
                MediaPlayer.Volume = (vol / 10) + 0.1f;
                vol++;
            }
        }

        public override void LoadContent() { }

        public override void Update(GameTime gameTime)
        {
            backButton.Update(gameTime); //updating back button to see if it's clicked
            foreach (var component in VolumeControls)
                component.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(bgTexture, new Vector2(0, 0), Color.White);                            //background texture
            spriteBatch.DrawString(ArcadeFont, "settings", new Vector2(250, 75), Color.White);      //title in arcade font

            backButton.Draw(gameTime, spriteBatch); //back button from parent class drawn
            foreach (var component in VolumeControls)
                component.Draw(gameTime, spriteBatch);


            spriteBatch.DrawString(mainFont, "background music:", new Vector2(275, 200), Color.White);
            spriteBatch.DrawString(mainFont, "sound effects:", new Vector2(275, 250), Color.White);
            spriteBatch.DrawString(mainFont, "volume:", new Vector2(275, 300), Color.White);
            spriteBatch.DrawString(mainFont, Convert.ToString(vol), new Vector2(505, 300), Color.White);

            spriteBatch.End();
        }
    }
}
