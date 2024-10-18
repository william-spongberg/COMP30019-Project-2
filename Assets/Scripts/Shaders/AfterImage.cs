using UnityEngine;

public class AfterImage : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private Vector3 scale = Vector3.one;
    [SerializeField]
    private int instanceCount = 3;
    [SerializeField]
    private float trailSpeed = 0.1f;
    [SerializeField]
    private float glitchIntensity = 0.1f;

    private Matrix4x4[] matrices;
    private Vector3[] positions;
    private Material material;
    private Animator animator;

    void Start()
    {
        matrices = new Matrix4x4[instanceCount];
        positions = new Vector3[instanceCount];

        SkinnedMeshRenderer meshRenderer = prefab.GetComponent<SkinnedMeshRenderer>();
        material = meshRenderer.material;
        material.enableInstancing = true;

        animator = GetComponent<Animator>();

        for (int i = 0; i < instanceCount; i++)
        {
            ResetInstance(i);
        }
    }

    void Update()
    {
        for (int i = instanceCount - 1; i > 0; i--)
        {
            // trail behind previous instance
            positions[i] = Vector3.Lerp(positions[i], positions[i - 1], trailSpeed);
            // randomly offset ('glitch')
            positions[i] += new Vector3(
                Random.Range(-glitchIntensity, glitchIntensity),
                Random.Range(-glitchIntensity, glitchIntensity),
                Random.Range(-glitchIntensity, glitchIntensity)
            );
            matrices[i] = Matrix4x4.TRS(positions[i], Quaternion.identity, scale);
        }

        // Apply animator's transformation to the first instance
        Transform animatedTransform = animator.transform;
        positions[0] = animatedTransform.position;
        matrices[0] = Matrix4x4.TRS(positions[0], animatedTransform.rotation, scale);
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