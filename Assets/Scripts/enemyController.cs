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
        animator = transform.Find("Robot Kyle").GetComponent<Animator>();
        overHeadCanvas = Instantiate(Resources.Load("overHeadCanvas", typeof(GameObject)), new Vector3(this.transform.position.x, this.transform.position.y + 2, this.transform.position.z), Quaternion.identity) as GameObject;
        overHeadCanvas.GetComponent<overHeadCanvasScript>().yOffset = 2;
        healthBar = overHeadCanvas.transform.Find("healthBar").gameObject;
        overHeadCanvas.GetComponent<overHeadCanvasScript>().parentCharacter = this.gameObject;
        healthBar.SetActive(false);
        healthBarActive = false;
        castBar = overHeadCanvas.transform.Find("castBar").gameObject;
        castBar.SetActive(false);
        castBarActive = false;
        audioSource = GetComponent<AudioSource>();
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

        generateAbility(abilityScript0);
        generateAbility(abilityScript1);
        /*
        generateAbility(abilityScript2);
        generateAbility(abilityScript3);
        generateAbility(abilityScript4);
        */

        levelUp(GameObject.Find("playerCharacter(Clone)").GetComponent<playerController>().level);

        gameManager = GameObject.Find("gameManager").GetComponent<gameManagerScript>();
        levelManager = gameManager.activeLevel.GetComponent<levelManagerScript>();
        levelManager.enemyReady(this.gameObject);
    }

    void Update()
    {
       if(Time.timeScale>0)
        {
            if (charactersInRange.Count > 0)
            {
                aggroTarget = charactersInRange[0];
            }
            else
            {
                aggroTarget = null;
            }

            if (aggroTarget != null)
            {
                if (inCombat == false)
                {
                    inCombat = true;
                }
            }
            else
            {
                inCombat = false;
            }

            if (inCombat && castingAbility == null && casting == false && stunned == false)
            {
                aiSelectAbility();
            }

            if (targetCharacter != null && casting == false && stunned == false && castingAbility != null)
            {

                if (navMeshAgent.destination != targetCharacter.transform.position)
                {
                    navMeshAgent.destination = targetCharacter.transform.position;
                }

                if (targetPosition != targetCharacter.transform.position)
                {
                    targetPosition = targetCharacter.transform.position;
                }
            }

            if (castingAbility != null && casting == false && stunned == false && movingToRange == true)
            {
                if (Vector3.Distance(gameObject.transform.position, targetCharacter.transform.position) <= (castingAbility.baseRange * (1 + (bonusRange / 100))))
                {
                    if (castingAbility.targeting == "pointAndClick")
                    {
                        castingAbility.StartCoroutine("applyPointandClickEffect");
                    }
                    else if (castingAbility.targeting == "direction")
                    {
                        castingAbility.StartCoroutine("spawnProjectile");
                    }
                    else if (castingAbility.targeting == "ground")
                    {
                        castingAbility.StartCoroutine("spawnAoe");
                    }

                    movingToRange = false;
                    navMeshAgent.destination = transform.position;
                }
            }

            if (navMeshAgent.speed != moveSpeed)
            {
                navMeshAgent.speed = moveSpeed;
            }

            if (animator.GetFloat("velocity") != navMeshAgent.velocity.magnitude)
            {
                animator.SetFloat("velocity", navMeshAgent.velocity.magnitude);
            }

            if (navMeshAgent.velocity.magnitude > 0)
            {
                walking = true;
                if (animator.speed != 1 * (navMeshAgent.velocity.magnitude / 10))
                {
                    animator.speed = 1 * (navMeshAgent.velocity.magnitude / 10);
                }
            }
            else if (navMeshAgent.velocity.magnitude == 0)
            {
                walking = false;
                animator.speed = 1;
            }

            if (walking == true && walkSoundPlaying == false)
            {
                StartCoroutine(playWalkSound());
            }

            if (casting == true)
            {
                castBarUpdate();
                if (targetCharacter != null)
                {
                    transform.forward = targetCharacter.transform.position - transform.position;
                }
            }
            else if (casting == false && castBarActive == true)
            {
                castBar.SetActive(false);
                castBarActive = false;
            }
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
        if(casting == false && stunned == false && alive==true)
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

        levelUp(5);

        this.transform.localScale = new Vector3(2f, 2f, 2f);
        this.gameObject.GetComponent<SphereCollider>().radius = 7.5f;
    }
}
