using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class QuadrupedAutoController : MonoBehaviour
{
    public bool target_adsorption;

    private bool hasTarget;

    private float angleFromBrain;

    public float max_speed;
    protected Terrain terrain;
    protected CustomTerrain cterrain;
    protected float width, height;

    private int[,] detailMap;

    private Animal scriptAnimals;
    private QuadrupedProceduralMotion scriptQuadruped;

    private GameObject newGoal;

    void Start()
    {
        target_adsorption = false;

        hasTarget = false;

        max_speed = 0.5f;

        newGoal = new GameObject("NewGoal");
        newGoal.transform.parent = transform;

        scriptAnimals = GetComponent<Animal>();
        if (scriptAnimals == null)
            Debug.LogError("Script 'Animal' is not found on the same GameObject!");
        scriptQuadruped = GetComponent<QuadrupedProceduralMotion>();
        if (scriptQuadruped == null)
            Debug.LogError("Script 'QuadrupedProceduralMotion' is not found on the same GameObject!");

        terrain = Terrain.activeTerrain;
        cterrain = terrain.GetComponent<CustomTerrain>();
        width = terrain.terrainData.size.x;
        height = terrain.terrainData.size.z;

        scriptQuadruped.minDistToGoal = 0.0f;
        detailMap = cterrain.getDetails();
    }

    void Update()
    {
        angleFromBrain = scriptAnimals.getAngle();
        if (target_adsorption)
        {
            if (!hasTarget)
            {
                Vector3 gameObjectPosition = gameObject.transform.position;
                float shortestDistance = float.MaxValue;
                Vector3 nearestPoint = Vector3.zero;
                List<Vector3> visibleCoord = scriptAnimals.GetVisibleCoordinates();
                foreach (Vector3 spot in visibleCoord)
                {
                    if (detailMap[(int)spot.x, (int)spot.z] > 0)
                    {
                        float distance = Vector3.Distance(gameObjectPosition, spot);
                        if (distance < shortestDistance)
                        {
                            shortestDistance = distance;
                            nearestPoint = spot;
                        }
                    }
                }

                if (nearestPoint != Vector3.zero)
                {
                    newGoal.transform.position = nearestPoint;
                }
                else
                {
                    Vector3 scale = terrain.terrainData.heightmapScale;
                    Vector3 v = newGoal.transform.rotation * Vector3.forward * max_speed;
                    Vector3 loc = newGoal.transform.position + v;
                    if (loc.x < 0)
                        loc.x += width;
                    else if (loc.x > width)
                        loc.x -= width;
                    if (loc.z < 0)
                        loc.z += height;
                    else if (loc.z > height)
                        loc.z -= height;
                    loc.y = cterrain.getInterp(loc.x / scale.x, loc.z / scale.z);
                    newGoal.transform.position = loc;
                }

                newGoal.transform.rotation = Quaternion.AngleAxis(angleFromBrain, gameObject.transform.up);
                scriptQuadruped.updateGoal(newGoal.transform);
            }
        }
        else
        {
            Vector3 scale = terrain.terrainData.heightmapScale;
            Vector3 v = newGoal.transform.rotation * Vector3.forward * max_speed;
            Vector3 loc = newGoal.transform.position + v;
            if (loc.x < 0)
                loc.x += width;
            else if (loc.x > width)
                loc.x -= width;
            if (loc.z < 0)
                loc.z += height;
            else if (loc.z > height)
                loc.z -= height;
            loc.y = cterrain.getInterp(loc.x / scale.x, loc.z / scale.z);
            newGoal.transform.position = loc;

            newGoal.transform.rotation = Quaternion.AngleAxis(angleFromBrain, gameObject.transform.up);
            scriptQuadruped.updateGoal(newGoal.transform);
        }

        hasTarget = (detailMap[(int)newGoal.transform.position.x, (int)newGoal.transform.position.z] > 0);
        //Debug.Log(hasTarget);
    }
}
