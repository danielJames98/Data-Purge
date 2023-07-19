using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class enemyController : baseCharacter
{
    public List<GameObject> charactersInRange;

    
    void Start()
    {
        navMeshAgent = this.GetComponent<NavMeshAgent>();

        frontFirePoint = transform.Find("frontFirePoint").gameObject;

        overHeadCanvas = Instantiate(Resources.Load("overHeadCanvas", typeof(GameObject)), new Vector3(this.transform.position.x, this.transform.position.y + 2, this.transform.position.z), Quaternion.identity) as GameObject;
        healthBar = overHeadCanvas.transform.Find("healthBar").gameObject;
        overHeadCanvas.GetComponent<overHeadCanvasScript>().parentCharacter = this.gameObject;
        healthBar.SetActive(false);
        healthBarActive = false;
        castBar = overHeadCanvas.transform.Find("castBar").gameObject;
        castBar.SetActive(false);
        castBarActive = false;

        rb = this.GetComponent<Rigidbody>();

        ability0 = transform.Find("ability0").gameObject;
        ability1 = transform.Find("ability1").gameObject;
        ability2 = transform.Find("ability2").gameObject;
        ability3 = transform.Find("ability3").gameObject;
        ability4 = transform.Find("ability4").gameObject;

        abilityScript0 = ability0.GetComponent<baseAbilityScript>();
        abilityScript1 = ability1.GetComponent<baseAbilityScript>();
        abilityScript2 = ability2.GetComponent<baseAbilityScript>();
        abilityScript3 = ability3.GetComponent<baseAbilityScript>();
        abilityScript4 = ability4.GetComponent<baseAbilityScript>();

        gameManager = GameObject.Find("gameManager").GetComponent<gameManagerScript>();
        levelManager = gameManager.activeLevel.GetComponent<levelManagerScript>();
        levelManager.enemyReady(this.gameObject);
    }

    void Update()
    {
        if(charactersInRange.Count > 0)
        {
            aggroTarget = charactersInRange[0];
        }
        else
        {
            aggroTarget= null;
        }

        if(aggroTarget!= null)
        {
            if(inCombat==false)
            {
                inCombat = true;
            }
        }
        else
        {
            inCombat = false;
        }

        if(inCombat && castingAbility==null && casting == false && stunned == false)
        {
            aiSelectAbility();
        }

        if (targetCharacter != null && casting == false && stunned == false && castingAbility!=null)
        {       

            if (navMeshAgent.destination != (transform.position - targetCharacter.transform.position).normalized * (castingAbility.baseRange * bonusRange) + targetCharacter.transform.position)
            {
                navMeshAgent.destination = (transform.position - targetCharacter.transform.position).normalized * (castingAbility.baseRange * bonusRange) + targetCharacter.transform.position;               
            }   
            
            if(targetPosition!=targetCharacter.transform.position)
            {
                targetPosition= targetCharacter.transform.position;
            }
        }

        if (castingAbility != null && casting == false && stunned == false && movingToRange == true)
        {
            if (navMeshAgent.destination == new Vector3(transform.position.x, transform.position.y-1, transform.position.z))
            {
                if(castingAbility.targeting=="pointAndClick")
                {
                    castingAbility.StartCoroutine("applyPointandClickEffect");
                }
                else if (castingAbility.targeting=="direction")
                {
                    castingAbility.StartCoroutine("applyPointandClickEffect");
                }
                else if (castingAbility.targeting == "ground")
                {
                    castingAbility.StartCoroutine("spawnAoe");
                }

                movingToRange = false;
            }
        }

        if (navMeshAgent.speed != moveSpeed)
        {
            navMeshAgent.speed = moveSpeed;
        }

        if (casting==true)
        {
            castBarUpdate();
        }
        else if (casting == false && castBarActive == true)
        {
            castBar.SetActive(false);
            castBarActive = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Player")
        {
            charactersInRange.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag=="Player"&& charactersInRange.Contains(other.gameObject))
        {
            charactersInRange.Remove(other.gameObject);
        }
    }

    public void aiSelectAbility()
    {
        if(casting == false && stunned == false)
        {
            int abilityNumber = Random.Range(0, 4);

            if (abilityNumber == 0 && abilityScript0.onCooldown == false)
            {
                activateAbility(abilityScript0);
            }
            else if (abilityNumber == 1 && abilityScript1.onCooldown == false)
            {
                activateAbility(abilityScript1);
            }
            else if (abilityNumber == 2 && abilityScript2.onCooldown == false)
            {
                activateAbility(abilityScript2);
            }
            else if (abilityNumber == 3 && abilityScript3.onCooldown == false)
            {
                activateAbility(abilityScript3);
            }
            else if (abilityNumber == 4 && abilityScript4.onCooldown == false)
            {
                activateAbility(abilityScript4);
            }
        }
    }

    public void promoteToBoss()
    {
        boss = true;

        int i = 0;
        while(i<10)
        {
            levelUp();
            i++;
        }

        this.transform.localScale = new Vector3(2f, 2f, 2f);
    }
}
