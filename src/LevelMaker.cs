using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Breakout;

public static class LevelMaker
{
    private const float BrickWidth = 48f;
    private const float BrickHeight = 16f;

    public static List<Brick> Generate(int level, int viewportWidth)
    {
        var bricks = new List<Brick>();
        var rng = new Random();

        int numRows = rng.Next(3, 7);
        int numCols = rng.Next(9, 14);
        int highestTier = Math.Min(4, level / 2 + 1);

        float totalWidth = numCols * BrickWidth;
        float startX = (viewportWidth - totalWidth) / 2f;
        float startY = 40f;

        for (int row = 0; row < numRows; row++)
        {
            int rowTier = rng.Next(0, highestTier + 1);
            for (int col = 0; col < numCols; col++)
            {
                bricks.Add(new Brick(new Vector2(startX + col * BrickWidth, startY + row * BrickHeight), rowTier));
            }
        }

        return bricks;
    }
}