using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class InstanceBrushConsole : InstanceBrush
{
    const float maximumHeight = 100.0f;

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
        Tree
    }
    public BrushType type = BrushType.Tree;

    // Tree
    [HideInInspector]
    public bool random_mode = false;
    [SerializeField, HideInInspector]
    private int tree_number = 1;

    [HideInInspector]
    public bool height_control = false;
    [SerializeField, Range(0.0f, maximumHeight), HideInInspector]
    private float min_height = 0.0f;
    [SerializeField, Range(0.0f, maximumHeight), HideInInspector]
    private float max_height = 100.0f;

    [HideInInspector]
    public bool steepness_control = false;
    [SerializeField, Range(0.0f, 90.0f), HideInInspector]
    private float max_steepness = 60.0f;

    [HideInInspector]
    public bool distance_control = false;
    [SerializeField, HideInInspector]
    private float min_distance = 3.0f;


    public bool fine_tuning_mode = true;
    private static bool hasGenerated = false;



    public override void draw(float x, float z)
    {
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
            if (type == BrushType.Tree)
            {
                if (eraser_mode)
                {
                    terrain.RemoveTreeInstance(x, z, radius);
                }
                else
                {
                    List<Tuple<float, float>> tupleList = TreeInstanceBrush.Generate_coordinates(
                        terrain, x, z, radius, shape_is_round, tree_number, random_mode, 
                        min_height, max_height, height_control, 
                        max_steepness, steepness_control, 
                        min_distance, distance_control
                        );
                    foreach (var tuple in tupleList)
                    {
                        spawnObject(tuple.Item1, tuple.Item2);
                    }
                }
            }

            hasGenerated = true;
        }

        if (fine_tuning_mode && !eraser_mode)
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
        yield return new WaitForSeconds(0.2f);
        ResetGenerationFlag();
    }
}
