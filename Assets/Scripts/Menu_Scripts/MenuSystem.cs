using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
   public void Play(){
    SceneManager.LoadScene("Test_Race_1");
   }

   public void Exit(){
    Debug.Log("Saliendo del juego...");
    Application.Quit();
   }
}
