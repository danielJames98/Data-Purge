using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pauseMenuScript : MonoBehaviour
{

    public gameManagerScript gameManagerScript;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Canvas>().worldCamera = GameObject.Find("Main Camera(Clone)").GetComponent<Camera>();
        gameManagerScript = GameObject.Find("gameManager").GetComponent<gameManagerScript>();
        gameManagerScript.pauseMenu = this.gameObject;
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void newGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("generatedScene");
    }

    public void quit()
    {
        Application.Quit();
    }

    public void mainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("mainMenu");
    }

    public void pauseGame()
    {
        gameManagerScript.pauseGame();
    }
}
