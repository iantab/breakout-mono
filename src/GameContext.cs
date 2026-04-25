namespace Breakout;

public class GameContext
{
    public Paddle Paddle;
    public Ball Ball;
    public System.Collections.Generic.List<Brick> Bricks;
    public int Score;
    public int Lives = 3;
    public StateMachine Machine;
    public int ViewportWidth;
    public int ViewportHeight;

    public void ResetGame()
    {
        Score = 0;
        Lives = 3;
        Bricks = LevelMaker.Generate(level: 1, ViewportWidth);
        Ball.Reset();
    }
}