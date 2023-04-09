using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGenerator
{
    private ShapeSettings shaperSettings;

    public ShapeGenerator(ShapeSettings shaperSettings)
    {
        this.shaperSettings = shaperSettings;
    }

    public Vector3 CalculatePointOnPlanet(Vector3 pointOnUnitSphere)
    {
        return pointOnUnitSphere * shaperSettings.planetRadius;
    }
}
