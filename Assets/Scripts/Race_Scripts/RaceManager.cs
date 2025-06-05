using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public enum RaceState { Countdown, Racing, Finished }

public class RaceManager : MonoBehaviour
{

    public static RaceManager Instance;

    [Header("Configuración de la carrera")]
    public int totalLaps = 3;                               // Number of laps
    public int totalParticipants = 0;                       // Number of cars/participants
    public List<LapCounter> allLapCounters;                 // List of participants(LapCounter)

    [Header("UI")]
    [SerializeField] private GameObject HUDPanel;           // HUD panel in race
    [SerializeField] private GameObject scoreboardPanel;    // Scoreboard panel
    [SerializeField] private TMP_Text scoreboardText;       // Scoreboard text object

    [Header("Cameras")]
    [SerializeField] private GameObject thirdPersonCamera;  // Third Person Camera for race
    [SerializeField] private GameObject freeLookCamera;     // Free Look Camera for race
    [SerializeField] private GameObject CCTVCamera;         // CCTV Camera for scoreboard
    private GameObject activeRaceCamera;                    // Active camera in race
    private List<ParticipantResult> results = new List<ParticipantResult>(); // List of participants finished
    private int finishedParticipants = 0;                   // Number of participants finished
    private bool raceFinished = false;                      // Is the race finished (true, yes - false, no)

    public AudioSource audioSource;
    public AudioClip scoreboardSound;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        scoreboardPanel.SetActive(false);

        thirdPersonCamera.SetActive(true);
        freeLookCamera.SetActive(false);
        CCTVCamera.SetActive(false);

        activeRaceCamera = thirdPersonCamera;
    }

    public void AddResult(string playerName, float time)
    {
        if (raceFinished) return;

        results.Add(new ParticipantResult(playerName, time));
        finishedParticipants++;

        bool playerFinished = playerName == "Jugador";  // Name asigned to player

        if (playerFinished)
        {
            AddPenalizedResultsForUnfinishedCars(time);
            
            ShowScoreboard();
            return;
        }

        if (finishedParticipants >= totalParticipants)
        {
            ShowScoreboard();
        }
    }

    private void AddPenalizedResultsForUnfinishedCars(float playerFinishTime)
    {
        float penaltyIncrement = 5f;                    // Time for penalty
        float currentPenalty = penaltyIncrement;

        foreach (var lapCounter in allLapCounters)
        {
            // If finished, skip
            if (results.Exists(r => r.playerName == lapCounter.CarName))
                continue;

            // Get current time
            float currentTime = lapCounter.GetCurrentTime();

            // If not finished, use player time + penalty
            float penalizedTime = Mathf.Max(currentTime, playerFinishTime) + currentPenalty;

            results.Add(new ParticipantResult(lapCounter.CarName, penalizedTime));

            finishedParticipants++;
            currentPenalty += penaltyIncrement;
        }
    }

    private void Update()
    {
        if (!raceFinished && allLapCounters.Count > 0)
        {
            UpdatePositions();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleRaceCamera();
        }
    
    }

    public void UpdatePositions()
    {
        allLapCounters.Sort((a, b) =>
        {
            // Order for position update
            if (a.CurrentLap != b.CurrentLap)
                return b.CurrentLap.CompareTo(a.CurrentLap); // First more laps
            return b.Tracker.GetProgressDistance().CompareTo(a.Tracker.GetProgressDistance()); // First more progress
        });

        for (int i = 0; i < allLapCounters.Count; i++)
        {
            allLapCounters[i].SetPosition(i + 1);
        }
    }

    public void ShowScoreboard()
    {
        audioSource.PlayOneShot(scoreboardSound);

        raceFinished = true;
        HUDPanel.SetActive(false);
        scoreboardPanel.SetActive(true);

        thirdPersonCamera.SetActive(false);
        freeLookCamera.SetActive(false);
        CCTVCamera.SetActive(true);

        results.Sort((a, b) => a.time.CompareTo(b.time));

        // Format for results
        string scoreboard = "Position".PadRight(12) + "Name".PadRight(12) + "Time".PadRight(12) + "\n";
        for (int i = 0; i < results.Count; i++)
        {
            scoreboard +=
                (i + 1).ToString().PadRight(12) +
                results[i].playerName.PadRight(12) +
                FormatTime(results[i].time).PadRight(12) + "\n";
        }
        scoreboardText.text = scoreboard;
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        int milliseconds = Mathf.FloorToInt((time * 1000f) % 1000f);
        return string.Format("{0:00}:{1:00}.{2:000}", minutes, seconds, milliseconds);
    }

    public void RestartRace()
    {
        raceFinished = false;
        finishedParticipants = 0;
        results.Clear();
        scoreboardPanel.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ToggleRaceCamera()
    {
        if (activeRaceCamera == thirdPersonCamera)
        {
            thirdPersonCamera.SetActive(false);
            freeLookCamera.SetActive(true);
            activeRaceCamera = freeLookCamera;
        }
        else
        {
            freeLookCamera.SetActive(false);
            thirdPersonCamera.SetActive(true);
            activeRaceCamera = thirdPersonCamera;
        }
    }

    [System.Serializable]
    public struct ParticipantResult
    {
        public string playerName;
        public float time;

        public ParticipantResult(string name, float t)
        {
            playerName = name;
            time = t;
        }
    }
}