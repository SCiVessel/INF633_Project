using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseTerrainBrush
{
    public static void Draw_(CustomTerrain terrain, int x, int z, int radius, float maxHeight, float frequency, float amplitude, bool shape_bool)
    {
        float terrainX0 = terrain.transform.position.x;
        float terrainZ0 = terrain.transform.position.z;
        float terrainX1 = terrainX0 + terrain.terrainSize().x + 12; // works better with +12, don't understand why
        float terrainZ1 = terrainZ0 + terrain.terrainSize().z + 12;

        for (int zi = -radius; zi <= radius; zi++)
        {
            for (int xi = -radius; xi <= radius; xi++)
            {
                if (shape_bool && xi * xi + zi * zi > radius * radius)
                {
                    continue;
                }

                float noiseValue = Mathf.PerlinNoise((x + xi) * frequency, (z + zi) * frequency) * amplitude;
                float currentHeight = terrain.get(x + xi, z + zi);
                float newHeight = currentHeight + noiseValue;

                newHeight = Mathf.Clamp(newHeight, 0f, maxHeight);

                if (x + xi >= terrainX0 && x + xi <= terrainX1 && z + zi >= terrainZ0 && z + zi <= terrainZ1)
                    terrain.set(x + xi, z + zi, newHeight);
            }
        }
    }
}
