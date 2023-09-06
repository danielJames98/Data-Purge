using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class gameManagerScript : MonoBehaviour
{
    public GameObject startingLevel;
    public GameObject player;
    public GameObject activeLevel;
    public GameObject inGameUI;
    public uiManagerScript uiScript;
    public GameObject cam;
    public GameObject pauseMenu;
    public GameObject endGamePortal;
    public bool gameComplete;
    public GameObject backToGamePortal;

    public directionalLightScript changeLightScript;
    public Light staticLight;

    public List<GameObject> firewalls;
    

    void Start()
    {
        startingLevel = Instantiate(Resources.Load<GameObject>("levels/level" + Random.Range(0, 10).ToString()));
        activeLevel = startingLevel;
        player = Instantiate(Resources.Load<GameObject>("PlayerCharacter"), new Vector3(-48,1.5f,0), Quaternion.identity);
        player.transform.Rotate(0, 90, 0);
        
    }

    private void Update()
    {
        if(Input.GetButtonDown("Cancel"))
        {
            if(uiScript.inventoryUIActive==false && uiScript.statPageActive == false)
            {
                pauseGame();
            }
            else
            {
                if(uiScript.statPageActive == true)
                {
                    uiScript.cPress();
                }

                if(uiScript.inventoryUIActive==true)
                {
                    uiScript.iPress();
                }
            }
        }
    }

    public void lockdown()
    {
        foreach (GameObject firewall in firewalls)
        {
            firewall.GetComponent<NavMeshLink>().enabled = false;
            firewall.GetComponent<MeshRenderer>().material.color = Color.red;
            firewall.GetComponent<BoxCollider>().enabled = true;
        }
    }

    public IEnumerator unlock()
    {
        yield return new WaitForSeconds (0.1f);
        foreach (GameObject firewall in firewalls)
        {
            firewall.GetComponent<NavMeshLink>().enabled = true;
            firewall.GetComponent<MeshRenderer>().material.color=Color.green;
            firewall.GetComponent<BoxCollider>().enabled = false;
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

    public void launchFinalLevel()
    {
        if(gameComplete==false)
        {
            GameObject finalLevel = Instantiate(Resources.Load<GameObject>("levels/finalBossLevel"), new Vector3(0, 500, 0), Quaternion.identity);
            StartCoroutine("corruption");
            StartCoroutine("disableCorruption");
        }       
        levelManagerScript activeLevelScript = activeLevel.GetComponent<levelManagerScript>();
        activeLevelScript.killEnemies();
        activeLevelScript.completeObjective();
        backToGamePortal.GetComponent<backToGamePortalScript>().warpLocation = player.transform.position;       
        playerController pcCon = player.GetComponent<playerController>();
        pcCon.interruptCast();
        player.GetComponent<NavMeshAgent>().Warp(new Vector3(-7, 502, -80));
    }

    IEnumerator corruption()
    {
        cam.GetComponent<ShaderEffect_CorruptedVram>().enabled = true;
        cam.GetComponent<ShaderEffect_CorruptedVram>().shift=Random.Range(-500, 500);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine("corruption");       
    }

    public void finalBossDefeated()
    {
        gameComplete = true;
        inGameUI.GetComponent<uiManagerScript>().showFinalBossDefeatedDialogue();
        endGamePortal.SetActive(true);
        backToGamePortal.SetActive(true);
    }

    IEnumerator disableCorruption()
    {
        yield return new WaitForSeconds(5);
        StopCoroutine("corruption");
        cam.GetComponent<ShaderEffect_CorruptedVram>().enabled = false;
        inGameUI.GetComponent<uiManagerScript>().showFinalBossDialogue();
    }

    public void skipLevel()
    {
        levelManagerScript activeLevelScript = activeLevel.GetComponent<levelManagerScript>();
        activeLevelScript.killEnemies();
    }
}
