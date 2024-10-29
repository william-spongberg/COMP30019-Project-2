using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public int entitySpawned = 0;
    [SerializeField]
    private float radius = 10.0f;
    [SerializeField]
    private Vector3 offset = Vector3.zero;
    [SerializeField]
    private List<GameObject> objects = new();
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
        if(index < waves.Length && !systemCheck.getProgress() && triggers[index] == Tracker.getSlainEnemies()){
            SpawnWave(waves[index]);
            index += 1;
        }
    }

    public void SpawnObject()
    {
        // choose random object
        int randomIndex = Random.Range(0, objects.Count);
        float playerX = player.transform.position.x;
        float playerZ = player.transform.position.z;

        Vector3 pos = new(playerX + offset.x + Random.Range(-radius, radius), offset.y, playerZ + offset.z + Random.Range(-radius, radius));

        // spawn randomly within x radius of player
        GameObject newObj = Instantiate(objects[randomIndex], pos, Quaternion.identity);
        //newObj.EnemyHP.setTracker(Tracker);
        
        if(newObj.GetComponent<EnemyHP>() != null){
            EnemyHP enemy = newObj.GetComponent<EnemyHP>();
            enemy.setTracker(Tracker);
        }
        entitySpawned += 1;
        Tracker.IncreaseSpawn(1);
    }

    public void SpawnWave(int amount){
        for(int i = 0; i < amount; i++){
            SpawnObject();
        }
    }

    public void LevelSpawn(int[] waves){
        for(int j = 0; j < waves.Length; j++){
            SpawnWave(waves[j]);
        }
    }
}