using UnityEngine;
using TMPro;
using System.Collections;

public class CountdownController : MonoBehaviour
{
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private float countdownTime = 3f;
    [SerializeField] private MonoBehaviour[] controlScriptsToDisable;

    public delegate void RaceStartAction();
    public static event RaceStartAction OnRaceStart;

    private void Start()
    {
        StartCoroutine(StartCountdown());
    }

    private IEnumerator StartCountdown()
    {
        // Desactivar controles o IA
        foreach (var script in controlScriptsToDisable)
        {
            script.enabled = false;
        }

        float timer = countdownTime;
        while (timer > 0)
        {
            countdownText.text = Mathf.Ceil(timer).ToString();
            timer -= Time.deltaTime;
            yield return null;
        }
        countdownText.text = "GO!";
        OnRaceStart?.Invoke();

        // Activar controles o IA
        foreach (var script in controlScriptsToDisable)
        {
            script.enabled = true;
        }

        yield return new WaitForSeconds(1f);
        countdownText.gameObject.SetActive(false);
    }
}
