using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTerrainBrush
{
    public static void Draw_(CustomTerrain terrain, int x, int z, int radius, float maxHeight, float intensity, bool shape_bool)
    {
        for (int zi = -radius; zi <= radius; zi++)
        {
            for (int xi = -radius; xi <= radius; xi++)
            {
                if (shape_bool && xi * xi + zi * zi > radius * radius)
                {
                    continue;
                }

                float randomHeightChange = Random.Range(-intensity, intensity);
                float currentHeight = terrain.get(x + xi, z + zi);
                float newHeight = currentHeight + randomHeightChange;

                newHeight = Mathf.Clamp(newHeight, 0f, maxHeight);
                terrain.set(x + xi, z + zi, newHeight);
            }
        }
    }
}
