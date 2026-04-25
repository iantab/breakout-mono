using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Breakout;

public class VictoryState : State
{
    private readonly GameContext ctx;
    private KeyboardState prevKeys;

    public VictoryState(GameContext c)
    {
        ctx = c;
    }

    public override void Enter()
    {
        prevKeys = Keyboard.GetState();
    }

    public override void Update(float delta)
    {
        var keys = Keyboard.GetState();
        bool justPressedEnter =
            keys.IsKeyDown(Keys.Enter) && !prevKeys.IsKeyDown(Keys.Enter);
        prevKeys = keys;
        if (justPressedEnter)
        {
            ctx.Bricks = LevelMaker.Generate(level: 1, ctx.ViewportWidth);
            ctx.Ball.Reset();
            ctx.Machine.ChangeState(new ServeState(ctx));
        }
    }

    public override void Draw(SpriteBatch sb, SpriteFont font, Texture2D pixel)
    {
        ctx.Paddle.Draw(sb, pixel);
        ctx.Ball.Draw(sb);

        sb.Draw(pixel, new Rectangle(0, 0, ctx.ViewportWidth, ctx.ViewportHeight),
            new Color(0, 0, 0, 160));

        sb.DrawString(font, "VICTORY!",
            new Vector2(ctx.ViewportWidth / 2 - 55, 120),
            Color.Yellow);
        sb.DrawString(font, $"Score: {ctx.Score}",
            new Vector2(ctx.ViewportWidth / 2 - 50, 160),
            Color.White);
        sb.DrawString(font, "Press Enter to continue",
            new Vector2(ctx.ViewportWidth / 2 - 95, 200),
            Color.LightGray);
    }
}