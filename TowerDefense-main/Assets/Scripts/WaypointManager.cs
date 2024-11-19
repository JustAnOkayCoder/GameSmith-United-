using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    public Transform[] waypoints;  // Make sure this is public

    void Start()
    {
        // Get all waypoints and store them in order
        waypoints = new Transform[transform.childCount];
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = transform.GetChild(i);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform point = transform.GetChild(i);
            Gizmos.DrawSphere(point.position, 0.5f);
            if (i < transform.childCount - 1)
            {
                Transform nextPoint = transform.GetChild(i + 1);
                Gizmos.DrawLine(point.position, nextPoint.position);
            }
        }
    }
}