using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorSpawner : MonoBehaviour
{
    public int entitySpawned = 0;
    [SerializeField]
    private float radius = 10.0f;
    [SerializeField]
    private Vector3 offset = Vector3.zero;
    [SerializeField]
    private KeyCode key = KeyCode.F;
    [SerializeField]
    private List<GameObject> objects = new();
    
    public int trigger;
    private GameObject player;
    public Counter Tracker;
    private bool spawned;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        spawned = false;
    }

    void Update()
    {
        // spawn new NPC on button press
        if(Tracker.getSlainEnemies() == trigger && !spawned){
            SpawnNPC();
            spawned = true;
        }
    }

    public void SpawnNPC()
    {
        // choose random npc
        int randomIndex = Random.Range(0, objects.Count);
        float playerX = player.transform.position.x;
        float playerZ = player.transform.position.z;

        Vector3 pos = new(playerX + offset.x + Random.Range(-radius, radius), offset.y, playerZ + offset.z + Random.Range(-radius, radius));

        // spawn randomly within x radius of player
        GameObject newObj = Instantiate(objects[randomIndex], pos, Quaternion.identity);
        entitySpawned += 1;
        Tracker.IncreaseSpawn(1);
    }
}