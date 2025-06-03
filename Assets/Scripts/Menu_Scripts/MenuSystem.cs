using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
   public void Play(){
    SceneManager.LoadScene("Custom_Car");
   }

   public void Exit(){
    Debug.Log("Saliendo del juego...");
    Application.Quit();
   }
}
