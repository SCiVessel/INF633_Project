using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseTerrainBrush
{
    public static void Draw_(CustomTerrain terrain, int x, int z, int radius, float maxHeight, float frequency, float amplitude, bool shape_bool)
    {
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
                terrain.set(x + xi, z + zi, newHeight);
            }
        }
    }
}
