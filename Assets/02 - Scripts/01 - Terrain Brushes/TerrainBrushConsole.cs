using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TerrainBrushConsole : TerrainBrush
{
    const float max_height = 100;

    [SerializeField, Range(-max_height, max_height)]
    private float ranged_height = 20.0f;

    [SerializeField]
    private bool eraser_mode = false;

    private enum BrushShape
    {
        Round,
        Square
    }
    [SerializeField]
    private BrushShape shape = BrushShape.Square;

    public enum BrushType
    {
        Simple,
        Incremental,
        Gaussian,
        Smooth,
        Random,
        Noise
    }
    public BrushType type = BrushType.Simple;

    public bool fine_tuning_mode = true;

    private static bool hasGenerated = false;

    [SerializeField, Range(0.0f, 100.0f), HideInInspector]
    private float standard_deviation = 10.0f;

    [SerializeField, Range(0.0f, 10.0f), HideInInspector]
    private float smoothing_factor = 5.0f;
    private float lastSmoothingFactor = -1;

    [SerializeField, HideInInspector]
    private int kernel_size = 3;
    private int lastKernelSize = -1;

    [SerializeField, Range(0.0f, 1.0f), HideInInspector]
    private float frequency = 0.01f;

    public override void draw(int x, int z)
    {
        float height;
        if (eraser_mode)
        {
            height = 0;
        }
        else
        {
            height = ranged_height;
        }

        bool shape_is_round;
        if (shape == BrushShape.Square)
        {
            shape_is_round = false;
        }
        else
        {
            shape_is_round = true;
        }

        if (!hasGenerated)
        {
            if (type == BrushType.Simple || eraser_mode)
            {
                SimpleTerrainBrush.Draw_(terrain, x, z, height, radius, shape_is_round);
            }
            else if (type == BrushType.Incremental)
            {
                IncrementalTerrainBrush.Draw_(terrain, x, z, height, radius, shape_is_round, max_height);
            }
            else if (type == BrushType.Gaussian)
            {
                GaussianTerrainBrush.Draw_(terrain, x, z, height, radius, shape_is_round, max_height, standard_deviation);
            }
            else if (type == BrushType.Smooth)
            {
                float currentSmoothingFactor = smoothing_factor;
                int currentKernalSize = kernel_size;
                if (currentSmoothingFactor != lastSmoothingFactor || currentKernalSize != lastKernelSize)
                {
                    SmoothingTerrainBrush.Initialize(kernel_size, currentSmoothingFactor);
                    lastSmoothingFactor = currentSmoothingFactor;
                    lastKernelSize = currentKernalSize;
                }
                SmoothingTerrainBrush.Draw_(terrain, x, z, radius, shape_is_round, max_height);
            }
            else if (type == BrushType.Random)
            {
                RandomTerrainBrush.Draw_(terrain, x, z,  radius, max_height, height, shape_is_round);
            }
            else if (type == BrushType.Noise)
            {
                NoiseTerrainBrush.Draw_(terrain, x, z, radius, max_height, frequency, height, shape_is_round);
            }


            hasGenerated = true;
        }

        if (fine_tuning_mode)
        {
            StartCoroutine(ResetGenerationFlagDelayed());
        }
        else
        {
            ResetGenerationFlag();
        }
    }

    private static void ResetGenerationFlag()
    {
        hasGenerated = false;
    }

    IEnumerator ResetGenerationFlagDelayed()
    {
        yield return new WaitForSeconds(0.05f);
        ResetGenerationFlag();
    }
}
