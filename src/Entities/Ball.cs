using System.Collections.Generic;
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

    public bool TryBouncePaddle(Paddle paddle)
    {
        if (!Bounds.Intersects(paddle.Bounds)) return false;

        if (Velocity.Y < 0) return false;

        float paddleCenterX = paddle.Position.X + paddle.Size.X / 2f;
        float offsetX = (Position.X - paddleCenterX) / (paddle.Size.X / 2f);
        offsetX = System.Math.Clamp(offsetX, -1f, 1f);

        float speed = Velocity.Length();
        float maxAngle = MathHelper.ToRadians(60f);
        float angle = offsetX * maxAngle;
        Velocity = new Vector2(
            (float)System.Math.Sin(angle), -(float)System.Math.Cos(angle)) * speed;

        Position.Y = paddle.Position.Y - Radius;
        return true;
    }

    public Brick TryBounceBricks(List<Brick> bricks, float delta)
    {
        var prevPos = Position - Velocity * delta;
        var prevBounds = new Rectangle(
            (int)(prevPos.X - Radius), (int)(prevPos.Y - Radius), (int)(Radius * 2), (int)(Radius * 2));

        foreach (var brick in bricks)
        {
            if (brick.Destroyed || !Bounds.Intersects(brick.Bounds)) continue;

            bool wasAbove = prevBounds.Bottom <= brick.Bounds.Top;
            bool wasBelow = prevBounds.Top >= brick.Bounds.Bottom;
            bool wasLeft = prevBounds.Right <= brick.Bounds.Left;
            bool wasRight = prevBounds.Left >= brick.Bounds.Right;

            if (wasAbove)
            {
                Velocity.Y = -System.Math.Abs(Velocity.Y);
                Position.Y = brick.Bounds.Top - Radius;
            }
            else if (wasBelow)
            {
                Velocity.Y = System.Math.Abs(Velocity.Y);
                Position.Y = brick.Bounds.Bottom + Radius;
            }
            else if (wasLeft)
            {
                Velocity.X = -System.Math.Abs(Velocity.X);
                Position.X = brick.Bounds.Left - Radius;
            }
            else if (wasRight)
            {
                Velocity.X = System.Math.Abs(Velocity.X);
                Position.X = brick.Bounds.Right + Radius;
            }
            else
            {
                Velocity.Y = -Velocity.Y;
            }

            brick.Destroyed = true;
            return brick;
        }

        return null;
    }
}