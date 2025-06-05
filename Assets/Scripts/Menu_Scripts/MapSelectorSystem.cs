using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSelectorSystem : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip playSound;
    public AudioClip returnSound;
    public void Desert()
    {
        audioSource.PlayOneShot(playSound);
        SceneManager.LoadScene("Track_1");
    }

    public void Tokyo()
    {
        audioSource.PlayOneShot(playSound);
        SceneManager.LoadScene("Track_2");
    }

    public void Texas()
    {
        audioSource.PlayOneShot(playSound);
        SceneManager.LoadScene("Track_3");
    }

    public void NewYork()
    {
        audioSource.PlayOneShot(playSound);
        SceneManager.LoadScene("Track_4");
    }

    public void Brasil()
    {
        audioSource.PlayOneShot(playSound);
        SceneManager.LoadScene("Track_5");
    }

    public void Colombia()
    {
        audioSource.PlayOneShot(playSound);
        SceneManager.LoadScene("Track_6");
    }
    
    public void Return()
    {
        audioSource.PlayOneShot(returnSound);
        SceneManager.LoadScene("Custom_Car");
    }
}
