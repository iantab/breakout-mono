using System.Net.Security;
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

    private Vector2 _startPosition;

    private static readonly System.Random Rng = new();

    public Rectangle Bounds => new(
        (int)(Position.X - Radius), (int)(Position.Y - Radius),
        (int)(Radius * 2), (int)(Radius * 2));

    public Ball(Vector2 startPosition)
    {
        Position = startPosition;
        _startPosition = startPosition;
        Serve();
    }

    public void Serve()
    {
        Position = _startPosition;
        float vx = Rng.Next(-300, 301);
        if (System.Math.Abs(vx) < 80) vx = vx < 0 ? -80 : 80;
        float vy = -Rng.Next(180, 240);
        Velocity = new Vector2(vx, vy);
    }

    public void Reset()
    {
        Position = _startPosition;
        Velocity = Vector2.Zero;
    }

    public void Update(float delta, int viewportWidth, int viewportHeight)
    {
        Position += Velocity * delta;

        if (Position.X - Radius <= 0)
        {
            Position.X = Radius;
            Velocity.X = -Velocity.X;
        }
        else if (Position.X + Radius >= viewportWidth)
        {
            Position.X = viewportWidth - Radius;
            Velocity.X = -Velocity.X;
        }

        if (Position.Y - Radius <= 0)
        {
            Position.Y = Radius;
            Velocity.Y = -Velocity.Y;
        }
    }

    public void Draw(SpriteBatch sb)
    {
        sb.DrawCircle(Position, Radius, sides: 16, color: Color, thickness: Radius);
    }
}