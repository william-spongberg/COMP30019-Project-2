using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTree : MonoBehaviour
{
    void Start()
    {
        transform.Rotate(0, Random.Range(0, 360), 0);
    }
}
