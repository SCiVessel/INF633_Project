using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static InstanceBrushConsole;

using Random = UnityEngine.Random;

public class TreeInstanceBrush
{
    public static List<Tuple<float, float>> Generate_coordinates(
        CustomTerrain terrain, float x, float z, int radius, bool shape_bool, int numTrees, bool random, 
        float minHeight, float maxHeight, bool height, 
        float maxSteepness, bool steepness, 
        float minDistance, bool distance
        )
    {
        List<Tuple<float, float>> tupleList = new List<Tuple<float, float>>();
        List<Tuple<float, float>> tupleListRandom = new List<Tuple<float, float>>();

        int count = 0;

        if (numTrees > 0 && random)
        {
            for (int i = 0; i < numTrees; i++)
            {
                float randomX = x;
                float randomZ = z;

                if (shape_bool)
                {
                    float angle = Random.Range(0f, 360f);
                    float dist = Random.Range(0f, radius);
                    randomX += dist * Mathf.Cos(angle * Mathf.Deg2Rad);
                    randomZ += dist * Mathf.Sin(angle * Mathf.Deg2Rad);
                }
                else
                {
                    randomX += Random.Range(-radius, radius);
                    randomZ += Random.Range(-radius, radius);
                }

                tupleListRandom.Add(new Tuple<float, float>(randomX, randomZ));
            }
            count++;
        }

        if (count == 0)
        {
            tupleList.Add(new Tuple<float, float>(x, z));
        }
        else
        {
            tupleList = tupleListRandom;
        }

        if (height)
        {
            float min_val = Math.Min(minHeight, maxHeight);
            float max_val = Math.Max(minHeight, maxHeight);

            List<Tuple<float, float>> tempList = new List<Tuple<float, float>>();

            foreach (var tuple in tupleList)
            {
                float val = terrain.getInterp(tuple.Item1, tuple.Item2);
                if (val >= min_val && val <= max_val)
                {
                    tempList.Add(tuple);
                }
            }
            tupleList = tempList;
        }

        if (steepness)
        {
            List<Tuple<float, float>> tempList = new List<Tuple<float, float>>();

            foreach (var tuple in tupleList)
            {
                float val = terrain.getSteepness(tuple.Item1, tuple.Item2);
                if (val <= maxSteepness)
                {
                    tempList.Add(tuple);
                }
            }
            tupleList = tempList;
        }

        if (distance)
        {
            List<Tuple<float, float>> tempList = new List<Tuple<float, float>>();
            List<TreeInstance> treeInstances = new List<TreeInstance>();

            int treeCount = terrain.getObjectCount();
            for (int i = 0; i < treeCount; i++)
            {
                TreeInstance tree = terrain.getObject(i);
                treeInstances.Add(tree);
            }

            foreach (var tuple in tupleList)
            {
                Vector3 currentPosition = new Vector3(tuple.Item1, 0.0f, tuple.Item2);
                bool flag = true;

                foreach (TreeInstance tree in treeInstances)
                {
                    Vector3 treePosition = terrain.getObjectLoc(tree);
                    float dist = Vector3.Distance(currentPosition, treePosition);

                    if (dist < minDistance)
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag == true)
                {
                    tempList.Add(tuple);
                }
            }
            tupleList = tempList;
        }

        return tupleList;
    }
}
