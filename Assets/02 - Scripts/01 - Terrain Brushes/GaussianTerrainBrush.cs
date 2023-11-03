using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaussianTerrainBrush
{
    public static void Draw_(CustomTerrain terrain, int x, int z, float height, int radius, bool shape_bool, float maxHeight, float standardDeviation)
    {
        if (shape_bool)
        {
            int radiusSquared = radius * radius;

            for (int zi = -radius; zi <= radius; zi++)
            {
                for (int xi = -radius; xi <= radius; xi++)
                {
                    int distanceSquared = xi * xi + zi * zi;

                    if (distanceSquared <= radiusSquared)
                    {
                        float distance = Mathf.Sqrt(xi * xi + zi * zi);
                        float gaussValue = Mathf.Exp(-(distance * distance) / (2 * standardDeviation * standardDeviation));
                        float currentHeight = terrain.get(x + xi, z + zi);

                        currentHeight += height * gaussValue;
                        currentHeight = Mathf.Clamp(currentHeight, 0.0f, maxHeight);

                        terrain.set(x + xi, z + zi, currentHeight);
                    }
                }
            }
        }
        else
        {
            for (int zi = -radius; zi <= radius; zi++)
            {
                for (int xi = -radius; xi <= radius; xi++)
                {
                    float distance = Mathf.Sqrt(xi * xi + zi * zi);
                    float gaussValue = Mathf.Exp(-(distance * distance) / (2 * standardDeviation * standardDeviation));
                    float currentHeight = terrain.get(x + xi, z + zi);

                    currentHeight += height * gaussValue;
                    currentHeight = Mathf.Clamp(currentHeight, 0.0f, maxHeight);

                    terrain.set(x + xi, z + zi, currentHeight);
                }
            }
        }
    }
}
