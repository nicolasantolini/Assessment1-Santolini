using Assessment1_Santolini.Entities;

namespace Assessment1_Santolini.Simulation
{
    /// <summary>
    /// This interface defines a method for calculating the score of a plane based on its properties and the current time.
    /// </summary>
    public interface IScoreCalculator
    {
        int CalculatePlaneScore(Plane plane, int currentTime);
    }
}