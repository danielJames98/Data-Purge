using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class baseCharacter : MonoBehaviour
{
    public bool alive = true;

    public int level;
    public float currentHealth;
    public float maxHealth;
    public float moveSpeed;
    public float attackSpeed;
    public float cooldownReduction;
    public float armour;
    public float power;
    public float bonusRange;
    public float bonusArea;
    public float bonusProjectileSpeed;
    public float bonusDuration;

    public float currentXP;
    public float maxXP;


    public GameObject ability0;
    public GameObject ability1;
    public GameObject ability2;
    public GameObject ability3;
    public GameObject ability4;

    public baseAbilityScript abilityScript0;
    public baseAbilityScript abilityScript1;
    public baseAbilityScript abilityScript2;
    public baseAbilityScript abilityScript3;
    public baseAbilityScript abilityScript4;

    public GameObject targetCharacter;

    public GameObject healthBar;
    public GameObject castBar;

    public Vector3 targetPosition;

    public baseAbilityScript castingAbility;

    public NavMeshAgent navMeshAgent;

    public bool inCombat;

    public GameObject aggroTarget;

    public GameObject frontFirePoint;

    public Rigidbody rb;

    public bool casting;

    public bool healthBarActive;
    public bool castBarActive;

    public bool stunned;
    public int stuns;

    public string castingCoroutine;
    public float timeSinceCastStart;
    public float castStartTime;
    public bool movingToRange;


    public void takeDamage(float damage)
    {
        if (damage / (1+(armour/100)) > 0)
        {
            currentHealth = currentHealth - (damage / (1 + (armour / 100)));           

            if (healthBarActive ==false) 
            {
                healthBar.SetActive(true);
                healthBarActive = true;
            }

            if(healthBarActive==true)
            {
                healthBar.GetComponent<Slider>().value = currentHealth / maxHealth;

                spawnCombatText(Mathf.Round(damage / (1 + (armour / 100))), "damage");

                if (this.gameObject.tag!="Player")
                {                  
                    StopCoroutine("hideHealthBar");
                    StartCoroutine("hideHealthBar");
                }
            }

        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void takeHealing(float healing)
    {
        if(currentHealth<maxHealth)
        {
            if (currentHealth + healing < maxHealth)
            {
                currentHealth = currentHealth + healing;
                spawnCombatText(Mathf.Round(healing), "heal");                
            }
            else
            {
                spawnCombatText(Mathf.Round(maxHealth-currentHealth), "heal");               
                currentHealth = maxHealth;
            }            

            healthBar.GetComponent<Slider>().value = currentHealth / maxHealth;
        }
    }

    public void Die()
    {
        if(gameObject.tag=="Enemy")
        {
            GameObject.Find("playerCharacter(Clone)").GetComponent<playerController>().gainXP(level);
        }
        alive = false;
        Destroy(gameObject);
    }

    public void activateAbility(baseAbilityScript abilityToActivate)
    {
        abilityToActivate.activateAbility();
    }

    public void spawnCombatText(float quantity, string textType)
    {
        GameObject combatText = Instantiate(Resources.Load("combatTextCanvas", typeof(GameObject)), 
        new Vector3(this.transform.position.x + Random.Range(-2.5f, 2.5f), this.transform.position.y + Random.Range(2.5f, 5f), this.transform.position.z), 
        Quaternion.identity) as GameObject;
        TMPro.TextMeshProUGUI combatTextComponent = combatText.transform.Find("combatText").GetComponent<TMPro.TextMeshProUGUI>();

        if (textType=="damage")
        {
            combatTextComponent.color = Color.red;
            combatTextComponent.text = "-" + quantity.ToString();
        }

        if (textType == "heal")
        {
            combatTextComponent.color = Color.green;
            combatTextComponent.text = "+" + quantity.ToString();
        }
        else if(textType!="damage" && textType!= "heal")
        {
            if(quantity > 0)
            {
                combatTextComponent.color = Color.cyan;
            }
            else if (quantity < 0)
            {
                combatTextComponent.color = Color.magenta;
            }

            combatTextComponent.text = Mathf.Round(quantity).ToString() + " " + textType;
        }
    }

    IEnumerator hideHealthBar()
    {
        yield return new WaitForSeconds(5);
        healthBar.SetActive(false);
        healthBarActive = false;
    }

    public void interruptCast()
    {
        if(castingCoroutine!= null && castingAbility != null)
        {
            castingAbility.StopCoroutine(castingCoroutine);
        }
        castingAbility= null;
        targetCharacter = null;
        castBar.GetComponent<Slider>().value = 0;
        casting = false;
        navMeshAgent.destination = transform.position;
    }



    public void castBarUpdate()
    {
        if(castStartTime==0)
        {
            castStartTime = Time.time;
            timeSinceCastStart = Time.time - castStartTime;
        }       
        
        if(castBarActive==false)
        {
            castBar.SetActive(true);
            castBarActive = true;
        }

        if(castingAbility!=null && timeSinceCastStart < (castingAbility.baseCastTime / (1 + (attackSpeed / 100))))
        {
            castBar.GetComponent<Slider>().value = timeSinceCastStart / (castingAbility.baseCastTime / (1 + (attackSpeed / 100)));
            timeSinceCastStart = Time.time - castStartTime;
        }

        if(castingAbility != null && timeSinceCastStart >= (castingAbility.baseCastTime / (1 + (attackSpeed / 100))))
        {
            castBar.GetComponent<Slider>().value = 0;
            timeSinceCastStart = 0;
            castStartTime = 0;
        }       
    }
}