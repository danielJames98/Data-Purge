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
    public GameObject ui;
    public uiManagerScript uiScript;
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
    public float dashRange;

    public float currentXP;
    public float maxXP;

    public bool boss;

    public GameObject ability0;
    public GameObject ability1;
    public GameObject ability2;
    public GameObject ability3;
    public GameObject ability4;
    public GameObject warpAbility;

    public baseAbilityScript abilityScript0;
    public baseAbilityScript abilityScript1;
    public baseAbilityScript abilityScript2;
    public baseAbilityScript abilityScript3;
    public baseAbilityScript abilityScript4;
    public baseAbilityScript warpAbilityScript;

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
    public GameObject aimLaserScaler;

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
    public AudioClip levelUpSound;
    public bool walkSoundPlaying;
    public int currentWalkSound;
    public float walkSoundDelay;
    public bool walking;
    public GameObject weapon;

    public List<AudioClip> deathSounds;

    public List<aoeScript> aoesColliding;

    public List<projectileScript> projectilesHoming;

    public List<projectileScript> ownProjectiles;

    public void takeDamage(float damage)
    {
        if(alive==true)
        {
            if (damage / (1 + (armour / 100)) > 0)
            {
                currentHealth = currentHealth - (damage / (1 + (armour / 100)));

                if (healthBarActive == false)
                {
                    healthBar.SetActive(true);
                    healthBarActive = true;
                }

                if (healthBarActive == true)
                {
                    healthBar.GetComponent<Slider>().value = currentHealth / maxHealth;

                    spawnCombatText(Mathf.Round(damage / (1 + (armour / 100))), "damage");

                    if (this.gameObject.tag != "Player")
                    {
                        StopCoroutine("hideHealthBar");
                        StartCoroutine("hideHealthBar");
                    }
                }

            }

            if (currentHealth <= 0&& alive==true)
            {
                StartCoroutine("Die");
            }
        }



    }

    public void takeHealing(float healing)
    {
        if(alive==true)
        {
            if (currentHealth < maxHealth)
            {
                if (currentHealth + healing < maxHealth)
                {
                    currentHealth = currentHealth + healing;
                    spawnCombatText(Mathf.Round(healing), "heal");
                }
                else
                {
                    spawnCombatText(Mathf.Round(maxHealth - currentHealth), "heal");
                    currentHealth = maxHealth;
                }

                healthBar.GetComponent<Slider>().value = currentHealth / maxHealth;
            }
        }
    }

    public IEnumerator Die()
    {
        interruptCast();
        navMeshAgent.destination = transform.position;
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        AudioSource.PlayClipAtPoint(deathSounds[Random.Range(0, deathSounds.Count)], gameObject.transform.position);
        alive = false;

        if (gameObject.tag == "Enemy")
        {
            GameObject.Find("playerCharacter(Clone)").GetComponent<playerController>().gainXP(20);

            int lootInt = Random.Range(0, 2);
            if(lootInt==0)
            {
                GameObject loot0 = Instantiate(Resources.Load("abilityCore", typeof(GameObject)), this.transform.position, Quaternion.identity) as GameObject;
                generateAbilityCore(loot0.GetComponent<lootScript>());
            }
            else if(lootInt ==1)
            {
                GameObject loot0 = Instantiate(Resources.Load("healthCore", typeof(GameObject)), this.transform.position, Quaternion.identity) as GameObject;
            }


            if (levelManager.objective == "Annihilation")
            {
                levelManager.updateAnnihilation();
            }
            else if (levelManager.objective == "Assassination" && boss == true)
            {
                levelManager.completeObjective();
            }

            if(gameObject.name=="overlord")
            {
                gameManager.finalBossDefeated();
                GameObject loot1 = Instantiate(Resources.Load("abilityCore", typeof(GameObject)), this.transform.position, Quaternion.identity) as GameObject;
                generateAbilityCore(loot1.GetComponent<lootScript>());
                GameObject loot2 = Instantiate(Resources.Load("abilityCore", typeof(GameObject)), this.transform.position, Quaternion.identity) as GameObject;
                generateAbilityCore(loot2.GetComponent<lootScript>());
                GameObject loot3 = Instantiate(Resources.Load("abilityCore", typeof(GameObject)), this.transform.position, Quaternion.identity) as GameObject;
                generateAbilityCore(loot3.GetComponent<lootScript>());
                GameObject loot4 = Instantiate(Resources.Load("abilityCore", typeof(GameObject)), this.transform.position, Quaternion.identity) as GameObject;
                generateAbilityCore(loot4.GetComponent<lootScript>());
                GameObject loot5 = Instantiate(Resources.Load("abilityCore", typeof(GameObject)), this.transform.position, Quaternion.identity) as GameObject;
                generateAbilityCore(loot5.GetComponent<lootScript>());
                GameObject loot6 = Instantiate(Resources.Load("abilityCore", typeof(GameObject)), this.transform.position, Quaternion.identity) as GameObject;
                generateAbilityCore(loot6.GetComponent<lootScript>());
                GameObject loot7 = Instantiate(Resources.Load("abilityCore", typeof(GameObject)), this.transform.position, Quaternion.identity) as GameObject;
                generateAbilityCore(loot7.GetComponent<lootScript>());
                GameObject loot8 = Instantiate(Resources.Load("abilityCore", typeof(GameObject)), this.transform.position, Quaternion.identity) as GameObject;
                generateAbilityCore(loot8.GetComponent<lootScript>());
                GameObject loot9 = Instantiate(Resources.Load("abilityCore", typeof(GameObject)), this.transform.position, Quaternion.identity) as GameObject;
                generateAbilityCore(loot9.GetComponent<lootScript>());
            }
            
        }
        if (gameObject.tag == "Player")
        {
            GameObject.Find("Main Camera(Clone)").GetComponent<camController>().player = null;
            GameObject.Find("Main Camera(Clone)").GetComponent<ShaderEffect_CorruptedVram>().enabled = true;
            GameObject.Find("Main Camera(Clone)").GetComponent<ShaderEffect_CorruptedVram>().shift = 5;
            GameObject.Find("musicPlayer(Clone)").GetComponent<musicPlayerScript>().playDeathSound();
            //gameManager.GetComponent<gameManagerScript>().activeLevel.GetComponent<levelManagerScript>().killEnemies();
            gameManager.GetComponent<gameManagerScript>().inGameUI.SetActive(false);
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene("gameOverScene");
        }

        foreach (aoeScript aoeScript in aoesColliding)
        {
            if (aoeScript.charsInAoe.Contains(this))
            {
                aoeScript.removeTarget(this);
            }
        }

        foreach (projectileScript projectileScript in projectilesHoming)
        {
            if (projectileScript.homingTarget==this)
            {
                projectileScript.homingTarget=null;
            }
        }

        foreach (projectileScript ownProjectile in ownProjectiles)
        {
            if (ownProjectile.charAppliedBy == this)
            {
                ownProjectile.charAppliedBy = null;
            }
        }

        if (gameObject.tag=="Enemy")
        {
            yield return new WaitForSeconds(0.1f);
            Destroy(overHeadCanvas);
            Destroy(this.gameObject);
        }      
    }


    public void activateAbility(baseAbilityScript abilityToActivate)
    {
        if(alive==true)
        {
            abilityToActivate.activateAbility();
        }       
    }

    public void spawnCombatText(float quantity, string textType)
    {
        GameObject combatText = Instantiate(Resources.Load("combatTextCanvas", typeof(GameObject)),
        new Vector3(this.transform.position.x + Random.Range(-2.5f, 2.5f), this.transform.position.y + Random.Range(2.5f, 5f), this.transform.position.z),
        Quaternion.identity) as GameObject;
        TMPro.TextMeshProUGUI combatTextComponent = combatText.transform.Find("combatText").GetComponent<TMPro.TextMeshProUGUI>();

        if (textType == "damage")
        {
            combatTextComponent.color = Color.red;
            combatTextComponent.text = "-" + quantity.ToString();
        }

        if (textType == "heal")
        {
            combatTextComponent.color = Color.green;
            combatTextComponent.text = "+" + quantity.ToString();
        }
        else if (textType != "damage" && textType != "heal")
        {
            if (quantity > 0)
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
        if (castingCoroutine != null && castingAbility != null)
        {
            castingAbility.StopCoroutine(castingCoroutine);
            castingAbility.GetComponent<AudioSource>().Stop();
        }
        animator.SetBool("casting", false);
        castingAbility = null;
        targetCharacter = null;
        aimLaserScaler.transform.localScale = new Vector3(1, 1, 1);
        aimLaserScaler.SetActive(false);
        castBar.GetComponent<Slider>().value = 0;
        casting = false;
        navMeshAgent.destination = transform.position;
    }

    public void castBarUpdate()
    {
        if (castStartTime == 0)
        {
            castStartTime = Time.time;
            timeSinceCastStart = Time.time - castStartTime;
        }

        if (castBarActive == false)
        {
            castBar.SetActive(true);
            castBarActive = true;
        }

        if (castingAbility != null && timeSinceCastStart < (castingAbility.baseCastTime / (1 + (attackSpeed / 100))))
        {
            castBar.GetComponent<Slider>().value = timeSinceCastStart / (castingAbility.baseCastTime / (1 + (attackSpeed / 100)));
            timeSinceCastStart = Time.time - castStartTime;
        }

        if (castingAbility != null && timeSinceCastStart >= (castingAbility.baseCastTime / (1 + (attackSpeed / 100))))
        {
            castBar.GetComponent<Slider>().value = 0;
            timeSinceCastStart = 0;
            castStartTime = 0;
        }

        if (castingAbility == null)
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

            if (tag == "Player")
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

        if (statToBuff == 0)
        {
            maxHealth+=10;
            spawnLevelUpText(level, "Health");
        }
        else if (statToBuff == 1)
        {
            moveSpeed = moveSpeed + 0.5f;
            spawnLevelUpText(level, "Move Speed");
        }
        else if (statToBuff == 2)
        {
            attackSpeed = attackSpeed + 5;
            spawnLevelUpText(level, "Attack Speed");
        }
        else if (statToBuff == 3)
        {
            cooldownReduction = cooldownReduction + 5;
            spawnLevelUpText(level, "CDR");
        }
        else if (statToBuff == 4)
        {
            armour = armour + 5;
            spawnLevelUpText(level, "Armour");
        }
        else if (statToBuff == 5)
        {
            power = power + 5;
            spawnLevelUpText(level, "Power");
        }
        else if (statToBuff == 6)
        {
            bonusRange = bonusRange + 5;
            spawnLevelUpText(level, "Range");
        }
        else if (statToBuff == 7)
        {
            bonusArea = bonusArea + 5;
            spawnLevelUpText(level, "AoE");
        }
        else if (statToBuff == 8)
        {
            bonusProjectileSpeed = bonusProjectileSpeed + 5;
            spawnLevelUpText(level, "Projectile Speed");
        }
        else if (statToBuff == 9)
        {
            bonusDuration = bonusDuration + 5;
            spawnLevelUpText(level, "Duration");
        }

        currentHealth = maxHealth;

        if (tag == "Player")
        {
            xpBar.GetComponent<Slider>().value = currentXP / maxXP;

            
            if(level>1)
            {
                AudioSource.PlayClipAtPoint(levelUpSound, transform.position, 2);
            }
            
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
        /*
        //select if offensive or supportive
        if (this.gameObject.tag == "Enemy")
        {
            ability.offensive = true;
        }
        else
        {
            int offenseInt = Random.Range(0, 21);
            if (offenseInt < 19)
            {
                ability.offensive = true;
            }
            else if (offenseInt == 19)
            {
                ability.offensive = false;
            }
        }
        
        
        //select a targeting method and type
        int targetingInt = Random.Range(1, 1);
        if (targetingInt == 0)
        {
            ability.targeting = "pointAndClick";
            ability.type = "pointAndClick";
        }
        else if (targetingInt == 1)
        {
        */
            ability.targeting = "direction";
            ability.type = "projectile";

            int projectilesOdds = Random.Range(1, 101);
            if (projectilesOdds <= 70)
            {
                ability.projectileCount = 1;
            }
            else if (projectilesOdds > 70 && projectilesOdds <= 85)
            {
                ability.projectileCount = 2;
            }
            else if (projectilesOdds > 85 && projectilesOdds <= 95)
            {
                ability.projectileCount = 3;
            }
            else if (projectilesOdds > 95 && projectilesOdds<=99) 
            {
                ability.projectileCount = 4;
            }
            else if(projectilesOdds==100)
            {
                ability.projectileCount = 5;
            }

            if(ability.projectileCount >1)
            {
                ability.projectileSpread = Random.Range(1, 15);
            }
            

            //sets if the projectile is piercing
             int piercingInt = Random.Range(0, 5);
            if (piercingInt == 0)
            {
                ability.piercing = true;
            }
            else if (piercingInt == 1)
            {
                ability.piercing = false;
            }

            int returningInt = Random.Range(0, 5);
            if (returningInt == 0)
            {
                ability.returning = true;
            }
            else if (returningInt == 1)
            {
                ability.returning = false;
            }

            int homingInt = Random.Range(0, 5);
            if (homingInt == 0)
            {
                ability.homing = true;
            }
            else if (homingInt == 1)
            {
                ability.homing = false;
            }

            //set projectile size and speed
            ability.projectileSpeed = Random.Range(10, 50);
            ability.projectileSize = Mathf.Round(Random.Range(0.2f, 1f) * 10) * 0.1f;
        /*
        }
        else if (targetingInt == 2)
        {
            ability.targeting = "ground";
            ability.type = "aoe";

        }
        else if (targetingInt == 3)
        {
            //offensive self targets are aoe, supportive can be aoe or point and click
            ability.targeting = "self";
            if (ability.offensive == true)
            {
                ability.type = "aoe";
            }
            else if (ability.offensive == false)
            {
                int selfTypeInt = Random.Range(0, 2);
                {
                    if (selfTypeInt == 0)
                    {
                        ability.type = "pointAndClick";
                    }
                    else if (selfTypeInt == 1)
                    {
                        ability.type = "aoe";
                    }
                }
            }
        }

        //sets the aoe size and duration
        if (ability.type == "aoe")
        {
        */
        int aoeOnHitInt = Random.Range(0, 5);
        if(aoeOnHitInt== 0)
        {
            ability.aoeOnHit = true;

            int lingeringAoeInt = Random.Range(0, 4);
            {
                if(lingeringAoeInt==0)
                {
                    ability.aoeDuration = Random.Range(1, 5);
                }
                else
                {
                    ability.aoeDuration = 0.2f;
                }
            }
            
            ability.baseAoeRadius = Random.Range(5, 10);
        }
            
        //}

        //adds damage if offensive or healing if supportive
       // if (ability.offensive == true)
       // {
            ability.baseDamage = Random.Range(1, 15);
        //  }
        // else if (ability.offensive == false)
        // {
        int healOdds = Random.Range(0, 10);
        if(healOdds == 0)
        {
            ability.baseHealing = Random.Range(1, 5);
        }
            
      //  }

        //adds a range value if not self-targeted
        if (ability.targeting != "self")
        {
            ability.baseRange = Random.Range(3, 20);
            if (ability.targeting == "pointAndClick")
            {
                ability.baseRange = ability.baseRange / 2;
            }
        }

        //assign cast time and cooldown
        ability.baseCastTime = Mathf.Round(Random.Range(0.1f, 1f) * 10) * 0.1f;
        ability.baseCooldown = Random.Range(1, 10);

        //decide how many effects this applies
        int effectsToAdd = Random.Range(0, 3);

        if (effectsToAdd > 0)
        {
            ability.appliesEffect = true;

            //determines effect duration
            ability.effectDuration = Random.Range(1, 10);
        }

        //adds effects if any
        while (effectsToAdd > 0)
        {
            int effectInt = Random.Range(0, 12);
            Debug.Log(effectInt);
            if (effectInt == 0 && ability.dotDamage == 0)
            {
               // if (ability.offensive == true && ability.dotDamage == 0)
               // {
                    ability.dotDamage = Mathf.Round(Random.Range(0.5f, 1f) * 10) * 0.1f;
                    effectsToAdd--;
                /*
                }
                else if (ability.offensive == false && ability.hotHealing == 0)
                {
                    ability.hotHealing = Mathf.Round(Random.Range(0.5f, 1f) * 10) * 0.1f;
                    effectsToAdd--;
                }
                */
            }
            else if (effectInt==11 && ability.hotHealing == 0)
            {
                ability.hotHealing = Mathf.Round(Random.Range(0.5f, 1f) * 10) * 0.1f;
                effectsToAdd--;
            }
            /*
            else if (effectInt == 1 && ability.percentArmourMod == 0)
            {

                if (ability.offensive == true) { ability.percentArmourMod = Random.Range(-10, -25); }
                else if (ability.offensive == false) { ability.percentArmourMod = Random.Range(10, 25); }
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
            */
            else if (effectInt == 1 && ability.flatArmourMod == 0)
            {
                int buffOdds = Random.Range(0, 5);
                //if (ability.offensive == true) 
                if(buffOdds!=0)
                { ability.flatArmourMod = Random.Range(-1, -5); }
               // else if (ability.offensive == false) 
               else if(buffOdds==0)
                { ability.flatArmourMod = Random.Range(1, 5); }
                effectsToAdd--;
            }
            else if (effectInt == 2 && ability.flatPowerMod == 0)
            {

                int buffOdds = Random.Range(0, 5);
                //if (ability.offensive == true) 
                if (buffOdds != 0)
                { ability.flatPowerMod = Random.Range(-1, -5); }
                // else if (ability.offensive == false) 
                else if (buffOdds == 0)
                { ability.flatPowerMod = Random.Range(1, 5); }
                effectsToAdd--;
            }
            else if (effectInt == 3 && ability.flatAttackSpeedMod == 0)
            {

                int buffOdds = Random.Range(0, 5);
                //if (ability.offensive == true) 
                if (buffOdds != 0)
                { ability.flatAttackSpeedMod = Random.Range(-1, -5); }
                // else if (ability.offensive == false) 
                else if (buffOdds == 0)
                { ability.flatAttackSpeedMod = Random.Range(1, 5); }
                effectsToAdd--;
            }
            else if (effectInt == 4 && ability.flatMoveSpeedMod == 0)
            {

                int buffOdds = Random.Range(0, 5);
                //if (ability.offensive == true) 
                if (buffOdds != 0)
                { ability.flatMoveSpeedMod = Random.Range(-1, -5); }
                // else if (ability.offensive == false) 
                else if (buffOdds == 0)
                { ability.flatMoveSpeedMod = Random.Range(1, 5); }
                effectsToAdd--;
            }
            else if (effectInt == 5 && ability.flatCdrMod == 0)
            {

                int buffOdds = Random.Range(0, 5);
                //if (ability.offensive == true) 
                if (buffOdds != 0)
                { ability.flatCdrMod = Random.Range(-1, -5); }
                // else if (ability.offensive == false) 
                else if (buffOdds == 0)
                { ability.flatCdrMod = Random.Range(1, 5); }
                effectsToAdd--;
            }
            else if (effectInt == 6 && ability.flatRangeMod == 0)
            {

                int buffOdds = Random.Range(0, 5);
                //if (ability.offensive == true) 
                if (buffOdds != 0)
                { ability.flatRangeMod = Random.Range(-1, -5); }
                // else if (ability.offensive == false) 
                else if (buffOdds == 0)
                { ability.flatRangeMod = Random.Range(1, 5); }
                effectsToAdd--;
            }
            else if (effectInt == 7 && ability.flatAoeMod == 0)
            {

                int buffOdds = Random.Range(0, 5);
                //if (ability.offensive == true) 
                if (buffOdds != 0)
                { ability.flatAoeMod = Random.Range(-1, -5); }
                // else if (ability.offensive == false) 
                else if (buffOdds == 0)
                { ability.flatAoeMod = Random.Range(1, 5); }
                effectsToAdd--;
            }
            else if (effectInt == 8 && ability.flatProjSpeedMod == 0)
            {

                int buffOdds = Random.Range(0, 5);
                //if (ability.offensive == true) 
                if (buffOdds != 0)
                { ability.flatProjSpeedMod = Random.Range(-1, -5); }
                // else if (ability.offensive == false) 
                else if (buffOdds == 0)
                { ability.flatProjSpeedMod = Random.Range(1, 5); }
                effectsToAdd--;
            }
            else if (effectInt == 9 && ability.flatDurationMod == 0)
            {
                int buffOdds = Random.Range(0, 5);
                //if (ability.offensive == true) 
                if (buffOdds != 0)
                { ability.flatDurationMod = Random.Range(-1, -5); }
                // else if (ability.offensive == false) 
                else if (buffOdds == 0)
                { ability.flatDurationMod = Random.Range(1, 5); }
                effectsToAdd--;
            }
            else if (effectInt == 10)
            {
                if (ability.stun == false)
                {
                    ability.stun = true;
                    ability.effectDuration = ability.effectDuration / 4;
                    ability.baseCooldown = ability.baseCooldown + 5;
                    effectsToAdd--;
                }
            }
            /*stacking removed due to too much impact
            int stackInt = Random.Range(0, 1);
            if (stackInt == 0)
            {
                ability.stackingEffect = false;
            }
            else if (stackInt == 1)
            {
                ability.stackingEffect = true;
                ability.effectDuration = ability.effectDuration / 2;
            }
            */
        }
    }

    public void generateAbilityCore(lootScript ability)
    {
        /*
     //select if offensive or supportive
     if (this.gameObject.tag == "Enemy")
     {
         ability.offensive = true;
     }
     else
     {
         int offenseInt = Random.Range(0, 21);
         if (offenseInt < 19)
         {
             ability.offensive = true;
         }
         else if (offenseInt == 19)
         {
             ability.offensive = false;
         }
     }


     //select a targeting method and type
     int targetingInt = Random.Range(1, 1);
     if (targetingInt == 0)
     {
         ability.targeting = "pointAndClick";
         ability.type = "pointAndClick";
     }
     else if (targetingInt == 1)
     {
     */
        ability.targeting = "direction";
        ability.type = "projectile";

        int projectilesOdds = Random.Range(1, 101);
        if (projectilesOdds <= 70)
        {
            ability.projectileCount = 1;
        }
        else if (projectilesOdds > 70 && projectilesOdds <= 85)
        {
            ability.projectileCount = 2;
        }
        else if (projectilesOdds > 85 && projectilesOdds <= 95)
        {
            ability.projectileCount = 3;
        }
        else if (projectilesOdds > 95 && projectilesOdds <= 99)
        {
            ability.projectileCount = 4;
        }
        else if (projectilesOdds == 100)
        {
            ability.projectileCount = 5;
        }

        if (ability.projectileCount > 1)
        {
            ability.projectileSpread = Random.Range(1, 15);
        }


        //sets if the projectile is piercing
        int piercingInt = Random.Range(0, 5);
        if (piercingInt == 0)
        {
            ability.piercing = true;
        }
        else if (piercingInt == 1)
        {
            ability.piercing = false;
        }

        int returningInt = Random.Range(0, 5);
        if (returningInt == 0)
        {
            ability.returning = true;
        }
        else if (returningInt == 1)
        {
            ability.returning = false;
        }

        int homingInt = Random.Range(0, 5);
        if (homingInt == 0)
        {
            ability.homing = true;
        }
        else if (homingInt == 1)
        {
            ability.homing = false;
        }

        //set projectile size and speed
        ability.projectileSpeed = Random.Range(10, 50);
        ability.projectileSize = Mathf.Round(Random.Range(0.2f, 1f) * 10) * 0.1f;
        /*
        }
        else if (targetingInt == 2)
        {
            ability.targeting = "ground";
            ability.type = "aoe";

        }
        else if (targetingInt == 3)
        {
            //offensive self targets are aoe, supportive can be aoe or point and click
            ability.targeting = "self";
            if (ability.offensive == true)
            {
                ability.type = "aoe";
            }
            else if (ability.offensive == false)
            {
                int selfTypeInt = Random.Range(0, 2);
                {
                    if (selfTypeInt == 0)
                    {
                        ability.type = "pointAndClick";
                    }
                    else if (selfTypeInt == 1)
                    {
                        ability.type = "aoe";
                    }
                }
            }
        }

        //sets the aoe size and duration
        if (ability.type == "aoe")
        {
        */
        int aoeOnHitInt = Random.Range(0, 5);
        if (aoeOnHitInt == 0)
        {
            ability.aoeOnHit = true;
            int lingeringAoeInt = Random.Range(0, 4);
            {
                if (lingeringAoeInt == 0)
                {
                    ability.aoeDuration = Random.Range(1, 6);
                }
                else
                {
                    ability.aoeDuration = 0.2f;
                }
            }

            ability.baseAoeRadius = Random.Range(5, 10);
        }

        //}

        //adds damage if offensive or healing if supportive
        // if (ability.offensive == true)
        // {
        ability.baseDamage = Random.Range(1, 15);
        //  }
        // else if (ability.offensive == false)
        // {
        int healOdds = Random.Range(0, 10);
        if (healOdds == 0)
        {
            ability.baseHealing = Random.Range(1, 5);
        }

        //  }

        //adds a range value if not self-targeted
        if (ability.targeting != "self")
        {
            ability.baseRange = Random.Range(3, 20);
            if (ability.targeting == "pointAndClick")
            {
                ability.baseRange = ability.baseRange / 2;
            }
        }

        //assign cast time and cooldown
        ability.baseCastTime = Mathf.Round(Random.Range(0.1f, 1f) * 10) * 0.1f;
        ability.baseCooldown = Random.Range(1, 10);

        //decide how many effects this applies
        int effectsToAdd = Random.Range(0, 3);

        if (effectsToAdd > 0)
        {
            ability.appliesEffect = true;

            //determines effect duration
            ability.effectDuration = Random.Range(1, 10);
        }

        //adds effects if any
        while (effectsToAdd > 0)
        {
            int effectInt = Random.Range(0, 12);
            Debug.Log(effectInt);

            if (effectInt == 0 && ability.dotDamage == 0)
            {
                // if (ability.offensive == true && ability.dotDamage == 0)
                // {
                ability.dotDamage = Mathf.Round(Random.Range(0.5f, 1f) * 10) * 0.1f;
                effectsToAdd--;
                /*
                }
                else if (ability.offensive == false && ability.hotHealing == 0)
                {
                    ability.hotHealing = Mathf.Round(Random.Range(0.5f, 1f) * 10) * 0.1f;
                    effectsToAdd--;
                }
                */
            }
            else if (effectInt == 11 && ability.hotHealing == 0)
            {
                ability.hotHealing = Mathf.Round(Random.Range(0.5f, 1f) * 10) * 0.1f;
                effectsToAdd--;
            }
            /*
            else if (effectInt == 1 && ability.percentArmourMod == 0)
            {

                if (ability.offensive == true) { ability.percentArmourMod = Random.Range(-10, -25); }
                else if (ability.offensive == false) { ability.percentArmourMod = Random.Range(10, 25); }
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
            */
            else if (effectInt == 1 && ability.flatArmourMod == 0)
            {
                int buffOdds = Random.Range(0, 5);
                //if (ability.offensive == true) 
                if (buffOdds != 0)
                { ability.flatArmourMod = Random.Range(-1, -5); }
                // else if (ability.offensive == false) 
                else if (buffOdds == 0)
                { ability.flatArmourMod = Random.Range(1, 5); }
                effectsToAdd--;
            }
            else if (effectInt == 2 && ability.flatPowerMod == 0)
            {

                int buffOdds = Random.Range(0, 5);
                //if (ability.offensive == true) 
                if (buffOdds != 0)
                { ability.flatPowerMod = Random.Range(-1, -5); }
                // else if (ability.offensive == false) 
                else if (buffOdds == 0)
                { ability.flatPowerMod = Random.Range(1, 5); }
                effectsToAdd--;
            }
            else if (effectInt == 3 && ability.flatAttackSpeedMod == 0)
            {

                int buffOdds = Random.Range(0, 5);
                //if (ability.offensive == true) 
                if (buffOdds != 0)
                { ability.flatAttackSpeedMod = Random.Range(-1, -5); }
                // else if (ability.offensive == false) 
                else if (buffOdds == 0)
                { ability.flatAttackSpeedMod = Random.Range(1, 5); }
                effectsToAdd--;
            }
            else if (effectInt == 4 && ability.flatMoveSpeedMod == 0)
            {

                int buffOdds = Random.Range(0, 5);
                //if (ability.offensive == true) 
                if (buffOdds != 0)
                { ability.flatMoveSpeedMod = Random.Range(-1, -5); }
                // else if (ability.offensive == false) 
                else if (buffOdds == 0)
                { ability.flatMoveSpeedMod = Random.Range(1, 5); }
                effectsToAdd--;
            }
            else if (effectInt == 5 && ability.flatCdrMod == 0)
            {

                int buffOdds = Random.Range(0, 5);
                //if (ability.offensive == true) 
                if (buffOdds != 0)
                { ability.flatCdrMod = Random.Range(-1, -5); }
                // else if (ability.offensive == false) 
                else if (buffOdds == 0)
                { ability.flatCdrMod = Random.Range(1, 5); }
                effectsToAdd--;
            }
            else if (effectInt == 6 && ability.flatRangeMod == 0)
            {

                int buffOdds = Random.Range(0, 5);
                //if (ability.offensive == true) 
                if (buffOdds != 0)
                { ability.flatRangeMod = Random.Range(-1, -5); }
                // else if (ability.offensive == false) 
                else if (buffOdds == 0)
                { ability.flatRangeMod = Random.Range(1, 5); }
                effectsToAdd--;
            }
            else if (effectInt == 7 && ability.flatAoeMod == 0)
            {

                int buffOdds = Random.Range(0, 5);
                //if (ability.offensive == true) 
                if (buffOdds != 0)
                { ability.flatAoeMod = Random.Range(-1, -5); }
                // else if (ability.offensive == false) 
                else if (buffOdds == 0)
                { ability.flatAoeMod = Random.Range(1, 5); }
                effectsToAdd--;
            }
            else if (effectInt == 8 && ability.flatProjSpeedMod == 0)
            {

                int buffOdds = Random.Range(0, 5);
                //if (ability.offensive == true) 
                if (buffOdds != 0)
                { ability.flatProjSpeedMod = Random.Range(-1, -5); }
                // else if (ability.offensive == false) 
                else if (buffOdds == 0)
                { ability.flatProjSpeedMod = Random.Range(1, 5); }
                effectsToAdd--;
            }
            else if (effectInt == 9 && ability.flatDurationMod == 0)
            {
                int buffOdds = Random.Range(0, 5);
                //if (ability.offensive == true) 
                if (buffOdds != 0)
                { ability.flatDurationMod = Random.Range(-1, -5); }
                // else if (ability.offensive == false) 
                else if (buffOdds == 0)
                { ability.flatDurationMod = Random.Range(1, 5); }
                effectsToAdd--;
            }
            else if (effectInt == 10)
            {
                if (ability.stun == false)
                {
                    ability.stun = true;
                    ability.effectDuration = ability.effectDuration / 4;
                    ability.baseCooldown = ability.baseCooldown + 5;
                    effectsToAdd--;
                }
            }
            /*stacking removed due to too much impact
            int stackInt = Random.Range(0, 1);
            if (stackInt == 0)
            {
                ability.stackingEffect = false;
            }
            else if (stackInt == 1)
            {
                ability.stackingEffect = true;
                ability.effectDuration = ability.effectDuration / 2;
            }
            */
        }
    }

    public IEnumerator playWalkSound()
    {
        walkSoundPlaying = true;

        currentWalkSound = Random.Range(0, walkSounds.Count);

        audioSource.PlayOneShot(walkSounds[currentWalkSound]);

        yield return new WaitForSeconds(walkSoundDelay/(navMeshAgent.velocity.magnitude/10));

        if (walking)
        {
            StartCoroutine(playWalkSound());
        }
        else
        {
            walkSoundPlaying = false;
        }
    }
}