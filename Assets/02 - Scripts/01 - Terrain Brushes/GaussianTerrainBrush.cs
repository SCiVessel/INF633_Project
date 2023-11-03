using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaussianTerrainBrush
{
    public static void Draw_(CustomTerrain terrain, int x, int z, float height, int radius, bool shape_bool, float maxHeight, float standardDeviation)
    {
        float terrainX0 = terrain.transform.position.x;
        float terrainZ0 = terrain.transform.position.z;
        float terrainX1 = terrainX0 + terrain.terrainSize().x + 12; // works better with +12, don't understand why
        float terrainZ1 = terrainZ0 + terrain.terrainSize().z + 12;

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

                        if (x + xi >= terrainX0 && x + xi <= terrainX1 && z + zi >= terrainZ0 && z + zi <= terrainZ1)
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

                    if (x + xi >= terrainX0 && x + xi <= terrainX1 && z + zi >= terrainZ0 && z + zi <= terrainZ1)
                        terrain.set(x + xi, z + zi, currentHeight);
                }
            }
        }
    }
}
