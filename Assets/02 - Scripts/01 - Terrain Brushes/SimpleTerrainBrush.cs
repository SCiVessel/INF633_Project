using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TerrainBrushConsole;

public class SimpleTerrainBrush
{
    public static void Draw_(CustomTerrain terrain, int x, int z, float height, int radius, bool shape_bool)
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
                        terrain.set(x + xi, z + zi, height);
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
                    terrain.set(x + xi, z + zi, height);
                }
            }
        }
    }
}
