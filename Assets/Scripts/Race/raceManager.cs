using UnityEngine;
using System.Collections.Generic;

public class RaceManager : MonoBehaviour
{
    [Header("Configuración de la Carrera")]
    public int totalLaps = 3; // Número de vueltas (editable en Inspector)
    public List<GameObject> racers = new List<GameObject>(); // Todos los coches (jugador + IA)
    public int totalWaypoints = 31; // Cambia este valor si tienes más/menos waypoints
    public bool raceFinished = false;

    public float raceTime = 0f;
    public bool timerRunning = false;

    // Info de cada corredor
    [System.Serializable]
    public class RacerInfo
    {
        public GameObject racerObject;
        public int currentLap = 1; // Empieza en 1
        public int currentWaypoint = 0;
        public int nextCheckpointIndex = 1;
        public bool isPlayer = false;
        public int position = 0;

        public float GetRaceProgress(int totalWaypoints, Transform[] waypoints)
        {
            float baseProgress = (currentLap - 1) * totalWaypoints + currentWaypoint;
            float distanceToNext = 0f;
            if (waypoints != null && waypoints.Length > 0)
            {
                int nextIndex = (currentWaypoint + 1) % totalWaypoints;
                distanceToNext = Vector3.Distance(racerObject.transform.position, waypoints[nextIndex].position);
            }
            // Restar la distancia para que el que esté más cerca del siguiente checkpoint tenga mayor progreso
            return baseProgress - (distanceToNext / 1000f);
        }
    }

    public List<RacerInfo> racerInfos = new List<RacerInfo>();

    void Start()
    {
        InitializeRace();
        raceTime = 0f;
        timerRunning = true;
    }

    void Update()
    {
        if (timerRunning && !raceFinished)
            raceTime += Time.deltaTime;

        CalculatePositions();
    }

    void InitializeRace()
    {
        racerInfos.Clear();
        foreach (GameObject racer in racers)
        {
            RacerInfo info = new RacerInfo();
            info.racerObject = racer;
            info.isPlayer = racer.GetComponent<PlayerInput>() != null;
            info.currentLap = 1;
            info.currentWaypoint = 0;
            racerInfos.Add(info);
        }
    }

    public void RacerPassedWaypoint(GameObject racer, int checkpointIndex)
    {
        RacerInfo info = racerInfos.Find(r => r.racerObject == racer);
        if (info != null)
        {
            if (checkpointIndex == info.nextCheckpointIndex)
            {
                info.currentWaypoint = checkpointIndex;

                // Si pasa por el checkpoint 0, suma vuelta
                if (info.currentWaypoint == 0 && checkpointIndex == 0)
                    info.currentLap++;

                info.nextCheckpointIndex = (checkpointIndex + 1) % totalWaypoints;

                // Si es el jugador y terminó todas las vueltas
                if (info.isPlayer && info.currentLap > totalLaps && !raceFinished)
                {
                    raceFinished = true;
                    OnPlayerFinishRace();
                }
            }
        }
    }

    private void OnPlayerFinishRace()
    {
        Debug.Log("¡Carrera terminada!");
        timerRunning = false; // Detiene el tiempo

        var playerInfo = GetPlayerInfo();
        if (playerInfo != null)
        {
            var input = playerInfo.racerObject.GetComponent<PlayerInput>();
            if (input != null)
                input.enabled = false;
        }
    }

    public RacerInfo GetPlayerInfo()
    {
        return racerInfos.Find(r => r.isPlayer);
    }

    void CalculatePositions()
    {
        Transform[] waypoints = racers[0].GetComponent<AiInput>()?.waypoints;
        racerInfos.Sort((a, b) => 
            b.GetRaceProgress(totalWaypoints, waypoints).CompareTo(a.GetRaceProgress(totalWaypoints, waypoints))
        );
        for (int i = 0; i < racerInfos.Count; i++)
            racerInfos[i].position = i + 1;
    }
}