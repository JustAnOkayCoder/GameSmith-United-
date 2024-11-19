using UnityEngine;

public class EnemyUnit : MonoBehaviour
{
    [Header("Enemy Properties")]
    public float moveSpeed = 5f;
    public int damageToPlayer = 10;
    public int maxHealth = 100;
    public int currentHealth;

    private Transform[] waypoints;
    private int currentWaypointIndex;
    private bool isMoving = true;

    void Start()
    {
        currentHealth = maxHealth;
        WaypointManager waypointManager = FindObjectOfType<WaypointManager>();
        if (waypointManager != null)
        {
            waypoints = waypointManager.waypoints;
            if (waypoints.Length > 0)
            {
                transform.position = waypoints[0].position;
            }
        }
        else
        {
            Debug.LogError("No WaypointManager found in the scene!");
        }
    }

    void Update()
    {
        if (isMoving && waypoints != null && currentWaypointIndex < waypoints.Length)
        {
            MoveToNextWaypoint();
        }
    }

    void MoveToNextWaypoint()
    {
        Transform targetWaypoint = waypoints[currentWaypointIndex];

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetWaypoint.position,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            currentWaypointIndex++;

            if (currentWaypointIndex >= waypoints.Length)
            {
                ReachEndpoint();
            }
        }
    }

    void ReachEndpoint()
    {
        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damageToPlayer);
        }

        Destroy(gameObject);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}