using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public void PlayGame()
    {   //Para la PEC2 cargaremos Gym
        SceneManager.LoadScene("Gym");
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Salir del juego");
    }
}