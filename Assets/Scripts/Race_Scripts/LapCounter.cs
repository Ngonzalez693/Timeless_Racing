using UnityEngine;
using UnityStandardAssets.Utility;
using TMPro;

public class LapCounter : MonoBehaviour
{
    [Header("Configuraciones")]
    [SerializeField] private WaypointCircuit circuit;                   // Script for waypoint tracking
    [SerializeField] private WaypointProgressTracker tracker;           // Object for WaypointCircuit
    public WaypointProgressTracker Tracker => tracker;                  // Public variable in reference to tracker
    [SerializeField] private string carName;                            // Name of the car (para verificación y scoreboard)
    public string CarName => carName;                                   // Public variable in reference to carName
    [SerializeField] private MonoBehaviour[] controlScriptsToDisable;   // Scripts to disable (CarUserControl - CarAiControl)

    [Header("UI")]
    [SerializeField] private TMP_Text lapText;                          // Text object for laps
    [SerializeField] private TMP_Text positionText;                     // Text object for position
    [SerializeField] private TMP_Text timerText;                        // Text object for timer

    private int currentLap = 0;                                         // Private variable for current lap
    public int CurrentLap => currentLap;                                // Public variable in reference to currentLap
    private bool raceStarted = false;                                   // Has the race started (true, yes - false, no)
    private TimerController timerController;                            // Call for TimerController

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
        UpdateLapUI();
        timerController.StartTimer();
    }

    private void FixedUpdate()
    {
        if (!raceStarted) return;

        float progress = tracker.GetProgressDistance();

        if (progress >= circuit.Length * (currentLap + 1))
        {
            currentLap++;
            UpdateLapUI();

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

        if (raceStarted)
        {
            timerText.text = timerController.GetFormattedTime();
        }
    }

    private void UpdateLapUI()
    {
        lapText.text = $"{currentLap + 1}/{RaceManager.Instance.totalLaps}";
    }

    public void SetPosition(int pos)
    {
        string suffix = pos == 1 ? "st" : pos == 2 ? "nd" : pos == 3 ? "rd" : "th";
        positionText.text = $"{pos}{suffix}";
    }

    public float GetCurrentTime()
    {
        if (timerController != null)
            return timerController.GetElapsedTime();
        return 0f;
    }

}
