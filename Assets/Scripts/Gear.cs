using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    public int toothCount = 1;
    public GearTooth toothPrefab;

    public GearRootCylinder rootCylinder;
    public GearToothSet toothSet;

    private int previousToothCount = 0;

    public GearTooth[] teeth
    {
        get
        {
            return toothSet.GetComponentsInChildren<GearTooth>();
        }
    }

    private void DestroyAllTeeth()
    {
        foreach (var tooth in teeth)
        {
            if (tooth == toothPrefab)
                continue;

            Destroy(tooth.gameObject);
        }
    }

    private void GenerateTeeth()
    {
        int remainingTeeth = toothCount;

        while (remainingTeeth > 0)
        {
            var tooth = Instantiate(toothPrefab);

            tooth.transform.parent = toothSet.transform;
            tooth.set = toothSet;
            tooth.gear = this;

            tooth.toothId = remainingTeeth--;
        }
    }

    private void RegenerateTeeth()
    {
        DestroyAllTeeth();
        GenerateTeeth();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        UpdateToothCount();
    }

    private void UpdateToothCount()
    {
        if (previousToothCount != toothCount)
        {
            RegenerateTeeth();
        }

        previousToothCount = toothCount;
    }
}
