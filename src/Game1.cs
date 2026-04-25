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

        _paddle = new Paddle(new Vector2(
            VirtualWidth / 2 - 48, VirtualHeight - 32));
        _ball = new Ball(new Vector2(VirtualWidth / 2, VirtualHeight / 2));
        _bricks = LevelMaker.Generate(level: 1, VirtualWidth);
        _font = Content.Load<SpriteFont>("Arcade");
    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _paddle.Update(delta, VirtualWidth);
        _ball.Update(delta, VirtualWidth, VirtualHeight);
        _ball.TryBouncePaddle(_paddle);
        var hit = _ball.TryBounceBricks(_bricks, delta);
        if (hit != null) _score += hit.Points;
        base.Update(gameTime);
        if (_ball.Position.Y > VirtualHeight)
        {
            _lives--;
            if (_lives <= 0)
            {
                _lives = 3;
                _score = 0;
                _bricks = LevelMaker.Generate(1, VirtualWidth);
            }

            _ball.Serve();
        }
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.SetRenderTarget(_virtualTarget);
        GraphicsDevice.Clear(new Color(20, 22, 39));

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        foreach (var brick in _bricks) brick.Draw(_spriteBatch, _pixel);
        _paddle.Draw(_spriteBatch, _pixel);
        _ball.Draw(_spriteBatch);
        _spriteBatch.DrawString(_font, $"Score: {_score}",
            new Vector2(8, 8), Color.White);
        _spriteBatch.DrawString(_font, $"Lives: {_lives}",
            new Vector2(VirtualWidth - 100, 8), Color.White);
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