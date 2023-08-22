using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManagerScript : MonoBehaviour
{
    public GameObject startingLevel;
    public GameObject player;
    public GameObject activeLevel;
    public GameObject inGameUI;
    public GameObject cam;
    public GameObject pauseMenu;
    

    void Start()
    {
        startingLevel = Instantiate(Resources.Load<GameObject>("levels/level" + Random.Range(0, 7).ToString()));
        player = Instantiate(Resources.Load<GameObject>("PlayerCharacter"), new Vector3(-48,1.5f,0), Quaternion.identity);
        player.transform.Rotate(0, 90, 0);
        startingLevel.GetComponent<levelManagerScript>().lockDown();
    }

    private void Update()
    {
        if(Input.GetButtonDown("Cancel"))
        {
            pauseGame();
        }
    }

    public void pauseGame()
    {
        if (Time.timeScale > 0)
        {
            Time.timeScale = 0;
            inGameUI.SetActive(false);
            pauseMenu.SetActive(true);
            cam.GetComponent<BWEffect>().intensity = 1;
        }
        else
        {
            Time.timeScale = 1;
            inGameUI.SetActive(true);
            pauseMenu.SetActive(false);
            cam.GetComponent<BWEffect>().intensity = 0;
        }
    }
}
