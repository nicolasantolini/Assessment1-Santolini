using System;
using System.Collections.Generic;

namespace Assessment1_Santolini.Entities
{
    /// <summary>
    /// Represents a simple N x N grid of Planes.
    /// </summary>
    public class Grid
    {

        /// <summary>
        /// Size of the grid (N x N grid).
        /// </summary>
        public readonly int Size;
        /// <summary>
        /// Planes 2D array storing the grid's planes.
        /// </summary>
        private readonly Plane[,] _planes;

		/// <summary>
		/// Initializes a new instance of the <see cref="Grid"/> class with the specified size.
		/// Throws an exception if size is not a positive integer.
		/// </summary>
		/// <param name="size"></param>
		/// <exception cref="ArgumentException"></exception>
		public Grid(int size)
        {
            if (size <= 0)
                throw new ArgumentException("Grid size must be a positive integer.", nameof(size));

            Size = size;
            _planes = new Plane[size, size];
        }

		/// <summary>
		/// This method initializes a plane at the specified coordinates with the given base value.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="baseValue"></param>
		public void InitializePlane(int x, int y, int baseValue)
        {
            ValidateCoordinates(x, y);
            _planes[y, x] = new Plane(baseValue);
        }

		/// <summary>
		/// This method retrieves the plane at the specified coordinates.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public Plane GetPlane(int x, int y)
        {
            ValidateCoordinates(x, y);
            return _planes[y, x]; 
        }

        /// <summary>
        /// This method returns the coordinates of all valid neighboring planes (including diagonals)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public IEnumerable<(int x, int y)> GetNeighborCoordinates(int x, int y)
        {
            ValidateCoordinates(x, y);
            (int, int)[] directions = { (-1, -1), (-1, 0), (-1, 1), (0, -1), (0, 1), (1, -1), (1, 0), (1, 1) };

            foreach (var (dx, dy) in directions)
            {
                int newX = x + dx;
                int newY = y + dy;
                if (IsValidCoordinate(newX, newY))
                {
                    yield return (newX, newY);
                }
            }
        }

		/// <summary>
		/// This method checks if the given coordinates are within the grid bounds.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public bool IsValidCoordinate(int x, int y) => x >= 0 && x < Size && y >= 0 && y < Size;

		/// <summary>
		/// This method validates that the given coordinates are within the grid bounds.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		private void ValidateCoordinates(int x, int y)
        {
            if (!IsValidCoordinate(x, y))
                throw new ArgumentOutOfRangeException($"Coordinates ({x}, {y}) are outside the grid bounds.");
        }

    }
}