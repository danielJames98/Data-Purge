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
    public GameObject xpBar;

    // Start is called before the first frame update
    void Start()
    {
        GameObject camObject = Instantiate(Resources.Load<GameObject>("Main Camera"));
        camObject.GetComponent<camController>().player = this.gameObject;
        cam = camObject.GetComponent<Camera>();       

        navMeshAgent = this.GetComponent<NavMeshAgent>();
        GameObject ui = Instantiate(Resources.Load<GameObject>("inGameUI"));
        healthBar = GameObject.Find("playerHealthBar");
        healthBarActive = true;
        xpBar = GameObject.Find("playerXpBar");
        castBar = GameObject.Find("playerCastBar");
        castBarActive = true;

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

    public void gainXP(float xpGain)
    {
        while (xpGain>0)
        {
            currentXP= currentXP+1;
            xpGain = xpGain - 1; 
            xpBar.GetComponent<Slider>().value = currentXP / maxXP;
            Debug.Log(currentXP / maxXP);
            if (currentXP==maxXP)
            {
                currentXP = 0;
                level++;
                maxXP = level * 100;
                maxHealth++;
                currentHealth = maxHealth;
                moveSpeed= moveSpeed+1;
                attackSpeed= attackSpeed+1;
                cooldownReduction= cooldownReduction+1;
                armour = armour + 1;
                power = power + 1;
                bonusRange = bonusRange + 1;
                bonusArea = bonusArea + 1;
                bonusProjectileSpeed = bonusProjectileSpeed + 1;
                bonusDuration = bonusDuration + 1;
                xpBar.GetComponent<Slider>().value = currentXP / maxXP;
                healthBar.GetComponent<Slider>().value = currentHealth / maxHealth;
            }           
        }
    }
}