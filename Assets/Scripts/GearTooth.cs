using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
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
            var value = 0.005f * (gear.toothCount - 1);

            return value;
        }
    }

    private void Start()
    {
        name = "Gear Tooth";

        transform.localScale = new Vector3(
            transform.localScale.x * 2.4f,
            transform.localScale.y,
            transform.localScale.z
        );
    }

    void Update()
    {
        if (gear == null || gear.rootCylinder == null || gear.toothSet == null)
            return;

        // Make this object a child of the gear tooth set.
        transform.parent = gear.toothSet.transform;

        if (toothId <= 0)
            toothId = 1;

        UpdateMaterial();
        UpdateScale();

        transform.localPosition = new Vector3(
            0,
            0,
            0.035f + (localBonusLength * 0.5f + gear.rootCylinder.localScaleFactor * 0.15f) * 2.0f
        );

        transform.localRotation = new Quaternion();

        // Pivot the tooth around the gear.
        transform.RotateAround(
            gear.transform.position,
            Vector3.up,
            (360.0f / gear.toothCount) * (toothId - 1.0f)
        );

        // transform.Rotate(new Vector3(0, 0, 90));
    }

    private void UpdateMaterial()
    {
        if (gear == null || gear.renderMaterial == null)
            return;

        foreach (var meshRenderer in GetComponentsInChildren<MeshRenderer>())
        {
            meshRenderer.material = gear.renderMaterial;
        }
    }

    private void UpdateScale()
    {
        transform.localScale = new Vector3(
            transform.localScale.x,
            transform.localScale.y,
            1.0f + localBonusLength * 5.0f
        );
    }
}
