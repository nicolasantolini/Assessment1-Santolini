namespace Assessment1_Santolini.Entities
{
    /// <summary>
    /// Represents a plane in a grid with a value that can change based on visitation time.
    /// </summary>
    public class Plane
    {
        private readonly int _baseValue;

        /// <summary>
        /// Gets the last time this plane was visited. -1 indicates never visited.
        /// </summary>
        public int LastVisitedTime { get; private set; } = -1; 

        /// <summary>
        /// Gets the base value of this plane.
        /// </summary>
        public int Value => _baseValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="Plane"/> class.
        /// </summary>
        /// <param name="value">The base value of the plane.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if value is negative.</exception>
        public Plane(int value)
        {
            ValidateNonNegativeValue(value, nameof(value), "Plane value cannot be negative.");
            _baseValue = value;
        }

        /// <summary>
        /// Marks this plane as visited at the specified time.
        /// </summary>
        /// <param name="time">The current time to record as visit time.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if time is negative.</exception>
        public void Visit(int time)
        {
            ValidateNonNegativeValue(time, nameof(time), "Time cannot be negative.");

            if(time < LastVisitedTime)
                throw new ArgumentOutOfRangeException(nameof(time), "Visit time cannot be earlier than the last visited time.");

            LastVisitedTime = time;
        }

       

        /// <summary>
        /// Validates that a value is non-negative.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="paramName">The name of the parameter being validated.</param>
        /// <param name="errorMessage">The error message to use if validation fails.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if value is negative.</exception>
        private void ValidateNonNegativeValue(int value, string paramName, string errorMessage)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(paramName, errorMessage);
        }
    }
}
