using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public enum RaceState { Countdown, Racing, Finished }

public class RaceManager : MonoBehaviour
{

    public static RaceManager Instance;

    [Header("Configuración de la carrera")]
    public int totalLaps = 3;
    public int totalParticipants = 0;  // Número total de coches/jugadores

    [Header("UI")]
    [SerializeField] private GameObject scoreboardPanel;
    [SerializeField] private TMP_Text scoreboardText;

    private List<ParticipantResult> results = new List<ParticipantResult>();
    private int finishedParticipants = 0;
    private bool raceFinished = false;

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
    }

    public void AddResult(string playerName, float time)
    {
        if (raceFinished) return;

        results.Add(new ParticipantResult(playerName, time));
        finishedParticipants++;

        // Mostrar scoreboard solo cuando todos hayan terminado
        if (finishedParticipants >= totalParticipants)
        {
            ShowScoreboard();
        }
    }

    public void ShowScoreboard()
    {
        raceFinished = true;
        scoreboardPanel.SetActive(true);

        results.Sort((a, b) => a.time.CompareTo(b.time));

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