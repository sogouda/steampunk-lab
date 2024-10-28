using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Gear : MonoBehaviour
{
    public int toothCount = 1;
    public float powerLevel = 0.0f;
    public float poweredRotationSpeed = 1.0f;
    
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

    public void ApplyPoweredRotation()
    {
        if (powerLevel <= 0.0f)
        {
            return;
        }

        var rotation = new Vector3(
            0,
            (poweredRotationSpeed * 1000.0f / toothCount) * powerLevel * Time.deltaTime,
            0
        );
        
        transform.Rotate(rotation);

        powerLevel = powerLevel - 1.0f * Time.deltaTime;

        if (powerLevel < 0.0f)
            powerLevel = 0.0f;
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
        ApplyPoweredRotation();
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
