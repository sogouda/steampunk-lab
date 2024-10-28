using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearTooth : MonoBehaviour
{
    public Gear gear;
    public GearToothSet set;

    public int toothId = 1;

    public float localScaleFactor
    {
        get
        {
            return -0.1f + gear.rootCylinder.localScaleFactor;
        }
    }

    private float localBonusLength
    {
        get
        {
            return 0.0005f * (gear.toothCount - 1);
        }
    }

    void Update()
    {
        if (gear == null || gear.toothSet == null)
            return;

        // Make this object a child of the gear tooth set.
        transform.parent = gear.toothSet.transform;

        if (toothId <= 0)
            toothId = 1;

        UpdateScale();

        transform.localPosition = new Vector3(
            0,
            0,
            gear.rootCylinder.localScaleFactor * 0.5f
        );

        transform.localRotation = new Quaternion();

        // Pivot the tooth around the gear.
        transform.RotateAround(
            gear.transform.position,
            Vector3.up,
            (360.0f / gear.toothCount) * (toothId - 1.0f)
        );
    }

    private void UpdateScale()
    {
        transform.localScale = new Vector3(
            transform.localScale.x,
            transform.localScale.y,
            0.25f + localBonusLength
        );
    }
}
