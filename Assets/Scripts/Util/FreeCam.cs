using UnityEngine;

public class FreeCam : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 10.0f;
    [SerializeField]
    private float lookSpeed = 2.0f;
    [SerializeField]
    // lock camera rotation to small area for smoother movement
    private float lookXLimit = 45.0f;
    [SerializeField]
    private float smoothTime = 0.3f;

    private float rotationX = 0.0f;
    private Vector3 currentVelocity;
    private Vector3 targetPosition;
    private Quaternion targetRotation;

    void Start()
    {
        targetPosition = transform.position;
        targetRotation = transform.rotation;
    }

    void Update()
    {
        // rotate camera
        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        float rotationY = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * lookSpeed;
        targetRotation = Quaternion.Euler(rotationX, rotationY, 0);

        // move camera
        float moveForward = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;
        float moveRight = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime;
        float moveUp = 0;

        // move up
        if (Input.GetKey(KeyCode.Q))
        {
            moveUp = movementSpeed * Time.deltaTime;
        }
        // move down
        else if (Input.GetKey(KeyCode.E))
        {
            moveUp = -movementSpeed * Time.deltaTime;
        }
        targetPosition += transform.right * moveRight + transform.up * moveUp + transform.forward * moveForward;

        // smooth everything out
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothTime);
    }
}