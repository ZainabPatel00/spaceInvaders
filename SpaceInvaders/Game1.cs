using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media; //for background music
using SpaceInvaders.States;

namespace SpaceInvaders;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics; //loading textures
    private SpriteBatch spriteBatch;

    private state _currentState;
    private state _nextState; //declare nextState then asign current to nextState

    Song bgMusic; //background music declaration

    public void ChangeState(state state) //state you want to change to
    {
        _nextState = state;
    }

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        IsMouseVisible = true;
        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);

        _currentState = new login(this, _graphics.GraphicsDevice, Content);  //first state shown is menu screen

        bgMusic = Content.Load<Song>("audio/bgMusic");  //loading background music
        MediaPlayer.Play(bgMusic);                      //playing music
        MediaPlayer.IsRepeating = true;                 //so track is looped
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        if (_nextState != null)
        {
            _currentState = _nextState;
            _nextState = null; //when button clicked change state
        }
        _currentState.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(new Color(35, 43, 51)); // default hex colour background, if no background image

        _currentState.Draw(gameTime, spriteBatch);

        base.Draw(gameTime);
    }
}

