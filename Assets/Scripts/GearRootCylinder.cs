using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearRootCylinder : MonoBehaviour
{
    public Gear gear;

    float diameter = 0.2f;
    float height = 0.1f;

    public float localScaleFactor
    {
        get
        {
            if (transform.localScale.x == transform.localScale.z)
                return transform.localScale.x;

            return transform.localScale.x + transform.localScale.z * 0.5f;
        }
    }

    void Update()
    {
        // Make this object a child of the gear.
        transform.parent = gear.transform;

        // Update the size of the root cylinder.
        CalculateScale();
    }

    /// <summary>
    /// Calculate the diameter of the root cylinder.
    /// </summary>
    private void CalculateDiameter()
    {
        diameter = 0.15f + 0.05f * gear.toothCount;
    }

    /// <summary>
    /// Calculate the scale of the root cylinder.
    /// </summary>
    private void CalculateScale()
    {
        CalculateDiameter();

        transform.localScale = new Vector3(
            diameter,
            height,
            diameter
        );
    }
}
