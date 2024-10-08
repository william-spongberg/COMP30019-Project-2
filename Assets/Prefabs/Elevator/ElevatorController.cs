using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    public Animator doorAnimator;
    public bool isOpen = false;

    void Update()
    {
        // Press 'O' to open the doors
        if (Input.GetKeyDown(KeyCode.O) && !isOpen)
        {
            doorAnimator.SetBool("IsOpen", true);  // Set the parameter to true to open doors
            isOpen = true;
        }

        // Press 'C' to close the doors
        if (Input.GetKeyDown(KeyCode.C) && isOpen)
        {
            doorAnimator.SetBool("IsOpen", false);  // Set the parameter to false to close doors
            isOpen = false;
        }
    }
}
