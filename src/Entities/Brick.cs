using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Breakout;

public class Brick
{
    public Vector2 Position;
    public Vector2 Size = new(48, 16);
    public int Tier;
    public bool Destroyed;

    private static readonly Color[] Palette =
    {
        new(100, 156, 255), // blue
        new(108, 191, 46), // green
        new(217, 87, 99), // red
        new(215, 123, 186), // purple
        new(251, 242, 54), // gold
    };

    public Color Color => Palette[Math.Clamp(Tier, 0, Palette.Length - 1)];

    public Rectangle Bounds =>
        new((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);

    public Brick(Vector2 position, int tier)
    {
        Position = position;
        Tier = tier;
    }

    public int Points => 50 + Tier * 50;

    public void Draw(SpriteBatch sb, Texture2D pixel)
    {
        if (Destroyed) return;
        sb.Draw(pixel, Bounds, Color);
        var outline = new Rectangle(Bounds.X, Bounds.Y, Bounds.Width, 1);
        sb.Draw(pixel, outline, new Color(0, 0, 0, 80));
    }
}