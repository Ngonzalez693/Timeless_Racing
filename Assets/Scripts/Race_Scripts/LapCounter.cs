using UnityEngine;
using UnityStandardAssets.Utility;

public class LapCounter : MonoBehaviour
{
    [SerializeField] private WaypointCircuit circuit;
    [SerializeField] private WaypointProgressTracker tracker;
    [SerializeField] private string carName;
    [SerializeField] private MonoBehaviour[] controlScriptsToDisable;

    private int currentLap = 0;
    private bool raceStarted = false;
    private TimerController timerController;

    private void Awake()
    {
        timerController = GetComponent<TimerController>();
    }

    private void OnEnable()
    {
        CountdownController.OnRaceStart += StartRace;
    }

    private void OnDisable()
    {
        CountdownController.OnRaceStart -= StartRace;
    }

    private void StartRace()
    {
        raceStarted = true;
        Debug.Log("Comienza la Carrera");
        timerController.StartTimer();
    }

    private void FixedUpdate()
    {
        if (!raceStarted) return;

        float progress = tracker.GetProgressDistance();

        if (progress >= circuit.Length * (currentLap + 1))
        {
            currentLap++;

            if (currentLap >= RaceManager.Instance.totalLaps)
            {
                Debug.Log($"¡{carName} finalizó la carrera!");

                timerController.StopTimer();

                // Reportar resultado al RaceManager
                float playerTime = timerController.GetElapsedTime();
                RaceManager.Instance.AddResult(carName, playerTime);

                // Desactivar controles o IA
                foreach (var script in controlScriptsToDisable)
                {
                    script.enabled = false;
                }

                raceStarted = false; // Para evitar contar más vueltas
            }
            else
            {
                Debug.Log($"¡{carName} completó la vuelta {currentLap}!");
            }
        }
    }
}
