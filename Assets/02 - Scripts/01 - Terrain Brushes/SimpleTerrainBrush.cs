using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TerrainBrushConsole;

public class SimpleTerrainBrush
{
    public static void Draw_(CustomTerrain terrain, int x, int z, float height, int radius, bool shape_bool)
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
                        if (x + xi >= terrainX0 && x + xi <= terrainX1 && z + zi >= terrainZ0 && z + zi <= terrainZ1)
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
                    if (x + xi >= terrainX0 && x + xi <= terrainX1 && z + zi >= terrainZ0 && z + zi <= terrainZ1)
                        terrain.set(x + xi, z + zi, height);
                }
            }
        }
    }
}
