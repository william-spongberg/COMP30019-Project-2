using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerControl : MonoBehaviour {
    [SerializeField]
    private Light lightObject;

    private float timeDelay;
    private bool isFlickering = false;

    void Update()
    {
        if (isFlickering == false)
        {
            StartCoroutine(FlickeringLight());
        }
    }

    IEnumerator FlickeringLight()
    {
        isFlickering = true;
        if (lightObject != null)
        {
            lightObject.enabled = false;
            timeDelay = Random.Range(0.01f, 0.5f);
            yield return new WaitForSeconds(timeDelay);
            lightObject.enabled = true;
            timeDelay = Random.Range(0.01f, 0.5f);
            yield return new WaitForSeconds(timeDelay);
        }
        isFlickering = false;
    }
}