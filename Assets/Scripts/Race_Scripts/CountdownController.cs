using UnityEngine;
using TMPro;
using System.Collections;

public class CountdownController : MonoBehaviour
{
    [Header("Game HUD")]
    [SerializeField] private TMP_Text countdownText;                    // Text object for the countdown
    [SerializeField] private float countdownTime = 3f;                  // Countdown time
    [SerializeField] private MonoBehaviour[] controlScriptsToDisable;   // Scripts to disable (CarUserControl - CarAiControl)

    [Header("Race HUD")]
    [SerializeField] private GameObject HUDPanel;                       // HUD panel in race

    public delegate void RaceStartAction();                             // Race start event
    public static event RaceStartAction OnRaceStart;                    // Variable to race start event

    private void Start()
    {
        StartCoroutine(StartCountdown());
    }

    private IEnumerator StartCountdown()
    {
        // Unable controls or AI
        foreach (var script in controlScriptsToDisable)
        {
            script.enabled = false;
        }

        float timer = countdownTime;
        while (timer > 0)
        {
            countdownText.text = Mathf.Ceil(timer).ToString();
            //Debug.Log(countdownText.text);
            timer -= Time.deltaTime;
            yield return null;
        }
        countdownText.text = "GO!";
        OnRaceStart?.Invoke();

        // Enable controls or AI
        foreach (var script in controlScriptsToDisable)
        {
            script.enabled = true;
        }

        yield return new WaitForSeconds(1f);
        HUDPanel.SetActive(true);
        countdownText.gameObject.SetActive(false);
    }
}
