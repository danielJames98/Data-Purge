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

    private void Start()
    {
        if(gameManager==null)
        {
            gameManager = GameObject.Find("gameManager").gameObject.GetComponent<gameManagerScript>();
        }
        
        turnNewLevel();
    }

    private void Update()
    {
        if (direction > 0 && rotationDone == false && directionSelected == true && this.transform.rotation==targetRotation)
        {
            rotationDone=true;
            spawnEnemies();
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
                GameObject newLevel0 = Instantiate(Resources.Load<GameObject>("levels/level" + Random.Range(0, 4).ToString()), new Vector3(this.transform.position.x + 100, this.transform.position.y, this.transform.position.z), Quaternion.identity);
            }

            overlaps = Physics.OverlapSphere(new Vector3(this.transform.position.x - 52, this.transform.position.y, this.transform.position.z), 1);

            if (overlaps.Length == 0)
            {
                GameObject newLevel1 = Instantiate(Resources.Load<GameObject>("levels/level" + Random.Range(0, 4).ToString()), new Vector3(this.transform.position.x - 100, this.transform.position.y, this.transform.position.z), Quaternion.identity);
            }

            overlaps = Physics.OverlapSphere(new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + 52), 1);

            if (overlaps.Length == 0)
            {
                GameObject newLevel2 = Instantiate(Resources.Load<GameObject>("levels/level" + Random.Range(0, 4).ToString()), new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + 100), Quaternion.identity);
            }

            overlaps = Physics.OverlapSphere(new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - 52), 1);

            if (overlaps.Length == 0)
            {
                GameObject newLevel3 = Instantiate(Resources.Load<GameObject>("levels/level" + Random.Range(0, 4).ToString()), new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - 100), Quaternion.identity);
            }

            foreach (GameObject firewall in firewalls)
            {
                Destroy(firewall);
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag=="Player" && objectiveComplete==false)
        {
            lockDown();
        }
    }

    public void killEnemies()
    {
        foreach(GameObject enemy in enemyList)
        {
            enemy.GetComponent<enemyController>().Die();
        }
    }

    public void lockDown()
    {
        if (gameManager == null)
        {
            gameManager = GameObject.Find("gameManager").gameObject.GetComponent<gameManagerScript>();
        }

        gameManager.activeLevel = this.gameObject;

        this.GetComponent<BoxCollider>().enabled = false;

        foreach(GameObject firewall in firewalls)
        {
            firewall.GetComponent<NavMeshLink>().enabled = false;
            firewall.GetComponent<Renderer>().material = closedDoorMat;
        }        
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
            spawnEnemies();
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
