using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace breakout_mono;

public class Ball
{
    public Vector2 Position;
    public Vector2 Size = new(12, 12);
    public Color Color = new(255, 102, 102);
    public Vector2 Velocity;
    
    public Rectangle Bounds => new((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);

    public Ball(Vector2 startPosition)
    {
        Position = startPosition;
    }
    
    public void Draw(SpriteBatch sb, Texture2D pixel)
    {
        sb.Draw(pixel, Bounds, Color);
    }
}