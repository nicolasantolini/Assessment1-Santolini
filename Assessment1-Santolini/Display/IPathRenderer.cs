using Assessment1_Santolini.Entities;
using Assessment1_Santolini.Simulation;

namespace Assessment1_Santolini.Display
{
    /// <summary>
    /// This interface defines methods for displaying the operations of the pathfinding process.
    /// </summary>
    public interface IPathRenderer
    {
        void PrintCompleteGrid(Grid grid, IScoreCalculator scoreCalculator, int currentTime, (int x, int y) currentPosition);
        void PrintCompletion(long elapsedMs);
        void PrintMove((int x, int y) nextPosition);
        void PrintStart((int x, int y) start, int maxSteps, int timeLimitMs);
        void PrintStepHeader(int step, long elapsedMs, (int x, int y) currentPosition, int planeValue, int score);
        void PrintSurroundingGrid(Grid grid, IScoreCalculator scoreCalculator, int currentTime, (int x, int y) currentPosition);
        void PrintTimeLimitReached(int timeLimitMs, int stepsCompleted);
    }
}