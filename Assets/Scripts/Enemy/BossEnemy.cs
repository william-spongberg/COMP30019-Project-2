using System.Collections;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private Transform dropPoint;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float moveSpeed = 5f; 
    [SerializeField] private float minWaitTimeAtWaypoint = 10f;
    [SerializeField] private float maxWaitTimeAtWaypoint = 20f;
    [SerializeField] private float bombSpreadRadius = 2f;
    [SerializeField] private float minBombInterval = 1f;
    [SerializeField] private float maxBombInterval = 3f;

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

        // Start dropping bombs at intervals
        StartCoroutine(DropBombRoutine());

        float waitTimeAtWaypoint = Random.Range(minWaitTimeAtWaypoint, maxWaitTimeAtWaypoint);
        yield return new WaitForSeconds(waitTimeAtWaypoint);

        // Move to the next waypoint
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        isWaiting = false;
    }

    public void SetWaypoints(Transform[] newWaypoints)
    {
        waypoints = newWaypoints;
    }

    private IEnumerator DropBombRoutine()
    {
        while (isWaiting)
        {
            DropBomb();
            float waitTime = Random.Range(minBombInterval, maxBombInterval);
            yield return new WaitForSeconds(waitTime);
        }
    }

    private void DropBomb()
    {
        // Drop bullet around the drop point location
        if (dropPoint != null && bullet != null)
        {
            Vector3 randomOffset = new Vector3(
                Random.Range(-bombSpreadRadius, bombSpreadRadius),
                0,
                Random.Range(-bombSpreadRadius, bombSpreadRadius)
            );
            Vector3 dropPosition = dropPoint.position + randomOffset;
            Instantiate(bullet, dropPosition, dropPoint.rotation);
        }
    }
}
