using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace breakout_mono;

public class Game1 : Game
{
    public const int VirtualWidth = 640;
    public const int VirtualHeight = 360;

    public const int WindowWidth = 1280;
    public const int WindowHeight = 720;

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private RenderTarget2D _virtualTarget;
    private Texture2D _pixel;

    private Paddle _paddle;
    private Ball _ball;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = WindowWidth;
        _graphics.PreferredBackBufferHeight = WindowHeight;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _virtualTarget = new RenderTarget2D(
            GraphicsDevice, VirtualWidth, VirtualHeight);
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _pixel = new Texture2D(GraphicsDevice, 1, 1);
        _pixel.SetData(new[] { Color.White });

        _paddle = new Paddle(new Vector2(
            VirtualWidth / 2 - 48, VirtualHeight - 32));
        _ball = new Ball(new Vector2(
            VirtualWidth / 2 - 6, VirtualHeight / 2));
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.SetRenderTarget(_virtualTarget);
        GraphicsDevice.Clear(new Color(20, 22, 39));
        
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _paddle.Draw(_spriteBatch, _pixel);
        _ball.Draw(_spriteBatch, _pixel);
        _spriteBatch.End();

        GraphicsDevice.SetRenderTarget(null);
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _spriteBatch.Draw(_virtualTarget, new Rectangle(0, 0, WindowWidth, WindowHeight), Color.White);
        _spriteBatch.End();
        base.Draw(gameTime);
    }
}