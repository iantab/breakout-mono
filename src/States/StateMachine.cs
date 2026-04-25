using Microsoft.Xna.Framework.Graphics;

namespace Breakout;

public class StateMachine
{
    public State Current { get; private set; }

    public void ChangeState(State next)
    {
        Current?.Exit();
        Current = next;
        Current.Enter();
    }

    public void Update(float delta) => Current?.Update(delta);

    public void Draw(SpriteBatch sb, SpriteFont font, Texture2D pixel)
        => Current?.Draw(sb, font, pixel);
}