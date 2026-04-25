using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Breakout;

public class Paddle
{
    public Vector2 Position;
    public Vector2 Size = new(96, 16);
    public Color Color = new(102, 178, 255);
    
    public Rectangle Bounds => new((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);

    public Paddle(Vector2 startPosition)
    {
        Position = startPosition;
    }

    public void Draw(SpriteBatch sb, Texture2D pixel)
    {
        sb.Draw(pixel, Bounds, Color);
    }
}