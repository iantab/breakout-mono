using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using nkast.Wasm.Dom;

namespace Breakout;

public class ServeState : State
{
    private readonly GameContext ctx;

    public ServeState(GameContext c)
    {
        ctx = c;
    }

    public override void Enter() => ctx.Ball.Reset();

    public override void Update(float delta)
    {
        ctx.Paddle.Update(delta, ctx.ViewportWidth);
        ctx.Ball.Position = new Vector2(
            ctx.Paddle.Position.X + ctx.Paddle.Size.X / 2,
            ctx.Paddle.Position.Y - ctx.Ball.Radius - 2);
        if (Keyboard.GetState().IsKeyDown(Keys.Space))
        {
            ctx.Ball.Serve();
            ctx.Machine.ChangeState(new PlayState(ctx));
        }
    }

    public override void Draw(SpriteBatch sb, SpriteFont font, Texture2D pixel)
    {
        foreach (var b in ctx.Bricks) b.Draw(sb, pixel);
        ctx.Paddle.Draw(sb, pixel);
        ctx.Ball.Draw(sb);
        sb.DrawString(font, "Press Space",
            new Vector2(ctx.ViewportWidth / 2 - 60, 160), Color.White);
    }
}