using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothingTerrainBrush
{
    private static float[,] gaussianKernel;

    public static void Initialize(int kernelSize, float smoothingFactor)
    {
        gaussianKernel = GenerateGaussianKernel(kernelSize, smoothingFactor);
    }

    public static void Draw_(CustomTerrain terrain, int x, int z, int radius, bool shape_bool, float maxHeight)
    {
        float terrainX0 = terrain.transform.position.x;
        float terrainZ0 = terrain.transform.position.z;
        float terrainX1 = terrainX0 + terrain.terrainSize().x + 12; // works better with +12, don't understand why
        float terrainZ1 = terrainZ0 + terrain.terrainSize().z + 12;

        if (shape_bool)
        {
            int diameter = radius * 2;
            int kernelSize = diameter + 1;

            int kernelCenter = kernelSize / 2;
            for (int zi = -kernelCenter; zi <= kernelCenter; zi++)
            {
                for (int xi = -kernelCenter; xi <= kernelCenter; xi++)
                {
                    float weight = gaussianKernel[zi + kernelCenter, xi + kernelCenter];
                    int sqrDist = xi * xi + zi * zi;
                    if (sqrDist <= radius * radius)
                    {
                        float currentHeight = terrain.get(x + xi, z + zi);
                        float smoothedHeight = ApplyGaussianWeight(terrain, x + xi, z + zi, weight);
                        smoothedHeight = Mathf.Clamp(smoothedHeight, 0f, maxHeight);

                        if (x + xi >= terrainX0 && x + xi <= terrainX1 && z + zi >= terrainZ0 && z + zi <= terrainZ1)
                            terrain.set(x + xi, z + zi, smoothedHeight);
                    }
                }
            }
        }
        else
        {
            int kernelSize = radius * 2 + 1;

            for (int zi = -radius; zi <= radius; zi++)
            {
                for (int xi = -radius; xi <= radius; xi++)
                {
                    float currentHeight = terrain.get(x + xi, z + zi);
                    float smoothedHeight = ApplyGaussianWeight(terrain, x + xi, z + zi, gaussianKernel);
                    smoothedHeight = Mathf.Clamp(smoothedHeight, 0f, maxHeight);

                    if (x + xi >= terrainX0 && x + xi <= terrainX1 && z + zi >= terrainZ0 && z + zi <= terrainZ1)
                        terrain.set(x + xi, z + zi, smoothedHeight);
                }
            }
        }
    }

    private static float ApplyGaussianWeight(CustomTerrain terrain, int x, int z, float weight)
    {
        float currentHeight = terrain.get(x, z);
        return Mathf.Lerp(currentHeight, weight, weight);
    }

    private static float ApplyGaussianWeight(CustomTerrain terrain, int x, int z, float[,] kernel)
    {
        int kernelSize = kernel.GetLength(0);
        int kernelCenter = kernelSize / 2;
        float result = 0;

        for (int zi = -kernelCenter; zi <= kernelCenter; zi++)
        {
            for (int xi = -kernelCenter; xi <= kernelCenter; xi++)
            {
                float weight = kernel[zi + kernelCenter, xi + kernelCenter];
                result += terrain.get(x + xi, z + zi) * weight;
            }
        }

        return result;
    }

    private static float[,] GenerateGaussianKernel(int size, float smoothingFactor)
    {
        float[,] kernel = new float[size, size];
        float sum = 0;
        int kernelCenter = size / 2;

        if (smoothingFactor == 0)
        {
            float uniformWeight = 1.0f / (size * size);
            for (int zi = 0; zi < size; zi++)
            {
                for (int xi = 0; xi < size; xi++)
                {
                    kernel[zi, xi] = uniformWeight;
                    sum += uniformWeight;
                }
            }
        }
        else
        {
            for (int zi = -kernelCenter; zi <= kernelCenter; zi++)
            {
                for (int xi = -kernelCenter; xi <= kernelCenter; xi++)
                {
                    float exponent = -(xi * xi + zi * zi) / (2 * smoothingFactor * smoothingFactor);
                    kernel[zi + kernelCenter, xi + kernelCenter] = Mathf.Exp(exponent);
                    sum += kernel[zi + kernelCenter, xi + kernelCenter];
                }
            }
        }

        // Normalize the kernel to ensure the sum of weights is 1
        for (int zi = -kernelCenter; zi <= kernelCenter; zi++)
        {
            for (int xi = -kernelCenter; xi <= kernelCenter; xi++)
            {
                kernel[zi + kernelCenter, xi + kernelCenter] /= sum;
            }
        }

        return kernel;
    }
}
