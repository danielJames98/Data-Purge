using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class levelManagerScript : MonoBehaviour
{
    public bool objectiveComplete=false;
    public List<GameObject> firewalls;
    public List<enemySpawnerScript> enemySpawners;
    public List<GameObject> enemyList;
    public Material openDoorMat;
    public Material closedDoorMat;
    public string objective;
    public GameObject objectiveText;
    public int enemies;
    public gameManagerScript gameManager;
    public int direction;
    public bool directionSelected;
    public bool rotationDone;
    public Quaternion targetRotation;
    public bool locked;

    private void Start()
    {

            gameManager = GameObject.Find("gameManager").gameObject.GetComponent<gameManagerScript>();
            gameManager.firewalls.Add(firewalls[0]);
            gameManager.firewalls.Add(firewalls[1]);
            gameManager.firewalls.Add(firewalls[2]);
            gameManager.firewalls.Add(firewalls[3]);
        
        
        turnNewLevel();
    }

    private void Update()
    {
        if (direction > 0 && rotationDone == false && directionSelected == true && this.transform.rotation==targetRotation)
        {
            rotationDone=true;            
        }

        if (rotationDone == true && objectiveComplete == false && gameManager.activeLevel == this.gameObject && locked == false) 
        {
            lockDown();
        }
    }

    public void completeObjective()
    {
        if(objectiveComplete==false) 
        {
            objectiveComplete = true;
            objectiveText.GetComponent<TMPro.TextMeshProUGUI>().text = "Objective Complete";

            var overlaps = Physics.OverlapSphere(new Vector3(this.transform.position.x + 52, this.transform.position.y, this.transform.position.z), 1);

            if(overlaps.Length==0)
            {
                GameObject newLevel0 = Instantiate(Resources.Load<GameObject>("levels/level" + Random.Range(0, 7).ToString()), new Vector3(this.transform.position.x + 100, this.transform.position.y, this.transform.position.z), Quaternion.identity);
            }

            overlaps = Physics.OverlapSphere(new Vector3(this.transform.position.x - 52, this.transform.position.y, this.transform.position.z), 1);

            if (overlaps.Length == 0)
            {
                GameObject newLevel1 = Instantiate(Resources.Load<GameObject>("levels/level" + Random.Range(0, 7).ToString()), new Vector3(this.transform.position.x - 100, this.transform.position.y, this.transform.position.z), Quaternion.identity);
            }

            overlaps = Physics.OverlapSphere(new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + 52), 1);

            if (overlaps.Length == 0)
            {
                GameObject newLevel2 = Instantiate(Resources.Load<GameObject>("levels/level" + Random.Range(0, 7).ToString()), new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + 100), Quaternion.identity);
            }

            overlaps = Physics.OverlapSphere(new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - 52), 1);

            if (overlaps.Length == 0)
            {
                GameObject newLevel3 = Instantiate(Resources.Load<GameObject>("levels/level" + Random.Range(0, 7).ToString()), new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - 100), Quaternion.identity);
            }

            gameManager.StartCoroutine("unlock");

            GameObject.Find("Main Camera(Clone)").transform.Find("inGameUI").GetComponent<uiManagerScript>().showLevelCompleteDialogue();
            GameObject.Find("playerCharacter(Clone)").GetComponent<playerController>().gainXP(100);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag=="Player" && objectiveComplete==false&&gameManager!=null&& gameManager.activeLevel!=this.gameObject)
        {
            gameManager.activeLevel = this.gameObject;
        }
    }

    public void killEnemies()
    {
        if(enemyList.Count>0)
        {
            foreach (GameObject enemy in enemyList)
            {
                enemy.GetComponent<enemyController>().StartCoroutine("Die");               
            }
            enemyList.Clear();
        }
    }

    public void lockDown()
    {
        locked = true;
        spawnEnemies();
        gameManager.StartCoroutine("lockdown");
    }

    public void spawnEnemies()
    {
        foreach (enemySpawnerScript spawner in enemySpawners)
        {
            spawner.spawn();
        }
    }

    public void startObjective()
    {       
        objectiveText = GameObject.Find("objectiveText");
        
        int objectiveInt = Random.Range(0,2);

        if(objectiveInt == 0)
        {
            objective = "Annihilation";
            objectiveText.GetComponent<TMPro.TextMeshProUGUI>().text = objective + ":" + " Destroy " + enemies + " enemies";
        }
        else if (objectiveInt == 1)
        {
            objective = "Assassination";
            objectiveText.GetComponent<TMPro.TextMeshProUGUI>().text = objective + ":" + " Destroy " + "the boss";
            int enemyToPromote = Random.Range(0, enemyList.Count);
            enemyList[enemyToPromote].GetComponent<enemyController>().promoteToBoss();
        }
        else if(objectiveInt == 2)
        {

        }
        else if (objectiveInt==3)
        {

        }
    }

    public void updateAnnihilation()
    {
        enemies--;
        objectiveText.GetComponent<TMPro.TextMeshProUGUI>().text = objective + ":" + " Destroy " + enemies + " enemies";
        if ( enemies == 0 )
        {            
            completeObjective();
        }
    }

    public void turnNewLevel()
    {
        direction = Random.Range(0, 4);
        directionSelected = true;

        if(direction==0)
        {
            rotationDone = true;
        }
        else if (direction == 1)
        {
            targetRotation = new Quaternion(0, 90, 0, 0);
            this.transform.Rotate(0, 90, 0, 0);
        }
        else if (direction == 2)
        {
            targetRotation = new Quaternion(0, 180, 0, 0);
            this.transform.Rotate(0, 180, 0, 0);
        }
        else if (direction == 3)
        {
            targetRotation = new Quaternion(0, 270, 0, 0);
            this.transform.Rotate(0, 270, 0, 0);
        }      
    }

    public void enemyReady(GameObject enemy)
    {
        enemies++;
        enemyList.Add(enemy);
        if(enemies==10)
        {
            startObjective();
        }
    }
}
