using UnityEngine;

public abstract class WaypointPath : MonoBehaviour
{
    public Transform GetWaypoint(int waypointIndex)
    {
        return transform.GetChild(waypointIndex);
    }

    public abstract int GetNextWaypointIndex(int currentWaypointIndex);
}
