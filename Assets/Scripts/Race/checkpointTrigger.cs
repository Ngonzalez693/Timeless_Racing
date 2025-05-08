using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    public int waypointIndex;

    [System.Obsolete]
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            RaceManager rm = FindObjectOfType<RaceManager>();
            if (rm != null)
            {
                rm.RacerPassedWaypoint(other.gameObject, waypointIndex);
            }
        }
    }
}
