using Microsoft.Xna.Framework.Graphics;

namespace Breakout;

public abstract class State
{
    public virtual void Enter()
    {
    }

    public virtual void Exit()
    {
    }

    public abstract void Update(float delta);
    public abstract void Draw(SpriteBatch sb, SpriteFont font, Texture2D pixel);
}