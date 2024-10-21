using System.Collections.Generic;
using UnityEngine;

namespace LuckiusDev.Utils
{
    public static class ChaikinPathSmoothing
    {
        public static List<Vector3> GetSmoothPath(List<Vector3> points, int iterations = 4, float percent = 0.25f,
                bool closed = false)
        {
            for (int i = 0; i < iterations; i++)
            {
                points = GetPath(points, percent, closed);
            }

            return points;
        }

        static List<Vector3> GetPath(List<Vector3> points, float percent = 0.25f, bool closed = false)
        {
            Vector3 start = points[0];
            Vector3 end = points[^1];

            List<Vector3> path = new List<Vector3>();

            for (int i = 0; i < points.Count - 1; i++)
            {
                Vector3 pointA = points[i];
                Vector3 pointB = points[i + 1];

                float dx = pointB.x - pointA.x;
                //float dy = pointB.y - pointA.y;
                float dz = pointB.z - pointA.z;

                path.Add(new Vector3(
                                pointA.x + dx * percent,
                                pointA.y /*+ dy * percent*/,
                                pointA.z + dz * percent
                        )
                );

                path.Add(new Vector3(
                                pointA.x + dx * (1 - percent),
                                pointA.y /*+ dy * (1 - percent)*/,
                                pointA.z + dz * (1 - percent)
                        )
                );
            }

            if (!closed)
            {
                path.Add(end);
            }
            else
            {
                float dx = start.x - end.x;
                //float dy = start.y - end.y;
                float dz = start.z - end.z;

                path.Add(new Vector3(
                                end.x + dx * percent,
                                end.y /*+ dy * percent*/,
                                end.z + dz * percent
                        )
                );

                path.Add(new Vector3(
                                end.x + dx * (1 - percent),
                                end.y /*+ dy * (1 - percent)*/,
                                end.z + dz * (1 - percent)
                        )
                );
            }

            return path;
        }
    }
}