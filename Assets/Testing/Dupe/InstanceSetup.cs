using UnityEngine;
using System.Collections.Generic;
using System;

public class InstanceSetup : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private int gridSize = 64;
    [SerializeField]
    private float spacing = 3f;
    [SerializeField]
    private Vector3 scale = Vector3.one;
    [SerializeField]
    private Vector3 offset = Vector3.zero;
    private GameObject player;
    private List<Matrix4x4> matrices;
    private Mesh mesh;
    private Material material;

    void Start() {
        // find prefab + its mesh
        if (prefab == null) {
            Debug.LogError("Prefab is not assigned.");
            return;
        }
        MeshRenderer meshRenderer = prefab.GetComponent<MeshRenderer>();
        if (meshRenderer == null) {
            Debug.LogError("Prefab does not have a MeshRenderer component.");
            return;
        }

        // find player object if using player location
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) {
            Debug.LogWarning("Player object not found. Creating empty player object at 0,0,0.");
            // create new empty player object at 0,0,0
            player = new GameObject("Player Empty");
            player.tag = "Player";
            player.transform.position = Vector3.zero;
        }
    
        // grab mesh and material
        mesh = prefab.GetComponent<MeshFilter>().sharedMesh;
        material = meshRenderer.sharedMaterial;
        material.enableInstancing = true;

        // initialize matrices list
        matrices = new List<Matrix4x4>();

        // update grid
        UpdateGrid();
    }

    void Update() {
        // update grid if player and prefab exist
        if (prefab == null || player == null ) return;

        UpdateGrid();
    }

    // update grid of gpu instances
    void UpdateGrid() {
        matrices.Clear();

        // calc offset of grid
        float gridOffset = (gridSize - 1) * spacing / 2;
        Vector3 playerPosition = player.transform.position;

        // iterate over each grid position
        for (int x = 0; x < gridSize; x++) {
            for (int z = 0; z < gridSize; z++) {
                // calc position of new instance
                Vector3 position = new Vector3(
                    Mathf.Floor((playerPosition.x + x * spacing - gridOffset) / spacing) * spacing,
                    0,
                    Mathf.Floor((playerPosition.z + z * spacing - gridOffset) / spacing) * spacing
                ) + offset;

                // check if overlapping with any existing colliding instances
                if (!IsOverlapping(position)) {
                    // if not, add instance matrix to list
                    matrices.Add(Matrix4x4.TRS(position, Quaternion.identity, scale));
                }
            }
        }
    }

    void OnRenderObject() {
        // draw instances from matrices
        if (mesh != null && material != null) {
            Graphics.DrawMeshInstanced(mesh, 0, material, matrices);
        }
    }

    private bool IsOverlapping(Vector3 position) {
        // check if any colliders overlap with position
        Collider[] colliders = Physics.OverlapBox(position, scale / 2);
        return colliders.Length > 0;
    }
}