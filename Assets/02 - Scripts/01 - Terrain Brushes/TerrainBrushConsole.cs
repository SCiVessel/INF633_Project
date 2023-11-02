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

    private enum BrushType
    {
        Simple,
        Incremental,
    }
    [SerializeField]
    private BrushType type = BrushType.Simple;

    public bool fine_tuning_mode = true;

    private static bool hasGenerated = false;

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
