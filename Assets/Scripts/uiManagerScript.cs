using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class uiManagerScript : MonoBehaviour
{
    public GameObject statPage;
    public bool statPageActive;
    public bool toolTipActive;
    public GameObject toolTip;
    public TMPro.TextMeshProUGUI toolTipTitle;
    public TMPro.TextMeshProUGUI toolTipText;
    public RectTransform toolTipTransform;
    public GameObject player;
    public baseCharacter playerScript;
    public TMPro.TextMeshProUGUI levelNum;
    public TMPro.TextMeshProUGUI healthNum;
    public TMPro.TextMeshProUGUI xpNum;
    public TMPro.TextMeshProUGUI moveSpeedNum;
    public TMPro.TextMeshProUGUI powerNum;
    public TMPro.TextMeshProUGUI attackSpeedNum;
    public TMPro.TextMeshProUGUI cdrNum;
    public TMPro.TextMeshProUGUI armourNum;
    public TMPro.TextMeshProUGUI rangeNum;
    public TMPro.TextMeshProUGUI areaNum;
    public TMPro.TextMeshProUGUI durationNum;
    public TMPro.TextMeshProUGUI projectileSpeedNum;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("playerCharacter(Clone)");
        playerScript = player.GetComponent<baseCharacter>();

        statPage = transform.Find("statPageBackground").gameObject;

        levelNum = statPage.transform.Find("levelNum").gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        healthNum = statPage.transform.Find("healthNum").gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        xpNum = statPage.transform.Find("xpNum").gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        moveSpeedNum = statPage.transform.Find("moveSpeedNum").gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        powerNum = statPage.transform.Find("powerNum").gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        attackSpeedNum = statPage.transform.Find("attackSpeedNum").gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        cdrNum = statPage.transform.Find("cooldownReductionNum").gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        armourNum = statPage.transform.Find("armourNum").gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        rangeNum = statPage.transform.Find("bonusRangeNum").gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        areaNum = statPage.transform.Find("bonusAreaNum").gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        durationNum = statPage.transform.Find("bonusDurationNum").gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        projectileSpeedNum = statPage.transform.Find("projectileSpeedNum").gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        statPage.SetActive(false);
        statPageActive = false;

        toolTip = transform.Find("toolTip").gameObject;
        toolTipTransform = toolTip.GetComponent<RectTransform>();
        toolTipTitle = toolTip.transform.Find("toolTipTitle").gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        toolTipText = toolTip.transform.Find("toolTipText").gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        toolTip.SetActive(false);
        toolTipActive= false;

        
    }

    void Update()
    {
        if (Input.GetButtonDown("c"))
        {
            if(statPageActive==false)
            {
                statPageActive=true;
                statPage.SetActive(true);
                fillStatPage();
            }
            else if(statPageActive==true)
            {
                statPageActive=false;
                statPage.SetActive(false);
            }
        }
    }

    public void showToolTip(int abilityNum)
    {
        toolTipActive = true;
        toolTip.SetActive(true);
        toolTipTitle.text = "Ability" + abilityNum.ToString();
        baseAbilityScript abilityScript= player.transform.Find("ability" + abilityNum.ToString()).gameObject.GetComponent<baseAbilityScript>();
        toolTipText.text = "Type: "+ abilityScript.type + "<br>";
        toolTipText.text = toolTipText.text + "Targeting: " + abilityScript.targeting + "<br>";

        if(abilityScript.baseDamage>0)
        {
            toolTipText.text = toolTipText.text + "Damage: " + abilityScript.baseDamage + "<br>";
            toolTipTransform.sizeDelta = new Vector2(toolTipTransform.sizeDelta.x, toolTip.GetComponent<RectTransform>().sizeDelta.y + 23);
        }

        if(abilityScript.baseHealing>0)
        {
            toolTipText.text = toolTipText.text + "Healing: " + abilityScript.baseHealing + "<br>";
            toolTipTransform.sizeDelta = new Vector2(toolTipTransform.sizeDelta.x, toolTip.GetComponent<RectTransform>().sizeDelta.y + 23);
        }

        if (abilityScript.baseRange > 0)
        {
            toolTipText.text = toolTipText.text + "Range: " + abilityScript.baseRange + "<br>";
            toolTipTransform.sizeDelta = new Vector2(toolTipTransform.sizeDelta.x, toolTip.GetComponent<RectTransform>().sizeDelta.y + 23);
        }

        if (abilityScript.baseAoeRadius > 0)
        {
            toolTipText.text = toolTipText.text + "AoE: " + abilityScript.baseAoeRadius + "<br>";
            toolTipTransform.sizeDelta = new Vector2(toolTipTransform.sizeDelta.x, toolTip.GetComponent<RectTransform>().sizeDelta.y + 23);
        }

        if (abilityScript.aoeDuration > 0)
        {
            toolTipText.text = toolTipText.text + "AoE Duration: " + abilityScript.aoeDuration + "<br>";
            toolTipTransform.sizeDelta = new Vector2(toolTipTransform.sizeDelta.x, toolTip.GetComponent<RectTransform>().sizeDelta.y + 23);
        }

        if (abilityScript.baseCastTime > 0)
        {
            toolTipText.text = toolTipText.text + "Cast Time: " + abilityScript.baseCastTime + "<br>";
            toolTipTransform.sizeDelta = new Vector2(toolTipTransform.sizeDelta.x, toolTip.GetComponent<RectTransform>().sizeDelta.y + 23);
        }

        if (abilityScript.baseCooldown > 0)
        {
            toolTipText.text = toolTipText.text + "Cooldown: " + abilityScript.baseCooldown + "<br>";
            toolTipTransform.sizeDelta = new Vector2(toolTipTransform.sizeDelta.x, toolTip.GetComponent<RectTransform>().sizeDelta.y + 23);
        }

        if (abilityScript.appliesEffect == true)
        {
            toolTipText.text = toolTipText.text + "Applies Effect: " + "Yes" + "<br>";
            toolTipTransform.sizeDelta = new Vector2(toolTipTransform.sizeDelta.x, toolTip.GetComponent<RectTransform>().sizeDelta.y + 23);
        }

        if (abilityScript.stackingEffect == true)
        {
            toolTipText.text = toolTipText.text + "Stacking Effect: " + "Yes" + "<br>";
            toolTipTransform.sizeDelta = new Vector2(toolTipTransform.sizeDelta.x, toolTip.GetComponent<RectTransform>().sizeDelta.y + 23);
        }

        if (abilityScript.dotDamage > 0)
        {
            toolTipText.text = toolTipText.text + "DoT: " + abilityScript.dotDamage + "<br>";
            toolTipTransform.sizeDelta = new Vector2(toolTipTransform.sizeDelta.x, toolTip.GetComponent<RectTransform>().sizeDelta.y + 23);
        }

        if (abilityScript.hotHealing > 0)
        {
            toolTipText.text = toolTipText.text + "HoT: " + abilityScript.hotHealing + "<br>";
            toolTipTransform.sizeDelta = new Vector2(toolTipTransform.sizeDelta.x, toolTip.GetComponent<RectTransform>().sizeDelta.y + 23);
        }

        if (abilityScript.effectDuration > 0)
        {
            toolTipText.text = toolTipText.text + "Effect Duration: " + abilityScript.effectDuration + "<br>";
            toolTipTransform.sizeDelta = new Vector2(toolTipTransform.sizeDelta.x, toolTip.GetComponent<RectTransform>().sizeDelta.y + 23);
        }

        if (abilityScript.percentArmourMod > 0)
        {
            toolTipText.text = toolTipText.text + "% Armour Mod: " + abilityScript.percentArmourMod + "<br>";
            toolTipTransform.sizeDelta = new Vector2(toolTipTransform.sizeDelta.x, toolTip.GetComponent<RectTransform>().sizeDelta.y + 23);
        }

        if (abilityScript.percentPowerMod > 0)
        {
            toolTipText.text = toolTipText.text + "% Power Mod: " + abilityScript.percentPowerMod + "<br>";
            toolTipTransform.sizeDelta = new Vector2(toolTipTransform.sizeDelta.x, toolTip.GetComponent<RectTransform>().sizeDelta.y + 23);
        }

        if (abilityScript.percentAttackSpeedMod > 0)
        {
            toolTipText.text = toolTipText.text + "% Attack Speed Mod: " + abilityScript.percentAttackSpeedMod + "<br>";
            toolTipTransform.sizeDelta = new Vector2(toolTipTransform.sizeDelta.x, toolTip.GetComponent<RectTransform>().sizeDelta.y + 23);
        }

        if (abilityScript.percentCdrMod > 0)
        {
            toolTipText.text = toolTipText.text + "% CDR Mod: " + abilityScript.percentCdrMod + "<br>";
            toolTipTransform.sizeDelta = new Vector2(toolTipTransform.sizeDelta.x, toolTip.GetComponent<RectTransform>().sizeDelta.y + 23);
        }

        if (abilityScript.percentMoveSpeedMod > 0)
        {
            toolTipText.text = toolTipText.text + "% Move Speed Mod: " + abilityScript.percentMoveSpeedMod + "<br>";
            toolTipTransform.sizeDelta = new Vector2(toolTipTransform.sizeDelta.x, toolTip.GetComponent<RectTransform>().sizeDelta.y + 23);
        }

        if (abilityScript.percentRangeMod > 0)
        {
            toolTipText.text = toolTipText.text + "% Range Mod: " + abilityScript.percentRangeMod + "<br>";
            toolTipTransform.sizeDelta = new Vector2(toolTipTransform.sizeDelta.x, toolTip.GetComponent<RectTransform>().sizeDelta.y + 23);
        }

        if (abilityScript.percentAoeMod > 0)
        {
            toolTipText.text = toolTipText.text + "% AoE Mod: " + abilityScript.percentAoeMod + "<br>";
            toolTipTransform.sizeDelta = new Vector2(toolTipTransform.sizeDelta.x, toolTip.GetComponent<RectTransform>().sizeDelta.y + 23);
        }

        if (abilityScript.flatArmourMod > 0)
        {
            toolTipText.text = toolTipText.text + "Armour Mod: " + abilityScript.flatArmourMod + "<br>";
            toolTipTransform.sizeDelta = new Vector2(toolTipTransform.sizeDelta.x, toolTip.GetComponent<RectTransform>().sizeDelta.y + 23);
        }

        if (abilityScript.flatPowerMod > 0)
        {
            toolTipText.text = toolTipText.text + "Power Mod: " + abilityScript.flatPowerMod + "<br>";
            toolTipTransform.sizeDelta = new Vector2(toolTipTransform.sizeDelta.x, toolTip.GetComponent<RectTransform>().sizeDelta.y + 23);
        }

        if (abilityScript.flatAttackSpeedMod > 0)
        {
            toolTipText.text = toolTipText.text + "Attack Speed Mod: " + abilityScript.flatAttackSpeedMod + "<br>";
            toolTipTransform.sizeDelta = new Vector2(toolTipTransform.sizeDelta.x, toolTip.GetComponent<RectTransform>().sizeDelta.y + 23);
        }

        if (abilityScript.flatCdrMod > 0)
        {
            toolTipText.text = toolTipText.text + "CDR Mod: " + abilityScript.flatCdrMod + "<br>";
            toolTipTransform.sizeDelta = new Vector2(toolTipTransform.sizeDelta.x, toolTip.GetComponent<RectTransform>().sizeDelta.y + 23);
        }

        if (abilityScript.flatMoveSpeedMod > 0)
        {
            toolTipText.text = toolTipText.text + "Move Speed Mod: " + abilityScript.flatMoveSpeedMod + "<br>";
            toolTipTransform.sizeDelta = new Vector2(toolTipTransform.sizeDelta.x, toolTip.GetComponent<RectTransform>().sizeDelta.y + 23);
        }

        if (abilityScript.flatRangeMod > 0)
        {
            toolTipText.text = toolTipText.text + "Range Mod: " + abilityScript.flatRangeMod + "<br>";
            toolTipTransform.sizeDelta = new Vector2(toolTipTransform.sizeDelta.x, toolTip.GetComponent<RectTransform>().sizeDelta.y + 23);
        }

        if (abilityScript.flatAoeMod > 0)
        {
            toolTipText.text = toolTipText.text + "AoE Mod: " + abilityScript.flatAoeMod + "<br>";
            toolTipTransform.sizeDelta = new Vector2(toolTipTransform.sizeDelta.x, toolTip.GetComponent<RectTransform>().sizeDelta.y + 23);
        }

        if (abilityScript.projectileSize > 0)
        {
            toolTipText.text = toolTipText.text + "Projectile Size: " + abilityScript.projectileSize + "<br>";
            toolTipTransform.sizeDelta = new Vector2(toolTipTransform.sizeDelta.x, toolTip.GetComponent<RectTransform>().sizeDelta.y + 23);
        }

        if (abilityScript.projectileSpeed > 0)
        {
            toolTipText.text = toolTipText.text + "Projectile Speed: " + abilityScript.projectileSpeed + "<br>";
            toolTipTransform.sizeDelta = new Vector2(toolTipTransform.sizeDelta.x, toolTip.GetComponent<RectTransform>().sizeDelta.y + 23);
        }

        if (abilityScript.piercing == true)
        {
            toolTipText.text = toolTipText.text + "Piercing Projectile: " + "Yes" + "<br>";
            toolTipTransform.sizeDelta = new Vector2(toolTipTransform.sizeDelta.x, toolTip.GetComponent<RectTransform>().sizeDelta.y + 23);
        }
    }

    public void hideToolTip()
    {
        toolTipTransform.sizeDelta = new Vector2(toolTipTransform.sizeDelta.x, 95);
        toolTipActive = false;
        toolTip.SetActive(false);
    }

    public void fillStatPage()
    {
        levelNum.text = playerScript.level.ToString();
        healthNum.text = playerScript.currentHealth.ToString() + "/"+ playerScript.maxHealth.ToString();
        xpNum.text = playerScript.currentXP.ToString() + "/" + playerScript.maxXP.ToString();
        moveSpeedNum.text = playerScript.moveSpeed.ToString();
        powerNum.text = "+"+playerScript.power.ToString()+"%";
        attackSpeedNum.text = "+" + playerScript.attackSpeed.ToString() + "%";
        cdrNum.text = "+" + playerScript.cooldownReduction.ToString() + "%";
        armourNum.text = "+" + playerScript.armour.ToString() + "%";
        rangeNum.text = "+" + playerScript.bonusRange.ToString() + "%";
        areaNum.text = "+" + playerScript.bonusArea.ToString() + "%";
        durationNum.text = "+" + playerScript.bonusDuration.ToString() + "%";
        projectileSpeedNum.text = "+" + playerScript.bonusProjectileSpeed.ToString() + "%";
    }
}
