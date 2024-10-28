using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class Gear : MonoBehaviour
{
    public int toothCount = 1;
    public float powerLevel = 0.0f;
    public float powerEfficiency = 1.0f;

    public bool hasInfinitePower = false;

    public GearRootCylinder rootCylinderPrefab;
    public GearTooth toothPrefab;

    public float materialDensity = 1.0f;
    public Material renderMaterial = null;
    public GearToothSet toothSet;

    private int previousToothCount = 0;

    public GearRootCylinder rootCylinder
    {
        get
        {
            return GetComponentInChildren<GearRootCylinder>();
        }
    }

    public GearTooth[] teeth
    {
        get
        {
            return toothSet.GetComponentsInChildren<GearTooth>();
        }
    }

    public float mass
    {
        get
        {
            return materialDensity + (toothCount * toothCount) * materialDensity * 0.01f;
        }
    }

    public void DrainPower(float amount = 1.0f)
    {
        if (hasInfinitePower)
        {
            return;
        }

        powerLevel = powerLevel - amount;

        if (powerLevel < 0.0f)
            powerLevel = 0.0f;
    }

    private void ApplyPoweredRotation()
    {
#if UNITY_EDITOR
        if (!EditorApplication.isPlaying)
        {
            return;
        }
#endif

        if (powerLevel <= 0.0f)
        {
            return;
        }

        var rotation = new Vector3(
            0,
            (1000.0f / toothCount) * powerLevel * Time.deltaTime,
            0
        );
        
        // transform.Rotate(rotation);

        var rigidbody = GetComponent<Rigidbody>();

        rigidbody.AddTorque(rotation);

        DrainPower(Time.deltaTime);
    }

    private void CombineMeshes()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);

            i++;
        }

        Mesh mesh = new Mesh();
        mesh.CombineMeshes(combine);
        transform.GetComponent<MeshFilter>().sharedMesh = mesh;
        transform.gameObject.SetActive(true);
    }

    private void DestroyAllTeeth()
    {
        foreach (var tooth in teeth)
        {
            if (tooth == toothPrefab)
                continue;

            if (Application.isEditor)
                DestroyImmediate(tooth.gameObject);
            else
                Destroy(tooth.gameObject);
        }
    }

    private void DestroyRootCylinder()
    {
        if (rootCylinder == null)
            return;

        if (Application.isEditor)
            DestroyImmediate(rootCylinder.gameObject);
        else
            Destroy(rootCylinder.gameObject);
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

    private void GenerateRootCylinder()
    {
        var rootCylinder = Instantiate(rootCylinderPrefab);

        rootCylinder.transform.parent = transform;
    }

    private void RegenerateRootCylinder()
    {
        DestroyRootCylinder();
        GenerateRootCylinder();
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
        UpdateMass();
        ApplyPoweredRotation();
    }

    private void UpdateMass()
    {
        GetComponent<Rigidbody>().mass = mass;
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
