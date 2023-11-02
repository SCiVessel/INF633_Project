using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TerrainBrushConsole : TerrainBrush
{

    [SerializeField, Range(0, 100)]
    private float ranged_height = 20;

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

        if (type == BrushType.Simple || eraser_mode)
        {
            if (!hasGenerated)
            {
                SimpleTerrainBrush.Draw_(terrain, x, z, height, radius, shape_is_round);
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
        /*else if ()
        {

        }*/

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
