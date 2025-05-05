using UnityEngine;

public class AiInput : MonoBehaviour
{
    public Transform[] waypoints;
    public float waypointThreshold = 1.0f;

    private int currentWaypoint = 0;
    private CarMovement carMovement;

    // --- Variables para detectar atascos ---
    private Vector3 lastPosition;
    private float stuckTimer = 0f;
    private float stuckCheckInterval = 1.5f; // Cada cuánto tiempo revisar si está atascado
    private float stuckDistanceThreshold = 0.5f; // Si se mueve menos de esto, está atascado
    private bool isStuck = false;
    private float unstuckTime = 1.5f; // Cuánto tiempo intenta desatascarse

    private void Start()
    {
        carMovement = GetComponent<CarMovement>();
        carMovement.usePlayerInput = false;
        lastPosition = transform.position;
    }

    private void Update()
    {
        if (waypoints.Length == 0) return;

        // --- Lógica de desatasco ---
        stuckTimer += Time.deltaTime;
        if (stuckTimer >= stuckCheckInterval)
        {
            float distanceMoved = Vector3.Distance(transform.position, lastPosition);
            if (distanceMoved < stuckDistanceThreshold)
            {
                isStuck = true;
                stuckTimer = 0f;
            }
            else
            {
                isStuck = false;
                stuckTimer = 0f;
                lastPosition = transform.position;
            }
        }

        float horizontal, vertical;

        if (isStuck)
        {
            // Intenta retroceder y girar aleatoriamente para salir del atasco
            vertical = -1f; // Reversa
            horizontal = Random.Range(-1f, 1f); // Gira a la izquierda o derecha
            unstuckTime -= Time.deltaTime;
            if (unstuckTime <= 0f)
            {
                isStuck = false;
                unstuckTime = 1.5f;
                lastPosition = transform.position;
            }
        }
        else
        {
            // --- Lógica normal de seguir waypoints ---
            Vector3 target = waypoints[currentWaypoint].position;
            Vector3 direction = (target - transform.position).normalized;
            float angle = Vector3.SignedAngle(transform.forward, direction, Vector3.up);

            horizontal = Mathf.Clamp(angle / 45f, -1f, 1f);
            vertical = 1f;

            if (Vector3.Distance(transform.position, target) < waypointThreshold)
            {
                currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
            }
        }

        carMovement.SetInput(horizontal, vertical);
    }
}