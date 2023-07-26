using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class baseCharacter : MonoBehaviour
{
    public bool alive = true;

    public Animator animator;

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

    public AudioSource audioSource;
    public List<AudioClip> walkSounds;
    public bool walkSoundPlaying;
    public int currentWalkSound;
    public float walkSoundDelay;
    public bool walking;

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
            GameObject.Find("playerCharacter(Clone)").GetComponent<playerController>().gainXP(10);

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
        if(gameObject.tag=="Player")
        {
            GameObject.Find("Main Camera(Clone)").GetComponent<camController>().player = null;
            SceneManager.LoadScene("generatedScene");
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

    public void spawnLevelUpText(float quantity, string textType)
    {
        GameObject combatText = Instantiate(Resources.Load("combatTextCanvas", typeof(GameObject)),
        new Vector3(this.transform.position.x + Random.Range(-2.5f, 2.5f), this.transform.position.y + Random.Range(2.5f, 5f), this.transform.position.z),
        Quaternion.identity) as GameObject;
        TMPro.TextMeshProUGUI combatTextComponent = combatText.transform.Find("combatText").GetComponent<TMPro.TextMeshProUGUI>();


        combatTextComponent.color = Color.green;
        combatTextComponent.text = "Level " + quantity.ToString() + "!" + " +1% " + textType;

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
                levelUp(1);
            }
        }
    }

    public void levelUp(int levelsToAdd)
    {
        currentXP = 0;
        level++;
        maxXP = level * 100;

        int statToBuff = Random.Range(0, 10);

        if(statToBuff==0)
        {
            maxHealth++;
            spawnLevelUpText(level, "Health");
        }
        else if (statToBuff == 1)
        {
            moveSpeed = moveSpeed + 1;
            spawnLevelUpText(level, "Move Speed");
        }
        else if (statToBuff == 2)
        {
            attackSpeed = attackSpeed + 1;
            spawnLevelUpText(level, "Attack Speed");
        }
        else if(statToBuff == 3)
        {
            cooldownReduction = cooldownReduction + 1;
            spawnLevelUpText(level, "CDR");
        }
        else if(statToBuff == 4)
        {
            armour = armour + 1;
            spawnLevelUpText(level, "Armour");
        }
        else if(statToBuff == 5)
        {
            power = power + 1;
            spawnLevelUpText(level, "Power");
        }
        else if(statToBuff == 6)
        {
            bonusRange = bonusRange + 1;
            spawnLevelUpText(level, "Range");
        }
        else if(statToBuff == 7)
        {
            bonusArea = bonusArea + 1;
            spawnLevelUpText(level, "AoE");
        }
        else if(statToBuff == 8)
        {
            bonusProjectileSpeed = bonusProjectileSpeed + 1;
            spawnLevelUpText(level, "Projectile Speed");
        }
        else if(statToBuff == 9)
        {
            bonusDuration = bonusDuration + 1;
            spawnLevelUpText(level, "Duration");
        }

        currentHealth = maxHealth;

        if (tag == "Player")
        {
            xpBar.GetComponent<Slider>().value = currentXP / maxXP;
        }

        healthBar.GetComponent<Slider>().value = currentHealth / maxHealth;

        int levelsLeftToAdd = levelsToAdd - 1;

        if (levelsLeftToAdd > 0)
        {
            levelUp(levelsLeftToAdd);
        }
    }

    public void generateAbility(baseAbilityScript ability)
    {
        //select if offensive or supportive
        int offenseInt = Random.Range(0, 20);
        if(offenseInt<19)
        {
            ability.offensive = true;
        }
        else if(offenseInt==19)
        {
            ability.offensive = false;
        }

        //select a targeting method and type
        int targetingInt = Random.Range(1, 3);
        if(targetingInt==0)
        {
            ability.targeting = "pointAndClick";
            ability.type = "pointAndClick";
        }
        else if(targetingInt==1)
        {
            ability.targeting = "direction";
            ability.type = "projectile";

            //sets if the projectile is piercing
            int piercingInt = Random.Range(0, 2);
            if(piercingInt==0)
            {
                ability.piercing = false;
            }
            else if (piercingInt==1)
            {
                ability.piercing = true;
            }

            //set projectile size and speed
            ability.projectileSpeed=Random.Range(10, 100);
            ability.projectileSize = Mathf.Round(Random.Range(0.1f, 2f)*10)*0.1f;
        }
        else if(targetingInt == 2)
        {
            ability.targeting = "ground";
            ability.type = "aoe";

        }
        else if(targetingInt==3)
        {
            //offensive self targets are aoe, supportive can be aoe or point and click
            ability.targeting = "self";
            if(ability.offensive==true)
            {
                ability.type = "aoe";
            }
            else if(ability.offensive==false)
            {
                int selfTypeInt = Random.Range(0, 2);
                {
                    if (selfTypeInt == 0)
                    {
                        ability.type = "pointAndClick";
                    }
                    else if(selfTypeInt==1)
                    {
                        ability.type = "aoe";
                    }
                }
            }
        }

        //sets the aoe size and duration
        if(ability.type=="aoe")
        {
            ability.aoeDuration = Random.Range(1, 5);
            ability.baseAoeRadius = Random.Range(5, 10);
        }

        //adds damage if offensive or healing if supportive
        if(ability.offensive==true)
        {
            ability.baseDamage = Random.Range(0, 25);
        }
        else if (ability.offensive==false)
        {
            ability.baseHealing = Random.Range(0, 25);
        }

        //adds a range value if not self-targeted
        if(ability.targeting!="self")
        {
            ability.baseRange = Random.Range(3, 10);
            if(ability.targeting=="pointAndClick")
            {
                ability.baseRange = ability.baseRange / 2;
            }
        }

        //assign cast time and cooldown
        ability.baseCastTime = Mathf.Round(Random.Range(0f, 2f)*10)*0.1f;
        ability.baseCooldown = Random.Range(0, 10);

        //decide how many effects this applies
        int effectsToAdd = Random.Range(0, 3);

        if(effectsToAdd > 0)
        {
            ability.appliesEffect = true;

            //determines effect duration
            ability.effectDuration = Random.Range(1, 10);
        }    

        //adds effects if any
        while (effectsToAdd>0)
        {
            int effectInt=Random.Range(0, 20);

            if(effectInt == 0) 
            {
                if(ability.offensive== true && ability.dotDamage==0)
                {
                    ability.dotDamage = Random.Range(10, 25);
                    effectsToAdd--;
                }
                else if(ability.offensive==false && ability.hotHealing==0)
                {
                    ability.hotHealing= Random.Range(10, 25);
                    effectsToAdd--;
                }
            }
            else if(effectInt == 1 && ability.percentArmourMod==0)
            {
                
                if(ability.offensive == true) { ability.percentArmourMod = Random.Range(-10, -25); }
                else if(ability.offensive == false) { ability.percentArmourMod = Random.Range(10, 25); }
                effectsToAdd--;
            }
            else if (effectInt == 2 && ability.percentPowerMod == 0)
            {

                if (ability.offensive == true) { ability.percentPowerMod = Random.Range(-10, -25); }
                else if (ability.offensive == false) { ability.percentPowerMod = Random.Range(10, 25); }
                effectsToAdd--;
            }
            else if (effectInt == 3 && ability.percentAttackSpeedMod == 0)
            {

                if (ability.offensive == true) { ability.percentAttackSpeedMod = Random.Range(-10, -25); }
                else if (ability.offensive == false) { ability.percentAttackSpeedMod = Random.Range(10, 25); }
                effectsToAdd--;
            }
            else if (effectInt == 4 && ability.percentMoveSpeedMod == 0)
            {

                if (ability.offensive == true) { ability.percentMoveSpeedMod = Random.Range(-10, -25); }
                else if (ability.offensive == false) { ability.percentMoveSpeedMod = Random.Range(10, 25); }
                effectsToAdd--;
            }
            else if (effectInt == 5 && ability.percentCdrMod == 0)
            {

                if (ability.offensive == true) { ability.percentCdrMod = Random.Range(-10, -25); }
                else if (ability.offensive == false) { ability.percentCdrMod = Random.Range(10, 25); }
                effectsToAdd--;
            }
            else if (effectInt == 6 && ability.percentRangeMod == 0)
            {

                if (ability.offensive == true) { ability.percentRangeMod = Random.Range(-10, -25); }
                else if (ability.offensive == false) { ability.percentRangeMod = Random.Range(10, 25); }
                effectsToAdd--;
            }
            else if (effectInt == 7 && ability.percentAoeMod == 0)
            {

                if (ability.offensive == true) { ability.percentAoeMod = Random.Range(-10, -25); }
                else if (ability.offensive == false) { ability.percentAoeMod = Random.Range(10, 25); }
                effectsToAdd--;
            }
            else if (effectInt == 8 && ability.percentProjSpeedMod == 0)
            {

                if (ability.offensive == true) { ability.percentProjSpeedMod = Random.Range(-10, -25); }
                else if (ability.offensive == false) { ability.percentProjSpeedMod = Random.Range(10, 25); }
                effectsToAdd--;
            }
            else if (effectInt == 9 && ability.percentDurationMod == 0)
            {

                if (ability.offensive == true) { ability.percentDurationMod = Random.Range(-10, -25); }
                else if (ability.offensive == false) { ability.percentDurationMod = Random.Range(10, 25); }
                effectsToAdd--;
            }
            else if (effectInt == 10 && ability.flatArmourMod == 0)
            {

                if (ability.offensive == true) { ability.flatArmourMod = Random.Range(-1, -25); }
                else if (ability.offensive == false) { ability.flatArmourMod = Random.Range(1, 25); }
                effectsToAdd--;
            }
            else if (effectInt == 11 && ability.flatPowerMod == 0)
            {

                if (ability.offensive == true) { ability.flatPowerMod = Random.Range(-1, -25); }
                else if (ability.offensive == false) { ability.flatPowerMod = Random.Range(1, 25); }
                effectsToAdd--;
            }
            else if (effectInt == 12 && ability.flatAttackSpeedMod == 0)
            {

                if (ability.offensive == true) { ability.flatAttackSpeedMod = Random.Range(-1, -25); }
                else if (ability.offensive == false) { ability.flatAttackSpeedMod = Random.Range(1, 25); }
                effectsToAdd--;
            }
            else if (effectInt == 13 && ability.flatMoveSpeedMod == 0)
            {

                if (ability.offensive == true) { ability.flatMoveSpeedMod = Random.Range(-1, -25); }
                else if (ability.offensive == false) { ability.flatMoveSpeedMod = Random.Range(1f, 25); }
                effectsToAdd--;
            }
            else if (effectInt == 14 && ability.flatCdrMod == 0)
            {

                if (ability.offensive == true) { ability.flatCdrMod = Random.Range(-1, -25); }
                else if (ability.offensive == false) { ability.flatCdrMod = Random.Range(1, 25); }
                effectsToAdd--;
            }
            else if (effectInt == 15 && ability.flatRangeMod == 0)
            {

                if (ability.offensive == true) { ability.flatRangeMod = Random.Range(-1, -25); }
                else if (ability.offensive == false) { ability.flatRangeMod = Random.Range(1, 25); }
                effectsToAdd--;
            }
            else if (effectInt == 16 && ability.flatAoeMod == 0)
            {

                if (ability.offensive == true) { ability.flatAoeMod = Random.Range(-1, -25); }
                else if (ability.offensive == false) { ability.flatAoeMod = Random.Range(1, 25); }
                effectsToAdd--;
            }
            else if (effectInt == 17 && ability.flatProjSpeedMod == 0)
            {

                if (ability.offensive == true) { ability.flatProjSpeedMod = Random.Range(-1, -25); }
                else if (ability.offensive == false) { ability.flatProjSpeedMod = Random.Range(1, 25); }
                effectsToAdd--;
            }
            else if (effectInt == 18 && ability.flatDurationMod == 0)
            {

                if (ability.offensive == true) { ability.flatDurationMod = Random.Range(-1, -25); }
                else if (ability.offensive == false) { ability.flatDurationMod = Random.Range(1, 25); }
                effectsToAdd--;
            }
            else if(effectInt==19)
            {
                if (ability.offensive == true && ability.stun == false)
                {
                    ability.stun = true;
                    ability.effectDuration = ability.effectDuration / 4;
                    effectsToAdd--;
                }
            }

            int stackInt = Random.Range(0, 2);
            if(stackInt==0)
            {
                ability.stackingEffect = false;
            }
            else if(stackInt == 1)
            {
                ability.stackingEffect = true;
                ability.effectDuration = ability.effectDuration / 2;
            }
        }
    }
}