using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class CustomSystem : MonoBehaviour
{
    public Slider speedSlider;                          // Slider to change top speed
    public TMP_Text speedText;                          // Text to show speed slider value
    public TMP_Text accelerationModeText;               // Text in button to change acceleration mode
    public Slider driftSlider;                          // Slider to change brake force
    public TMP_Text driftText;                          // Text to show drift slider value

    public bool fastAcceleration = false;               // Is in fast acceleration mode on (true, yes - false, no)

    public AudioSource audioSource;
    public AudioClip returnSound;
    public AudioClip playSound;

    private void Start()
    {
        UpdateSpeedText(speedSlider.value);
        UpdateDriftText(driftSlider.value);

        speedSlider.onValueChanged.AddListener(UpdateSpeedText);
        driftSlider.onValueChanged.AddListener(UpdateDriftText);
    }

    private void UpdateSpeedText(float value)
    {
        speedText.text = value.ToString("F0") + " km/h";
    }

    private void UpdateDriftText(float value)
    {
        driftText.text = value.ToString("F0");
    }
    public void Continue()
    {
        audioSource.PlayOneShot(playSound);

        // Save values in Static class
        CarSettings.topSpeed = speedSlider.value;
        CarSettings.acceleration = fastAcceleration ? 2f : 1f;
        CarSettings.brakeForce = driftSlider.value / 30000f;

        SceneManager.LoadScene("Select_Map");
    }

    public void ToggleAccelerationMode()
    {
        audioSource.PlayOneShot(playSound);
        fastAcceleration = !fastAcceleration;
        accelerationModeText.text = fastAcceleration ? "Aceleracion: Rapida" : "Aceleracion: Normal";
    }

    public void Return()
    {
        audioSource.PlayOneShot(returnSound);
        SceneManager.LoadScene("Main_Menu");
    }
}
