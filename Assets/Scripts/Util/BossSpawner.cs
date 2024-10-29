using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public int entitySpawned = 0;
    [SerializeField] private float radius = 10.0f;
    [SerializeField] private Vector3 offset = Vector3.zero;
    [SerializeField] private KeyCode key = KeyCode.F;
    [SerializeField] private List<GameObject> objects = new();
    [SerializeField] private Transform[] bossWaypoints; // Waypoints for the boss
    public int index;

    public DialogueSystem systemCheck;

    public int[] waves;
    public int[] triggers;
    private GameObject player;
    public Counter Tracker;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        index = 0;
    }

    void Update()
    {
        // spawn new NPC on button press
        if(index < waves.Length && !systemCheck.getProgress() && triggers[index] == Tracker.getSlainEnemies())
        {
            SpawnWave(waves[index]);
            index += 1;
        }
        if (Input.GetKeyDown(key))
        {
            LevelSpawn(waves);
        }
    }

    public void SpawnObject()
    {
        Debug.Log("Spawning Boss");
        // choose random object
        int randomIndex = Random.Range(0, objects.Count);

        // Spawn boss slightly above and in front of the player
        Vector3 spawnPosition = player.transform.position + player.transform.forward * 5f + Vector3.up * 3f;

        // spawn the object
        GameObject newObj = Instantiate(objects[randomIndex], spawnPosition, Quaternion.identity);

        // Set waypoints for the boss if it has the BossEnemy component
        if(newObj.GetComponent<BossEnemy>() != null)
        {
            BossEnemy boss = newObj.GetComponent<BossEnemy>();
            boss.SetWaypoints(bossWaypoints);
        }

        entitySpawned += 1;
        Tracker.IncreaseSpawn(1);
    }

    public void SpawnWave(int amount)
    {
        for(int i = 0; i < amount; i++)
        {
            SpawnObject();
        }
    }

    public void LevelSpawn(int[] waves)
    {
        for(int j = 0; j < waves.Length; j++)
        {
            SpawnWave(waves[j]);
        }
    }
}
