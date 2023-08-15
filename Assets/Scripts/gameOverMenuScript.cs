using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameOverMenuScript : MonoBehaviour
{
    public void newGame()
    {
        SceneManager.LoadScene("generatedScene");
    }

    public void quit()
    {
        Application.Quit();
    }

    public void mainMenu()
    {
        SceneManager.LoadScene("mainMenu");
    }
}
