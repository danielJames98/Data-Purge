using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class playerController : baseCharacter
{
    public Camera cam;
    
    public GameObject ui;

    // Start is called before the first frame update
    void Start()
    {
        GameObject camObject = Instantiate(Resources.Load<GameObject>("Main Camera"));
        camObject.GetComponent<camController>().player = this.gameObject;
        cam = camObject.GetComponent<Camera>();       

        navMeshAgent = this.GetComponent<NavMeshAgent>();
        ui = Instantiate(Resources.Load<GameObject>("inGameUI"));
        healthBar = GameObject.Find("playerHealthBar");
        healthBarActive = true;
        xpBar = GameObject.Find("playerXpBar");
        castBar = GameObject.Find("playerCastBar");
        castBarActive = true;
        gameManager = GameObject.Find("gameManager").GetComponent<gameManagerScript>();
        gameManager.startingLevel.GetComponent<levelManagerScript>().objectiveText = ui.transform.Find("objectiveText").gameObject;

        frontFirePoint = transform.Find("frontFirePoint").gameObject;

        rb=this.GetComponent<Rigidbody>();

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

        maxXP = level * 100;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if(navMeshAgent.enabled==true && casting ==false &&stunned==false)
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100, 1 << LayerMask.NameToLayer("Walkable")))
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

        if (castingAbility != null && movingToRange==true)
        {
            if (navMeshAgent.destination == new Vector3(transform.position.x, transform.position.y-1, transform.position.z))
            {
                if(castingAbility.targeting=="pointAndClick")
                {
                    castingAbility.StartCoroutine("applyPointandClickEffect");
                }
                else if(castingAbility.targeting=="ground")
                {
                    castingAbility.StartCoroutine("spawnAoe");
                }

                movingToRange = false;
            }
        }

        if (targetCharacter != null && castingAbility != null && movingToRange == true)
        {
            navMeshAgent.destination = (transform.position - targetCharacter.transform.position).normalized * (castingAbility.baseRange * (1+(bonusRange/100))) + targetCharacter.transform.position;
        }

        if(navMeshAgent.speed!=moveSpeed)
        {
            navMeshAgent.speed = moveSpeed;
        }

        if(casting == true)
        {
            castBarUpdate();
        }
    }

   
}