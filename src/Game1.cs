using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Breakout;

public class Game1 : Game
{
    public const int VirtualWidth = 640;
    public const int VirtualHeight = 360;

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private RenderTarget2D _virtualTarget;
    private Texture2D _pixel;

    private Paddle _paddle;
    private Ball _ball;
    private List<Brick> _bricks;

    private SpriteFont _font;
    private int _score;
    private int _lives = 3;

    private GameContext _ctx;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
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
        _font = Content.Load<SpriteFont>("Arcade");

        _ctx = new GameContext
        {
            Paddle = new Paddle(new Vector2(VirtualWidth / 2 - 48, VirtualHeight - 32)),
            Ball = new Ball(new Vector2(VirtualWidth / 2 - 6, VirtualHeight / 2)),
            Bricks = LevelMaker.Generate(1, VirtualWidth),
            ViewportWidth = VirtualWidth,
            ViewportHeight = VirtualHeight,
            Machine = new StateMachine(),
        };
        _ctx.Machine.ChangeState(new ServeState(_ctx));
    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        _ctx.Machine.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.SetRenderTarget(_virtualTarget);
        GraphicsDevice.Clear(new Color(20, 22, 39));

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _ctx.Machine.Draw(_spriteBatch, _font, _pixel);
        _spriteBatch.End();

        GraphicsDevice.SetRenderTarget(null);
        GraphicsDevice.Clear(Color.Black);

        int bw = GraphicsDevice.PresentationParameters.BackBufferWidth;
        int bh = GraphicsDevice.PresentationParameters.BackBufferHeight;

        const float targetAspect = (float)VirtualWidth / VirtualHeight;
        float screenAspect = (float)bw / bh;

        int destW, destH;
        if (screenAspect > targetAspect)
        {
            destH = bh;
            destW = (int)(bh * targetAspect);
        }
        else
        {
            destW = bw;
            destH = (int)(bw / targetAspect);
        }

        int destX = (bw - destW) / 2;
        int destY = (bh - destH) / 2;

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _spriteBatch.Draw(_virtualTarget, new Rectangle(destX, destY, destW, destH), Color.White);
        _spriteBatch.End();
        base.Draw(gameTime);
    }
}