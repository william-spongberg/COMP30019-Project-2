using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject desk;

    [Header("Prop Settings")]
    [SerializeField]
    private GameObject[] monitors;
    [SerializeField]
    private GameObject monitorPos;
    [SerializeField]
    private GameObject[] chairs;
    [SerializeField]
    private GameObject chairPos;
    [SerializeField]
    private GameObject[] sideObjects;
    [SerializeField]
    private GameObject sideObjectPos;

    void Start()
    {
        // get locations of props
        Vector3 monitorLocation = monitorPos.transform.position;
        Vector3 chairLocation = chairPos.transform.position;
        Vector3 sideObjectLocation = sideObjectPos.transform.position;

        // spawn props
        spawnObject(monitors, monitorLocation);
        spawnObject(chairs, chairLocation);
        spawnObject(sideObjects, sideObjectLocation);
    }

    void spawnObject(GameObject[] objects, Vector3 location)
    {
        // randomly offset object
        location = new Vector3(location.x + Random.Range(-2, 2), location.y, location.z + Random.Range(-2, 2));
        // randomly rotate object
        Quaternion rotation = Quaternion.Euler(0, Random.Range(0, 30), 0);
        // choose random object
        GameObject obj = objects[Random.Range(0, objects.Length)];
        // instantiate object
        Instantiate(obj, location, rotation);
    }
}
