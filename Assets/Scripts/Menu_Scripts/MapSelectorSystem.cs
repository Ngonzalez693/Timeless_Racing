using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSelectorSystem : MonoBehaviour
{
    public void Desert()
    {
        SceneManager.LoadScene("Track_1");
    }

    public void Tokyo()
    {
        SceneManager.LoadScene("Track_2");
    }

    public void Texas()
    {
        SceneManager.LoadScene("Track_3");
    }

    public void NewYork()
    {
        SceneManager.LoadScene("Track_4");
    }

    public void Brasil()
    {
        SceneManager.LoadScene("Track_5");
    }

    public void Colombia()
    {
        SceneManager.LoadScene("Track_6");
    }
    
    public void Return()
    {
        SceneManager.LoadScene("Custom_Car");
    }
}
