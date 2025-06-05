using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsAndFinal : MonoBehaviour
{
    [SerializeField] private GameObject HUDPanel;           // HUD panel in race
    [SerializeField] private GameObject scoreboardPanel;    // Scoreboard panel
    [SerializeField] private GameObject finishPanel;        // finish panel
    [SerializeField] private GameObject optionsPanel;       // finish panel
    private bool isOptionsOpen = false;

    public AudioSource audioSource;
    public AudioClip optionsSound;

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleOptions();
        }

    }

    private void ToggleOptions()
    {
        audioSource.PlayOneShot(optionsSound);

        if (isOptionsOpen == false)
        {
            HUDPanel.SetActive(false);
            optionsPanel.SetActive(true);
            isOptionsOpen = true;
        }
        else
        {
            HUDPanel.SetActive(true);
            optionsPanel.SetActive(false);
            isOptionsOpen = false;
        }
    }

    public void CloseScoreboard()
    {
        audioSource.PlayOneShot(optionsSound);
        
        scoreboardPanel.SetActive(false);
        finishPanel.SetActive(true);
    }

    public void Return()
    {
        SceneManager.LoadScene("Main_Menu");
    }
}