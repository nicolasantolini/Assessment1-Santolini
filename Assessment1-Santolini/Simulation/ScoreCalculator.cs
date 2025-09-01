using Assessment1_Santolini.Entities;

namespace Assessment1_Santolini.Simulation
{
    /// <summary>
    /// This class provides functionality to calculate the score of a plane based on its value, last visited time, and a regeneration time.
    /// </summary>
    public class ScoreCalculator : IScoreCalculator
    {
        private readonly int _regenerationTime; // Time needed to go from 0 to full value (in ms, e.g. when 100, for any plane, after 100ms it will have fully regenerated)

        /// <summary>
        /// Initializes a new instance of the <see cref="ScoreCalculator"/> class.
        /// </summary>
        /// <param name="regenerationTime"></param>
        /// <exception cref="ArgumentException"></exception>
        public ScoreCalculator(int regenerationTime = 100)
        {
            if (regenerationTime <= 0)
                throw new ArgumentException("Regeneration time must be a positive integer.", nameof(regenerationTime));
            _regenerationTime = regenerationTime;
        }

        /// <summary>
        /// Calculates the score of a given plane based on its value, last visited time, and the current time.
        /// </summary>
        /// <remarks>
        /// The algorithm works as follows:
        /// 1. If the plane has never been visited (LastVisitedTime == -1), its score is equal to its base value.
        /// 2. If the plane was visited at the current time or in the future (LastVisitedTime >= currentTime), its score is 0.
        /// 3. If the plane was visited in the past, its score is calculated based on the time elapsed since the last visit and the regeneration time.
        /// </remarks>
        /// <param name="plane"></param>
        /// <param name="currentTime"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public int CalculatePlaneScore(Plane plane, int currentTime)
        {
            if (plane == null)
                throw new ArgumentNullException(nameof(plane), "Plane cannot be null.");

            if (currentTime < 0)
                throw new ArgumentOutOfRangeException(nameof(currentTime), "Current time cannot be negative.");

            if (currentTime < plane.LastVisitedTime && plane.LastVisitedTime != -1)
                throw new ArgumentOutOfRangeException(nameof(currentTime), "Current time cannot be earlier than the plane's last visited time.");

            if (plane.Value <= 0)
            {
                return 0;
            }

            // If never visited, value is its base value
            if (plane.LastVisitedTime == -1)
            {
                return plane.Value;
            }

            int timeSinceLastVisit = currentTime - plane.LastVisitedTime;

            // If visited at the current time or in the future (not allowed operation), value is 0.
            if (timeSinceLastVisit <= 0)
            {
                return 0;
            }

            // Calculate the regenerated value.
            double rawFraction = (double)timeSinceLastVisit / _regenerationTime;
            double fractionRegenerated = Math.Min(1.0, rawFraction); // Set maximum to 1.0 (fully regenerated)

            int result = (int)(plane.Value * fractionRegenerated); 

            return result;
        }
    }
}
