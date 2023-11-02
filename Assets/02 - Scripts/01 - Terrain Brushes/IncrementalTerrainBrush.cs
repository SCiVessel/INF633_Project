using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncrementalTerrainBrush
{
    public static void Draw_(CustomTerrain terrain, int x, int z, float height, int radius, bool shape_bool, float maxHeight)
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
                        float currentHeight = terrain.get(x + xi, z + zi);
                        currentHeight += height;

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
                    float currentHeight = terrain.get(x + xi, z + zi);
                    currentHeight += height;

                    currentHeight = Mathf.Clamp(currentHeight, 0.0f, maxHeight);

                    terrain.set(x + xi, z + zi, currentHeight);
                }
            }
        }
    }
}
