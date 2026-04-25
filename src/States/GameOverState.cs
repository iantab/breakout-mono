using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Breakout;

public class GameOverState : State
{
    private readonly GameContext ctx;
    private KeyboardState prevKeys;

    public GameOverState(GameContext c)
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
        bool justPressedEnter = keys.IsKeyDown(Keys.Enter) && !prevKeys.IsKeyDown(Keys.Enter);
        prevKeys = keys;

        if (justPressedEnter)
        {
            ctx.ResetGame();
            ctx.Machine.ChangeState(new ServeState(ctx));
        }
    }

    public override void Draw(SpriteBatch sb, SpriteFont font, Texture2D pixel)
    {
        foreach (var b in ctx.Bricks) b.Draw(sb, pixel);
        ctx.Paddle.Draw(sb, pixel);
        ctx.Ball.Draw(sb);

        sb.Draw(pixel, new Rectangle(0, 0, ctx.ViewportWidth, ctx.ViewportHeight),
            new Color(0, 0, 0, 160));

        sb.DrawString(font, "GAME OVER",
            new Vector2(ctx.ViewportWidth / 2 - 60, 120),
            Color.White);
        sb.DrawString(font, $"Final Score: {ctx.Score}",
            new Vector2(ctx.ViewportWidth / 2 - 70, 160),
            Color.White);
        sb.DrawString(font, "Press Enter to restart",
            new Vector2(ctx.ViewportWidth / 2 - 90, 200),
            Color.LightGray);
    }
}