using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    public Animator doorAnimator;
    public bool doorsOpen = false;

    void Update()
    {
        // Press 'O' to open the doors
        if (Input.GetKeyDown(KeyCode.O) && !doorsOpen)
        {
            doorAnimator.SetBool("IsOpen", true);  // Set the parameter to true to open doors
            doorsOpen = true;
        }

        // Press 'C' to close the doors
        if (Input.GetKeyDown(KeyCode.C) && doorsOpen)
        {
            doorAnimator.SetBool("IsOpen", false);  // Set the parameter to false to close doors
            doorsOpen = false;
        }
    }
}
