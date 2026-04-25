using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Breakout;

public class PlayState : State
{
    private readonly GameContext ctx;

    public PlayState(GameContext c)
    {
        ctx = c;
    }

    public override void Update(float delta)
    {
        ctx.Paddle.Update(delta, ctx.ViewportWidth);
        ctx.Ball.Update(delta, ctx.ViewportWidth, ctx.ViewportHeight);
        ctx.Ball.TryBouncePaddle(ctx.Paddle);
        var hit = ctx.Ball.TryBounceBricks(ctx.Bricks, delta);
        if (hit != null) ctx.Score += hit.Points;
        if (ctx.Ball.Position.Y > ctx.ViewportHeight)
        {
            ctx.Lives--;
            if (ctx.Lives <= 0)
                ctx.Machine.ChangeState(new GameOverState(ctx));
            else
                ctx.Machine.ChangeState(new ServeState(ctx));
            return;
        }

        bool anyAlive = false;
        foreach (var b in ctx.Bricks)
            if (!b.Destroyed)
            {
                anyAlive = true;
                break;
            }

        if (!anyAlive)
            ctx.Machine.ChangeState(new VictoryState(ctx));
    }

    public override void Draw(SpriteBatch sb, SpriteFont font, Texture2D pixel)
    {
        foreach (var b in ctx.Bricks) b.Draw(sb, pixel);
        ctx.Paddle.Draw(sb, pixel);
        ctx.Ball.Draw(sb);
        sb.DrawString(font, $"Score: {ctx.Score}",
            new Vector2(8, 8), Color.White);
        sb.DrawString(font, $"Lives: {ctx.Lives}",
            new Vector2(ctx.ViewportWidth - 100, 8), Color.White);
    }
}