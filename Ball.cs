using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Breakout;

public class Ball
{
    public Vector2 Position;
    public float Radius = 6f;
    public Color Color = new(255, 102, 102);
    public Vector2 Velocity;

    public Rectangle Bounds => new(
        (int)(Position.X - Radius), (int)(Position.Y - Radius),
        (int)(Radius * 2), (int)(Radius * 2));

    public Ball(Vector2 startPosition)
    {
        Position = startPosition;
    }

    public void Draw(SpriteBatch sb)
    {
        sb.DrawCircle(Position, Radius, sides: 16, color: Color, thickness: Radius);
    }
}