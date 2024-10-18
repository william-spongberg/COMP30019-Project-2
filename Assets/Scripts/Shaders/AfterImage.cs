using UnityEngine;

public class AfterImage : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private Vector3 scale = Vector3.one;
    [SerializeField]
    private int instanceCount = 100;
    [SerializeField]
    private float trailSpeed = 0.1f;

    private Matrix4x4[] matrices;
    private Vector3[] positions;
    private Material material;

    void Start()
    {
        matrices = new Matrix4x4[instanceCount];
        positions = new Vector3[instanceCount];

        SkinnedMeshRenderer meshRenderer = prefab.GetComponent<SkinnedMeshRenderer>();
        material = meshRenderer.material;
        material.enableInstancing = true;

        for (int i = 0; i < instanceCount; i++)
        {
            ResetInstance(i);
        }
    }

    void Update()
    {
        for (int i = instanceCount - 1; i > 0; i--)
        {
            positions[i] = Vector3.Lerp(positions[i], positions[i - 1], trailSpeed);
            matrices[i] = Matrix4x4.TRS(positions[i], Quaternion.identity, scale);
        }
        positions[0] = transform.position;
        matrices[0] = Matrix4x4.TRS(positions[0], Quaternion.identity, scale);
    }

    void OnRenderObject()
    {
        Graphics.DrawMeshInstanced(prefab.GetComponent<SkinnedMeshRenderer>().sharedMesh, 0, material, matrices);
    }

    void ResetInstance(int index)
    {
        positions[index] = transform.position;
        matrices[index] = Matrix4x4.TRS(positions[index], Quaternion.identity, scale);
    }
}