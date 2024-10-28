using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GearRootCylinder : MonoBehaviour
{
    private const float BaseHeight = 0.025f;

    float diameter = 1.0f;
    float height = BaseHeight;

    public Gear gear
    {
        get
        {
            if (transform.parent == null)
                return null;

            return transform.parent.GetComponent<Gear>();
        }
    }

    public float localScaleFactor
    {
        get
        {
            if (transform.localScale.x == transform.localScale.z)
                return transform.localScale.x;

            return transform.localScale.x + transform.localScale.z * 0.4f;
        }
    }

    private void Start()
    {
        foreach (var rootCylinder in GetComponentsInChildren<GearRootCylinder>())
        {
            if (rootCylinder == null || rootCylinder == this)
                continue;

            DestroyImmediate(rootCylinder.gameObject);
        }
    }

    void Update()
    {
        transform.localPosition = Vector3.zero;

        // Update the height of the root cylinder.
        height = BaseHeight;

        // Update the material.
        UpdateMaterial();

        // Update the size of the root cylinder.
        CalculateScale();
    }

    /// <summary>
    /// Calculate the diameter of the root cylinder.
    /// </summary>
    private void CalculateDiameter()
    {
        diameter = 0.15f + 0.02f * gear.toothCount * 1.4f;
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

    private void UpdateMaterial()
    {
        if (gear == null || gear.renderMaterial == null)
            return;

        GetComponent<MeshRenderer>().material = gear.renderMaterial;
    }
}
