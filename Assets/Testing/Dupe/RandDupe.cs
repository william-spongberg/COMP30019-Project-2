using UnityEngine;

public class RandDupe : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private Vector3 scale = Vector3.one;
    [SerializeField]
    private int instanceCount = 100;
    [SerializeField]
    private float range = 5.0f;
    [SerializeField]
    private float minLifetime = 0.1f;
    [SerializeField]
    private float maxLifetime = 1.0f;

    private Matrix4x4[] matrices;
    private float[] lifetimes;
    private Vector3[] positions;
    private SkinnedMeshRenderer meshRenderer;
    private Mesh mesh;
    private Material material;
    private int randRotation = 0;

    void Start()
    {
        matrices = new Matrix4x4[instanceCount];
        lifetimes = new float[instanceCount];
        positions = new Vector3[instanceCount];

        meshRenderer = prefab.GetComponent<SkinnedMeshRenderer>();
        mesh = meshRenderer.sharedMesh;
        material = meshRenderer.material;
        material.enableInstancing = true;

        for (int i = 0; i < instanceCount; i++)
        {
            ResetInstance(i);
        }
    }

    void Update()
    {
        for (int i = 0; i < instanceCount; i++)
        {
            lifetimes[i] -= Time.deltaTime;
            if (lifetimes[i] <= 0)
            {
                ResetInstance(i);
            }
            else
            {
                AddInstance(i);
            }
        }
    }

    void OnRenderObject()
    {
        // draw instances from matrices
        Graphics.DrawMeshInstanced(mesh, 0, material, matrices);
    }

    void ResetInstance(int index)
    {
        positions[index] = transform.position + new Vector3(
            Random.Range(-range, range),
            transform.position.y,
            Random.Range(-range, range)
        );
        lifetimes[index] = Random.Range(minLifetime, maxLifetime);

        randRotation = Random.Range(0, 360);
        matrices[index] = Matrix4x4.TRS(positions[index], Quaternion.Euler(0, randRotation, 0), scale);
    }

    void AddInstance(int index)
    {
        matrices[index] = Matrix4x4.TRS(positions[index], Quaternion.Euler(0, randRotation, 0), scale);
    }
}