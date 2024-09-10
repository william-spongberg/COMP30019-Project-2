using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerControl : MonoBehaviour
{
    [SerializeField]
    private float timeDelay;

    void Start()
    {
        StartCoroutine(FlickeringLight());
    }

    // if disabled by cache, restart flickering again
    void OnEnable() {
        StartCoroutine(FlickeringLight());
    }

    IEnumerator FlickeringLight()
    {
        while(true) {
            GetComponent<Light>().enabled = false;
            timeDelay = Random.Range(0.01f, 0.5f);
            yield return new WaitForSeconds(timeDelay);
            GetComponent<Light>().enabled = true;
            timeDelay = Random.Range(0.01f, 0.5f);
            yield return new WaitForSeconds(timeDelay);
        }
    }
}
