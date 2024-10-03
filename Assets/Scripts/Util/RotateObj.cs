using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// show in editor
[ExecuteInEditMode]
public class RotateObj : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);   
    }
}
