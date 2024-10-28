using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GearToothSet : MonoBehaviour
{
    public Gear gear;

    void Update()
    {
        // Make this object a child of the gear.
        transform.parent = gear.transform;
    }
}
