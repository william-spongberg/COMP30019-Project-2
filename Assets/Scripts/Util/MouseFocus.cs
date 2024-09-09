using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFocus : MonoBehaviour
{
    void Start()
    {
        // unlock the cursor
        Cursor.lockState = CursorLockMode.None;
        // show cursor
        Cursor.visible = true;
    }
}