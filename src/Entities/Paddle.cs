using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Breakout;

public class Paddle
{
    public Vector2 Position;
    public Vector2 Size = new(96, 16);
    public Color Color = new(102, 178, 255);
    public float Speed = 300f;

    public Rectangle Bounds => new((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);

    public Paddle(Vector2 startPosition)
    {
        Position = startPosition;
    }

    public void Update(float delta, int viewportWidth)
    {
        var keys = Keyboard.GetState();
        float direction = 0f;
        if (keys.IsKeyDown(Keys.A)) direction = -1f;
        else if (keys.IsKeyDown(Keys.D)) direction = 1f;

        Position.X += direction * Speed * delta;

        if (Position.X < 0) Position.X = 0;
        if (Position.X + Size.X > viewportWidth) Position.X = viewportWidth - Size.X;
    }

    public void Draw(SpriteBatch sb, Texture2D pixel)
    {
        sb.Draw(pixel, Bounds, Color);
    }
}