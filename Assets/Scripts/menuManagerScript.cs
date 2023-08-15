using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class menuManagerScript : MonoBehaviour
{
    public void newGame()
    {
        SceneManager.LoadScene("generatedScene");
    }

    public void quit()
    {
        Application.Quit();
    }
}
