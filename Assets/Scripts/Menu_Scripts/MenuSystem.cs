using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{

   public AudioSource audioSource;
   public AudioClip playSound;
   public AudioClip quitSound;

   public void Play()
   {
      audioSource.PlayOneShot(playSound);
      SceneManager.LoadScene("Custom_Car");
   }

   public void Exit()
   {
      audioSource.PlayOneShot(quitSound);
      Debug.Log("Saliendo del juego...");
      Application.Quit();
   }
}