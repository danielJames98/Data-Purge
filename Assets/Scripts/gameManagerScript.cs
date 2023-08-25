using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class gameManagerScript : MonoBehaviour
{
    public GameObject startingLevel;
    public GameObject player;
    public GameObject activeLevel;
    public GameObject inGameUI;
    public GameObject cam;
    public GameObject pauseMenu;

   
    public directionalLightScript changeLightScript;
    public Light staticLight;

    public List<GameObject> firewalls;
    

    void Start()
    {
        startingLevel = Instantiate(Resources.Load<GameObject>("levels/level" + Random.Range(0, 7).ToString()));
        activeLevel = startingLevel;
        player = Instantiate(Resources.Load<GameObject>("PlayerCharacter"), new Vector3(-48,1.5f,0), Quaternion.identity);
        player.transform.Rotate(0, 90, 0);
        
    }

    private void Update()
    {
        if(Input.GetButtonDown("Cancel"))
        {
            pauseGame();
        }
    }

    public void lockdown()
    {
        foreach (GameObject firewall in firewalls)
        {
            firewall.GetComponent<NavMeshLink>().enabled = false;
            firewall.GetComponent<MeshRenderer>().material.color = Color.red;
        }
    }

    public IEnumerator unlock()
    {
        yield return new WaitForSeconds (0.1f);
        foreach (GameObject firewall in firewalls)
        {
            firewall.GetComponent<NavMeshLink>().enabled = true;
            firewall.GetComponent<MeshRenderer>().material.color=Color.green;
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
        cam.GetComponent<ShaderEffect_CorruptedVram>().enabled = true;
        StartCoroutine("disableCorruption");
        levelManagerScript activeLevelScript = activeLevel.GetComponent<levelManagerScript>();
        activeLevelScript.killEnemies();
        activeLevelScript.completeObjective();

        GameObject finalLevel= Instantiate(Resources.Load<GameObject>("levels/finalBossLevel"), new Vector3(0,500,0), Quaternion.identity);
        playerController pcCon = player.GetComponent<playerController>();
        pcCon.interruptCast();
        

        player.GetComponent<NavMeshAgent>().Warp(new Vector3(-7, 502, -85));
    }

    IEnumerator disableCorruption()
    {
        yield return new WaitForSeconds(5);
        cam.GetComponent<ShaderEffect_CorruptedVram>().enabled = false;
        inGameUI.GetComponent<uiManagerScript>().showFinalBossDialogue();
    }

    public void skipLevel()
    {
        levelManagerScript activeLevelScript = activeLevel.GetComponent<levelManagerScript>();
        activeLevelScript.killEnemies();
    }
}
