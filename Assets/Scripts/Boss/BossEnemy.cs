using System.Collections;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints; // Array of waypoints for the boss to move to
    [SerializeField] private float moveSpeed = 5f;  // Speed at which the boss flies
    [SerializeField] private float waitTimeAtWaypoint = 5f; // Time to wait at each waypoint

    private int currentWaypointIndex = 0;
    private bool isWaiting = false;

    void Start()
    {
        if (waypoints.Length > 0)
        {
            transform.position = waypoints[currentWaypointIndex].position;
        }
    }

    void Update()
    {
        if (!isWaiting && waypoints.Length > 0)
        {
            MoveToNextWaypoint();
        }
    }

    private void MoveToNextWaypoint()
    {
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, moveSpeed * Time.deltaTime);

        // If the boss reaches the waypoint
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            StartCoroutine(WaitAtWaypoint());
        }
    }

    private IEnumerator WaitAtWaypoint()
    {
        isWaiting = true;

        // Placeholder for dropping bomb
        Debug.Log("Dropping bomb at waypoint");

        yield return new WaitForSeconds(waitTimeAtWaypoint);

        // Move to the next waypoint
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        isWaiting = false;
    }

    public void SetWaypoints(Transform[] newWaypoints)
    {
        waypoints = newWaypoints;
    }
}
