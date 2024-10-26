using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : MonoBehaviour
{
    public int spawnEnemies;
    public int slainEnemies;

    // Start is called before the first frame update
    void Start()
    {
        spawnEnemies = 0;
        slainEnemies = 0;
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    public void IncreaseSpawn(int number){
        spawnEnemies += number;
    }

    public void IncreaseSlain(int number){
        slainEnemies += number;
    }

    public void DecreaseSpawn(int number){
        spawnEnemies -= number;
    }

    public void DecreaseSlain(int number){
        slainEnemies -= number;
    }

    public int getSlainEnemies(){
        return slainEnemies;
    }
}
