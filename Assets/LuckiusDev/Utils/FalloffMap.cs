using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LuckiusDev.Utils
{
    public static class FalloffMap
    {
        public static float[,] Generate( int mapWidth, int mapHeight, float falloffStart, float falloffEnd ) {
            float[,] heightMap = new float[mapWidth, mapHeight];

            for (int y = 0; y < mapHeight; y++) {
                for (int x = 0; x < mapWidth; x++) {
                    Vector2 position = new Vector2(
                        (float) x / mapWidth * 2 - 1,
                        (float) y / mapHeight * 2 - 1
                    );

                    // Find which value is closer to the edge
                    float t = Mathf.Max(Mathf.Abs(position.x), Mathf.Abs(position.y));

                    if (t < falloffStart) {
                        heightMap[x, y] = 1;
                    } else if (t > falloffEnd) {
                        heightMap[x, y] = 0;
                    } else {
                        heightMap[x, y] = Mathf.SmoothStep(1, 0, Mathf.InverseLerp(falloffStart, falloffEnd, t));
                    }
                }
            }

            return heightMap;
        }
    }
}