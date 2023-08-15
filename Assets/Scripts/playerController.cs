using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class playerController : baseCharacter
{
    public Camera cam;
    
    public GameObject ui;
    public uiManagerScript uiScript;
    public List<baseAbilityScript> inventoryItems;
    public int firstEmptyInventorySlot;
    public GameObject inventory;
    public bool inventoryOpen;
    public bool inventoryFull;

    // Start is called before the first frame update
    void Start()
    {
        GameObject camObject = Instantiate(Resources.Load<GameObject>("Main Camera"));
        camObject.GetComponent<camController>().player = this.gameObject;
        cam = camObject.GetComponent<Camera>();
        ui = camObject.transform.Find("inGameUI").gameObject;
        uiScript = ui.GetComponent<uiManagerScript>();
        GameObject musicPlayer = Instantiate(Resources.Load<GameObject>("musicPlayer"));
        musicPlayer.transform.parent = gameObject.transform;
        animator = transform.Find("Robot Kyle").GetComponent<Animator>();
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        healthBar = GameObject.Find("playerHealthBar");
        healthBarActive = true;
        xpBar = GameObject.Find("playerXpBar");
        castBar = GameObject.Find("playerCastBar");
        castBarActive = true;
        gameManager = GameObject.Find("gameManager").GetComponent<gameManagerScript>();
        gameManager.startingLevel.GetComponent<levelManagerScript>().objectiveText = ui.transform.Find("objectiveText").gameObject;
        audioSource = GetComponent<AudioSource>();
        frontFirePoint = transform.Find("frontFirePoint").gameObject;
        firstEmptyInventorySlot = 0;
        rb=this.GetComponent<Rigidbody>();
        inventory = transform.Find("inventory").gameObject;

        inventoryItems[0] = inventory.transform.Find("inventorySlot0").GetComponent<baseAbilityScript>();
        inventoryItems[1] = inventory.transform.Find("inventorySlot1").GetComponent<baseAbilityScript>();
        inventoryItems[2] = inventory.transform.Find("inventorySlot2").GetComponent<baseAbilityScript>();
        inventoryItems[3] = inventory.transform.Find("inventorySlot3").GetComponent<baseAbilityScript>();
        inventoryItems[4] = inventory.transform.Find("inventorySlot4").GetComponent<baseAbilityScript>();
        inventoryItems[5] = inventory.transform.Find("inventorySlot5").GetComponent<baseAbilityScript>();
        inventoryItems[6] = inventory.transform.Find("inventorySlot6").GetComponent<baseAbilityScript>();
        inventoryItems[7] = inventory.transform.Find("inventorySlot7").GetComponent<baseAbilityScript>();
        inventoryItems[8] = inventory.transform.Find("inventorySlot8").GetComponent<baseAbilityScript>();
        inventoryItems[9] = inventory.transform.Find("inventorySlot9").GetComponent<baseAbilityScript>();
        inventoryItems[10] = inventory.transform.Find("inventorySlot10").GetComponent<baseAbilityScript>();
        inventoryItems[11] = inventory.transform.Find("inventorySlot11").GetComponent<baseAbilityScript>();
        inventoryItems[12] = inventory.transform.Find("inventorySlot12").GetComponent<baseAbilityScript>();
        inventoryItems[13] = inventory.transform.Find("inventorySlot13").GetComponent<baseAbilityScript>();
        inventoryItems[14] = inventory.transform.Find("inventorySlot14").GetComponent<baseAbilityScript>();
        inventoryItems[15] = inventory.transform.Find("inventorySlot15").GetComponent<baseAbilityScript>();
        inventoryItems[16] = inventory.transform.Find("inventorySlot16").GetComponent<baseAbilityScript>();
        inventoryItems[17] = inventory.transform.Find("inventorySlot17").GetComponent<baseAbilityScript>();
        inventoryItems[18] = inventory.transform.Find("inventorySlot18").GetComponent<baseAbilityScript>();
        inventoryItems[19] = inventory.transform.Find("inventorySlot19").GetComponent<baseAbilityScript>();


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
        generateAbility(abilityScript2);
        generateAbility(abilityScript3);
        generateAbility(abilityScript4);

        levelUp(1);
    }

    void Update()
    {
        if (Time.timeScale > 0 && alive == true)
        {
            if (Input.GetMouseButton(0))
            {
                if (navMeshAgent.enabled == true && casting == false && stunned == false)
                {

                    Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    Ray lootRay = cam.ScreenPointToRay(Input.mousePosition);
                    RaycastHit lootHit;

                    if (Physics.Raycast(lootRay, out lootHit, 100) && lootHit.transform.gameObject.tag == "loot" && Vector3.Distance(this.transform.position, lootHit.point) < 10)
                    {
                        getFirstEmptySlot();
                        if (inventoryFull == false)
                        {
                            lootHit.transform.gameObject.GetComponent<lootScript>().pickUp(this, inventoryItems[firstEmptyInventorySlot]);
                        }
                    }

                    if (Physics.Raycast(ray, out hit, 100, 1 << LayerMask.NameToLayer("Walkable")) && inventoryOpen == false)
                    {
                        targetCharacter = null;
                        castingAbility = null;
                        navMeshAgent.destination = hit.point;
                    }
                }
            }

            if (Input.GetMouseButtonDown(1) && casting == false && stunned == false)
            {
                activateAbility(abilityScript0);
            }

            if (Input.GetButtonDown("1") && casting == false && stunned == false)
            {
                activateAbility(abilityScript1);
            }

            if (Input.GetButtonDown("2") && casting == false && stunned == false)
            {
                activateAbility(abilityScript2);
            }

            if (Input.GetButtonDown("3") && casting == false && stunned == false)
            {
                activateAbility(abilityScript3);
            }

            if (Input.GetButtonDown("4") && casting == false && stunned == false)
            {
                activateAbility(abilityScript4);
            }

            if (castingAbility != null && movingToRange == true)
            {
                if (Vector3.Distance(gameObject.transform.position, targetPosition) <= (castingAbility.baseRange * (1 + (bonusRange / 100))))
                {
                    if (castingAbility.targeting == "pointAndClick")
                    {
                        castingAbility.StartCoroutine("applyPointandClickEffect");
                    }
                    else if (castingAbility.targeting == "ground")
                    {
                        castingAbility.StartCoroutine("spawnAoe");
                    }

                    navMeshAgent.destination = transform.position;
                    movingToRange = false;
                }
            }

            if (targetCharacter != null && castingAbility != null && movingToRange == true)
            {
                // navMeshAgent.destination = targetCharacter.transform.position;
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
        }
    }

    void getFirstEmptySlot()
    {
        int i = 0;
        bool findingSlot=true;

        while (i<=19 && findingSlot==true)
        {
            if (inventoryItems[i].type == null || inventoryItems[i].type == "")
            {
                firstEmptyInventorySlot = i;
                findingSlot = false;
            }
            else
            {
                if(i<19)
                {
                    i++;
                }
                else
                {
                    inventoryFull = true;
                    findingSlot=false;
                }              
            }
        }
    }
}