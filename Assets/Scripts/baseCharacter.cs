using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class baseCharacter : MonoBehaviour
{
    public bool alive = true;

    public gameManagerScript gameManager;
    public levelManagerScript levelManager;

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

    public bool boss;

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
    public GameObject overHeadCanvas;
    public GameObject healthBar;
    public GameObject castBar;
    public GameObject xpBar;

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

            if(levelManager.objective=="Annihilation")
            {
                levelManager.updateAnnihilation();
            }
            else if(levelManager.objective == "Assassination" && boss==true)
            {
                levelManager.completeObjective();
            }
            
            Destroy(overHeadCanvas);
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

    public void gainXP(float xpGain)
    {
        while (xpGain > 0)
        {
            currentXP = currentXP + 1;
            xpGain = xpGain - 1;

            if(tag=="Player")
            {
                xpBar.GetComponent<Slider>().value = currentXP / maxXP;
            }
            
            if (currentXP == maxXP)
            {
                levelUp();
            }
        }
    }

    public void levelUp()
    {
        currentXP = 0;
        level++;
        maxXP = level * 100;

        int statToBuff = Random.Range(0, 10);

        if(statToBuff==0)
        {
            maxHealth++;
        }
        else if (statToBuff == 1)
        {
            moveSpeed = moveSpeed + 1;
        }
        else if (statToBuff == 2)
        {
            attackSpeed = attackSpeed + 1;
        }
        else if(statToBuff == 3)
        {
            cooldownReduction = cooldownReduction + 1;
        }
        else if(statToBuff == 4)
        {
            armour = armour + 1;
        }
        else if(statToBuff == 5)
        {
            power = power + 1;
        }
        else if(statToBuff == 6)
        {
            bonusRange = bonusRange + 1;
        }
        else if(statToBuff == 7)
        {
            bonusArea = bonusArea + 1;
        }
        else if(statToBuff == 8)
        {
            bonusProjectileSpeed = bonusProjectileSpeed + 1;
        }
        else if(statToBuff == 9)
        {
            bonusDuration = bonusDuration + 1;
        }

        currentHealth = maxHealth;

        if (tag == "Player")
        {
            xpBar.GetComponent<Slider>().value = currentXP / maxXP;
        }

        healthBar.GetComponent<Slider>().value = currentHealth / maxHealth;
    }
}