using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

[System.Serializable]
public class SpawnObject
{
    public GameObject obj = null;
    public Vector3 dimensions = new(0, 0, 0);
    public Vector3 offsets = new(0, 0, 0);
    // public int spawnWeight = 10;
}

public class Generator : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField]
    private int radius = 30;
    [SerializeField]
    private float gap = 0f;
    [SerializeField]
    private Vector3 gridDimensions = new(0, 0, 0);
    private GameObject player;

    [Header("Perlin Noise Settings")]
    [SerializeField]
    private float noiseScale = 2f;
    [SerializeField]
    private float offsetX = 0;
    [SerializeField]
    private float offsetY = 0;

    [Header("Spawn Objects")]
    [SerializeField]
    private List<SpawnObject> spawnObjects = new List<SpawnObject>();
    [SerializeField]
    private Dictionary<Vector2Int, GameObject> objects = new Dictionary<Vector2Int, GameObject>();

    // ? keeps last 1000 objects in cache to reuse
    // * (this is to prevent constant instantiation and destruction of objects, which causes lag)
    [Header("Cache Settings")]
    [SerializeField]
    private int cacheSize = 1000;
    [SerializeField]
    private Queue<GameObject> spawnObjectsCache = new();

    [Header("Nav Mesh Settings")]
    // for nav mesh
    [SerializeField]
    private Boolean isBaking = false;
    [SerializeField]
    private NavMeshSurface navMeshSurface;
    [SerializeField]
    private float bakeInterval = 1f; // Time interval between bakes
    private Coroutine bakingCoroutine;
    private float lastBakeTime;

    void Start()
    {
        // generate random offsets
        offsetX = UnityEngine.Random.Range(0, 999999);
        offsetY = UnityEngine.Random.Range(0, 999999);

        // set current time as last bake time
        lastBakeTime = Time.time;

        // get player object
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player object not found.");
            return;
        }

        // update dimensions for each spawn object if not manually set
        for (int i = 0; i < spawnObjects.Count; i++)
        {
            if (spawnObjects[i].dimensions != null)
                spawnObjects[i].dimensions = spawnObjects[i].obj.GetComponent<Renderer>().bounds.size;
        }

        // TODO: need to all be same length, fix in future?
        // * for now just set grid size to size of first object in list
        gridDimensions.x = spawnObjects[0].dimensions.x;
        gridDimensions.y = spawnObjects[0].dimensions.y;
        gridDimensions.z = spawnObjects[0].dimensions.z;
    }

    void Update()
    {
        // get player 2d coords
        float playerX = player.transform.position.x;
        float playerZ = player.transform.position.z;

        // generate world
        WorldGeneration(playerX, playerZ);

        // bake the NavMesh periodically
        if (isBaking && Time.time - lastBakeTime >= bakeInterval)
        {
            if (bakingCoroutine != null)
                StopCoroutine(bakingCoroutine);
            bakingCoroutine = StartCoroutine(BakeNavMeshAsync());
            lastBakeTime = Time.time;
        }
    }

    void WorldGeneration(float playerX, float playerZ)
    {
        CreateObjects(playerX, playerZ);
        DestroyObjects(playerX, playerZ);
    }

    void CreateObjects(float playerX, float playerZ)
    {
        int startX = Mathf.FloorToInt((playerX - radius) / (gridDimensions.x + gap));
        int endX = Mathf.CeilToInt((playerX + radius) / (gridDimensions.x + gap));
        int startY = Mathf.FloorToInt((playerZ - radius) / (gridDimensions.z + gap));
        int endY = Mathf.CeilToInt((playerZ + radius) / (gridDimensions.z + gap));

        for (int x = startX; x <= endX; x++)
        {
            for (int y = startY; y <= endY; y++)
            {
                Vector2Int gridPosition = new Vector2Int(x, y);
                if (!objects.ContainsKey(gridPosition))
                {
                    // get noise value for current grid position
                    float noiseValue = GetPerlinNoiseValue(x, y, noiseScale, offsetX, offsetY);
                    SpawnObject sObj = GetObjectBasedOnNoise(noiseValue);
                    if (sObj != null)
                    {
                        Vector3 worldPosition = new Vector3(x * (gridDimensions.x + gap), -gridDimensions.y, y * (gridDimensions.z + gap));
                        Vector3 offset = sObj.offsets;
                        GameObject obj;
                
                        if (spawnObjectsCache.Count > 0)
                        {
                            // reuse object from cache
                            obj = spawnObjectsCache.Dequeue();
                            obj.transform.position = worldPosition + offset;
                            obj.transform.rotation = Quaternion.identity;
                            obj.SetActive(true);
                        }
                        else
                        {
                            obj = Instantiate(sObj.obj, worldPosition + offset, Quaternion.identity);
                        }
                
                        // ! in progress attempt to fix objects not being of same size in grid
                        // scale to grid scale if not the same
                        if (obj.GetComponent<Renderer>().bounds.size.x != gridDimensions.x)
                        {
                            float scale = gridDimensions.x / obj.GetComponent<Renderer>().bounds.size.x;
                            obj.transform.localScale = new Vector3(scale, scale, scale);
                        }
                        else if (obj.GetComponent<Renderer>().bounds.size.z != gridDimensions.z)
                        {
                            float scale = gridDimensions.z / obj.GetComponent<Renderer>().bounds.size.z;
                            obj.transform.localScale = new Vector3(scale, scale, scale);
                        }
                
                        objects[gridPosition] = obj;
                
                        // new object created, bake nav mesh
                        if (isBaking)
                        {
                            if (bakingCoroutine != null)
                                StopCoroutine(bakingCoroutine);
                            bakingCoroutine = StartCoroutine(BakeNavMeshAsync());
                        }
                    }
                    else Debug.LogWarning("No object found for noise value: " + noiseValue);
                }
            }
        }
    }

    float GetPerlinNoiseValue(int x, int y, float scale, float offsetX, float offsetY)
    {
        // built-in perlin noise
        float xCoord = (float)x / scale + offsetX;
        float yCoord = (float)y / scale + offsetY;
        return Mathf.PerlinNoise(xCoord, yCoord);
    }

    SpawnObject GetObjectBasedOnNoise(float noiseValue)
    {
        // map noise to a spawn object based on object count
        int index = Mathf.FloorToInt(noiseValue * spawnObjects.Count);
        index = Mathf.Clamp(index, 0, spawnObjects.Count - 1);
        return spawnObjects[index];
    }

    void DestroyObjects(float playerX, float playerZ)
    {
        int startX = Mathf.FloorToInt((playerX - radius) / gridDimensions.x + gap);
        int endX = Mathf.CeilToInt((playerX + radius) / gridDimensions.x + gap);
        int startY = Mathf.FloorToInt((playerZ - radius) / gridDimensions.z + gap);
        int endY = Mathf.CeilToInt((playerZ + radius) / gridDimensions.z + gap);

        List<Vector2Int> keysToRemove = new List<Vector2Int>();

        foreach (var kvp in objects)
        {
            Vector2Int gridPosition = kvp.Key;
            if (gridPosition.x < startX || gridPosition.x > endX || gridPosition.y < startY || gridPosition.y > endY)
            {
                GameObject obj = kvp.Value;
                // don't destroy player
                if (obj != player)
                {
                    obj.SetActive(false);
                    spawnObjectsCache.Enqueue(obj);
                }
                keysToRemove.Add(gridPosition);
            }
        }

        foreach (var key in keysToRemove)
        {
            objects.Remove(key);
        }

        // maintain the cache size
        while (spawnObjectsCache.Count > cacheSize)
        {
            GameObject oldestObj = spawnObjectsCache.Dequeue();
            Destroy(oldestObj);
        }

        // objects destroyed, bake nav mesh
        if (keysToRemove.Count > 0 && isBaking)
        {
            if (bakingCoroutine != null)
                StopCoroutine(bakingCoroutine);
            bakingCoroutine = StartCoroutine(BakeNavMeshAsync());
        }
    }

    // bake nav mesh async
    IEnumerator BakeNavMeshAsync()
    {
        // clear the NavMeshSurface
        navMeshSurface.navMeshData = null;
        // wait for the end of the frame so all objects are updated
        yield return new WaitForEndOfFrame();
        // update to player position
        navMeshSurface.center = new Vector3(player.transform.position.x, 0, player.transform.position.z);
        // build
        navMeshSurface.BuildNavMesh();
        // wait for the end of the frame so the nav mesh is built
        yield return new WaitForEndOfFrame();
    }
}