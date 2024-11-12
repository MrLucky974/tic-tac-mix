using UnityEngine;

namespace LuckiusDev.Utils
{
    /// <summary>
    /// Utility functions for mathematical operations commonly used in game development.
    /// </summary>
    public static class JMath
    {
        public static Vector3 GetVectorFromAngle(float angle)
        {
            float radians = angle * Mathf.Deg2Rad;
            return new Vector3(Mathf.Cos(radians), Mathf.Sin(radians));
        }

        public static float GetAngleFromVectorFloat(Vector3 dir)
        {
            dir = dir.normalized;
            float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            if (n < 0) n += 360;

            return n;
        }

        public static int GetAngleFromVector(Vector3 dir)
        {
            dir = dir.normalized;
            float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            if (n < 0) n += 360;
            int angle = Mathf.RoundToInt(n);

            return angle;
        }

        public static void ApproximativeRectangle(float area, out int w, out int h)
        {
            h = (int)Mathf.Round(Mathf.Sqrt(area));
            w = Mathf.CeilToInt(area / h);
        }

        /// <summary>
        /// Calculates the optimal size of a square grid given the total number of elements and the size of each element.
        /// </summary>
        /// <param name="elementCount">Total number of elements.</param>
        /// <param name="elementSize">Size of each element.</param>
        /// <returns>A tuple containing the side count and the total size of the square grid.</returns>
        public static (int sideCount, int squareSize) OptimalSquare(int elementCount, int elementSize)
        {
            var sideCount = Mathf.CeilToInt(Mathf.Sqrt(elementCount));
            return (sideCount, sideCount * elementSize);
        }

        /// <summary>
        /// Converts 2D coordinates to a 1D index in a grid of a given size.
        /// </summary>
        /// <param name="position">2D coordinates (x, z).</param>
        /// <param name="size">Size of the grid.</param>
        /// <returns>The corresponding 1D index.</returns>
        public static int CoordinateToIndex((int x, int z) position, int size)
        {
            return position.x + size * position.z;
        }

        /// <summary>
        /// Converts a 1D index to 2D coordinates in a grid of a given size.
        /// </summary>
        /// <param name="i">1D index.</param>
        /// <param name="size">Size of the grid.</param>
        /// <returns>The corresponding 2D coordinates (x, z).</returns>
        public static (int x, int z) IndexToCoordinate(int i, int size)
        {
            var x = i / size;
            var z = i % size;
            return (x, z);
        }

        /// <summary>
        /// Converts the rectangle of a RectTransform from Canvas space to World space.
        /// Returns the rectangle in world space coordinates.
        /// </summary>
        /// <param name="rectTransform">The RectTransform to convert.</param>
        /// <returns>A Rect representing the world-space bounds of the RectTransform.</returns>
        public static Rect GetWorldSpaceRect(this RectTransform rectTransform)
        {
            // Get the four world corners of the RectTransform
            Vector3[] worldCorners = new Vector3[4];
            rectTransform.GetWorldCorners(worldCorners);

            // Calculate the width and height of the Rect in world space
            float width = Vector3.Distance(worldCorners[0], worldCorners[3]);
            float height = Vector3.Distance(worldCorners[0], worldCorners[1]);

            // Create and return a Rect based on the bottom-left corner in world space and calculated width and height
            return new Rect(worldCorners[0], new Vector2(width, height));
        }

        /// <summary>
        /// Generates a random point within an annulus (a ring-shaped area) around a given origin.
        /// </summary>
        /// <param name="origin">Center of the annulus.</param>
        /// <param name="minRadius">Minimum distance from the origin.</param>
        /// <param name="maxRadius">Maximum distance from the origin.</param>
        /// <returns>A random point within the annulus.</returns>
        public static Vector2 RandomPointInAnnulus(Vector2 origin, float minRadius, float maxRadius)
        {
            var randomDirection = (Random.insideUnitCircle * origin).normalized;

            var randomDistance = Random.Range(minRadius, maxRadius);

            var point = origin + randomDirection * randomDistance;

            return point;
        }

        /// <summary>
        /// Simulates a headbob effect using sine and cosine functions.
        /// </summary>
        /// <param name="time">Current time.</param>
        /// <param name="frequency">Frequency of the headbob.</param>
        /// <param name="amplitude">Amplitude of the headbob.</param>
        /// <returns>The simulated headbob position.</returns>
        public static Vector2 Headbob(float time, float frequency, float amplitude)
        {
            var position = Vector2.zero;

            position.y = Mathf.Sin(time * frequency) * amplitude;
            position.x = Mathf.Cos(time * frequency / 2f) * amplitude;

            return position;
        }

        /// <summary>
        /// Wraps a value within a given range.
        /// </summary>
        /// <param name="a">Minimum value of the range.</param>
        /// <param name="b">Maximum value of the range.</param>
        /// <param name="x">Value to wrap.</param>
        /// <returns>The wrapped value within the specified range.</returns>
        public static int WrapValue(int a, int b, int x)
        {
            var range = b - a + 1;
            var wrappedValue = (x - a) % range;

            if (wrappedValue < 0)
            {
                wrappedValue += range;
            }

            return wrappedValue + a;
        }

        /// <summary>
        /// Generates a random direction in a two-dimensional plane.
        /// </summary>
        /// <remarks>
        /// This method generates a random direction within a flat plane (x, z) by utilizing the Unity Random.insideUnitCircle method
        /// and then converting it into a Vector3 with a Y-component of 0. The resulting vector is normalized to ensure it has a magnitude of 1.
        /// </remarks>
        /// <returns>A Vector3 representing the random direction in the flat plane.</returns>
        public static Vector3 GenerateRandomFlatDirection()
        {
            // Generate a random point inside the unit circle in 2D
            var randomPoint = Random.insideUnitCircle;

            // Create a Vector3 with the randomPoint's x and y components and a 0 z-component
            var direction = new Vector3(randomPoint.x, 0f, randomPoint.y);

            // Normalize the vector to ensure it has a magnitude of 1
            return direction.normalized;
        }

        /// <summary>
        /// Remaps a value from one range to another.
        /// </summary>
        /// <param name="value">The value to remap.</param>
        /// <param name="fromMin">The minimum of the original range.</param>
        /// <param name="fromMax">The maximum of the original range.</param>
        /// <param name="toMin">The minimum of the target range.</param>
        /// <param name="toMax">The maximum of the target range.</param>
        /// <returns>The remapped value in the target range.</returns>
        public static float Remap(float value, float fromMin, float fromMax, float toMin, float toMax)
        {
            return toMin + (value - fromMin) * (toMax - toMin) / (fromMax - fromMin);
        }

        #region Numeric Approximation

        public static bool IsApproxEqual(int a, int b, float margin)
        {
            return Mathf.Abs(a - b) <= margin;
        }

        public static bool IsApproxEqual(int a, int b)
        {
            return IsApproxEqual(a, b, Mathf.Epsilon);
        }

        public static bool IsApproxEqual(float a, float b, float margin)
        {
            return Mathf.Abs(a - b) <= margin;
        }

        public static bool IsApproxEqual(float a, float b)
        {
            return IsApproxEqual(a, b, Mathf.Epsilon);
        }

        public static bool IsApproxEqual(double a, double b, double margin)
        {
            return Mathf.Abs((float)(a - b)) <= margin;
        }

        public static bool IsApproxEqual(double a, double b)
        {
            return IsApproxEqual(a, b, double.Epsilon);
        }

        #endregion
    }
}
