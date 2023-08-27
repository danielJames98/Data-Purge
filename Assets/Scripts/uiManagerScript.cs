using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
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
    public GameObject healthBar;
    public GameObject xpBar;
    public GameObject castBar;
    public GameObject abilityButton0;
    public GameObject abilityButton1;
    public GameObject abilityButton2;
    public GameObject abilityButton3;
    public GameObject abilityButton4;
    public playerController playerScript;
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

    public GameObject inventoryUI;
    public bool inventoryUIActive;

    public GameObject inventorySlotUI0;
    public GameObject inventorySlotUI1;
    public GameObject inventorySlotUI2;
    public GameObject inventorySlotUI3;
    public GameObject inventorySlotUI4;
    public GameObject inventorySlotUI5;
    public GameObject inventorySlotUI6;
    public GameObject inventorySlotUI7;
    public GameObject inventorySlotUI8;
    public GameObject inventorySlotUI9;
    public GameObject inventorySlotUI10;
    public GameObject inventorySlotUI11;
    public GameObject inventorySlotUI12;
    public GameObject inventorySlotUI13;
    public GameObject inventorySlotUI14;
    public GameObject inventorySlotUI15;
    public GameObject inventorySlotUI16;
    public GameObject inventorySlotUI17;
    public GameObject inventorySlotUI18;
    public GameObject inventorySlotUI19;

    public GameObject tempAbility;
    public baseAbilityScript tempAbilityScript;

    public bool abilitySelected;

    public baseAbilityScript selectedAbilityScript;
    public baseAbilityScript selectedAbilityScriptInv;
    public baseAbilityScript targetAbilityScript;
    public baseAbilityScript targetAbilityScriptInv;

    public gameManagerScript gameManager;
    public GameObject dialogueDisplay;
    public AudioSource dialogueAudioSource;
    public TMPro.TextMeshProUGUI dialogueText;
    public TMPro.TextMeshProUGUI dialogueSpeakerName;

    public AudioClip introAudio;
    public AudioClip levelCompleteAudio1;
    public AudioClip levelCompleteAudio2;
    public AudioClip levelCompleteAudio3;
    public AudioClip levelCompleteAudio4;
    public AudioClip levelCompleteAudio5;
    public AudioClip hack0Audio;
    public AudioClip hack1Audio;
    public AudioClip hack2Audio;
    public AudioClip hack3Audio;
    public AudioClip hack4Audio;
    public AudioClip hack5Audio;
    public AudioClip finalHackAudio;
    public AudioClip finalBossAudio;
    public AudioClip finalBossDefeatedAudio;
    public AudioClip notLeavingAudio;
    public AudioClip levelUpSound;
    public AudioClip launchFinalLevelAudio;

    public string introText;
    public string levelCompleteText1;
    public string levelCompleteText2;
    public string levelCompleteText3;
    public string levelCompleteText4;
    public string levelCompleteText5;
    public string hack0Text;
    public string hack1Text;
    public string hack2Text;
    public string hack3Text;
    public string hack4Text;
    public string hack5Text;
    public string finalBossText;
    public string finalBossDefeatedText;
    public string notLeavingText;

    public bool storyPlaying;

    public int nextHack;

    public GameObject finalBossButton;

    void Start()
    {
        gameObject.GetComponent<Canvas>().worldCamera = GameObject.Find("Main Camera(Clone)").GetComponent<Camera>();
        gameManager = GameObject.Find("gameManager").GetComponent<gameManagerScript>();
        gameManager.inGameUI = this.gameObject;

        healthBar = transform.Find("playerHealthBar").gameObject;
        xpBar = transform.Find("playerHealthBar").gameObject;
        castBar = transform.Find("playerHealthBar").gameObject;
        abilityButton0 = transform.Find("playerHealthBar").gameObject;
        abilityButton1 = transform.Find("playerHealthBar").gameObject;
        abilityButton2 = transform.Find("playerHealthBar").gameObject;
        abilityButton3 = transform.Find("playerHealthBar").gameObject;
        abilityButton4 = transform.Find("playerHealthBar").gameObject;
        player = GameObject.Find("playerCharacter(Clone)");
        playerScript = player.GetComponent<playerController>();

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
        toolTipActive = false;

        inventoryUI = transform.Find("inventory").gameObject;

        inventoryUI.SetActive(false);
        inventoryUIActive = false;

        tempAbility = transform.Find("tempAbility").gameObject;
        tempAbilityScript = tempAbility.GetComponent<baseAbilityScript>();

        showIntroDialogue();

        
    }

    void Update()
    {
        if (Input.GetButtonDown("c"))
        {
            if (statPageActive == false)
            {
                statPageActive = true;
                statPage.SetActive(true);
                fillStatPage();
            }
            else if (statPageActive == true)
            {
                statPageActive = false;
                statPage.SetActive(false);
            }
        }

        if (Input.GetButtonDown("i"))
        {
            if (inventoryUIActive == false)
            {
                inventoryUIActive = true;
                inventoryUI.SetActive(true);
                fillInventoryUI();
                playerScript.inventoryOpen = true;
            }
            else if (inventoryUIActive == true)
            {
                inventoryUIActive = false;
                inventoryUI.SetActive(false);
                hideToolTip();
                playerScript.inventoryOpen = false;
            }
        }

        if (storyPlaying == true && dialogueAudioSource.isPlaying == false)
        {
            storySceneDone();
        }
    }

    public void fillInventoryUI()
    {
        int i = 0;

        foreach (baseAbilityScript item in player.GetComponent<playerController>().inventoryItems)
        {
            if (item.type == "" || item.type == null)
            {
                inventoryUI.transform.Find("inventorySlot" + i).gameObject.GetComponent<Image>().color = Color.grey;
            }
            else if (item.type != "")
            {
                inventoryUI.transform.Find("inventorySlot" + i).gameObject.GetComponent<Image>().color = Color.white;
            }
            i++;
        }
    }

    public void showToolTip(int abilityNum, bool equipped)
    {
        if (equipped == true)
        {
            if (player.transform.Find("ability" + abilityNum.ToString()).gameObject.GetComponent<baseAbilityScript>().type != "")
            {
                toolTipActive = true;
                toolTip.SetActive(true);
                toolTipTitle.text = "Ability" + abilityNum.ToString();
                fillToolTipEquipped(abilityNum);
            }
        }
        else if (equipped == false)
        {
            if (player.transform.Find("inventory").transform.Find("inventorySlot" + abilityNum.ToString()).gameObject.GetComponent<baseAbilityScript>().type != "")
            {
                toolTipActive = true;
                toolTip.SetActive(true);
                toolTipTitle.text = "Ability" + abilityNum.ToString();
                fillToolTipInventory(abilityNum);
            }
        }
    }
    public void fillToolTipInventory(int abilityNum)
    {
        baseAbilityScript abilityScript = player.transform.Find("inventory").transform.Find("inventorySlot" + abilityNum.ToString()).gameObject.GetComponent<baseAbilityScript>();

        toolTipText.text = "Type: " + abilityScript.type + "<br>";
        toolTipText.text = toolTipText.text + "Targeting: " + abilityScript.targeting + "<br>";


        if (abilityScript.baseDamage > 0)
        {
            toolTipText.text = toolTipText.text + "Damage: " + abilityScript.baseDamage + "<br>";
            toolTipTransform.sizeDelta = new Vector2(toolTipTransform.sizeDelta.x, toolTip.GetComponent<RectTransform>().sizeDelta.y + 23);
        }

        if (abilityScript.baseHealing > 0)
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

        if (abilityScript.percentProjSpeedMod > 0)
        {
            toolTipText.text = toolTipText.text + "% Projectile Speed Mod: " + abilityScript.percentProjSpeedMod + "<br>";
            toolTipTransform.sizeDelta = new Vector2(toolTipTransform.sizeDelta.x, toolTip.GetComponent<RectTransform>().sizeDelta.y + 23);
        }

        if (abilityScript.percentDurationMod > 0)
        {
            toolTipText.text = toolTipText.text + "% Duration Mod: " + abilityScript.percentDurationMod + "<br>";
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

        if (abilityScript.flatProjSpeedMod > 0)
        {
            toolTipText.text = toolTipText.text + "Flat Projectile Speed Mod: " + abilityScript.flatProjSpeedMod + "<br>";
            toolTipTransform.sizeDelta = new Vector2(toolTipTransform.sizeDelta.x, toolTip.GetComponent<RectTransform>().sizeDelta.y + 23);
        }

        if (abilityScript.flatDurationMod > 0)
        {
            toolTipText.text = toolTipText.text + "flat Duration Mod: " + abilityScript.flatDurationMod + "<br>";
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
    public void fillToolTipEquipped(int abilityNum)
    {
        baseAbilityScript abilityScript = player.transform.Find("ability" + abilityNum.ToString()).gameObject.GetComponent<baseAbilityScript>();

        toolTipText.text = "Type: " + abilityScript.type + "<br>";
        toolTipText.text = toolTipText.text + "Targeting: " + abilityScript.targeting + "<br>";

        if (abilityScript.baseDamage > 0)
        {
            toolTipText.text = toolTipText.text + "Damage: " + abilityScript.baseDamage + "<br>";
            toolTipTransform.sizeDelta = new Vector2(toolTipTransform.sizeDelta.x, toolTip.GetComponent<RectTransform>().sizeDelta.y + 23);
        }

        if (abilityScript.baseHealing > 0)
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

        if (abilityScript.percentProjSpeedMod > 0)
        {
            toolTipText.text = toolTipText.text + "% Projectile Speed Mod: " + abilityScript.percentProjSpeedMod + "<br>";
            toolTipTransform.sizeDelta = new Vector2(toolTipTransform.sizeDelta.x, toolTip.GetComponent<RectTransform>().sizeDelta.y + 23);
        }

        if (abilityScript.percentDurationMod > 0)
        {
            toolTipText.text = toolTipText.text + "% Duration Mod: " + abilityScript.percentDurationMod + "<br>";
            toolTipTransform.sizeDelta = new Vector2(toolTipTransform.sizeDelta.x, toolTip.GetComponent<RectTransform>().sizeDelta.y + 23);
        }

        if (abilityScript.flatArmourMod != 0)
        {
            toolTipText.text = toolTipText.text + "Armour Mod: " + abilityScript.flatArmourMod + "<br>";
            toolTipTransform.sizeDelta = new Vector2(toolTipTransform.sizeDelta.x, toolTip.GetComponent<RectTransform>().sizeDelta.y + 23);
        }

        if (abilityScript.flatPowerMod != 0)
        {
            toolTipText.text = toolTipText.text + "Power Mod: " + abilityScript.flatPowerMod + "<br>";
            toolTipTransform.sizeDelta = new Vector2(toolTipTransform.sizeDelta.x, toolTip.GetComponent<RectTransform>().sizeDelta.y + 23);
        }

        if (abilityScript.flatAttackSpeedMod != 0)
        {
            toolTipText.text = toolTipText.text + "Attack Speed Mod: " + abilityScript.flatAttackSpeedMod + "<br>";
            toolTipTransform.sizeDelta = new Vector2(toolTipTransform.sizeDelta.x, toolTip.GetComponent<RectTransform>().sizeDelta.y + 23);
        }

        if (abilityScript.flatCdrMod != 0)
        {
            toolTipText.text = toolTipText.text + "CDR Mod: " + abilityScript.flatCdrMod + "<br>";
            toolTipTransform.sizeDelta = new Vector2(toolTipTransform.sizeDelta.x, toolTip.GetComponent<RectTransform>().sizeDelta.y + 23);
        }

        if (abilityScript.flatMoveSpeedMod != 0)
        {
            toolTipText.text = toolTipText.text + "Move Speed Mod: " + abilityScript.flatMoveSpeedMod + "<br>";
            toolTipTransform.sizeDelta = new Vector2(toolTipTransform.sizeDelta.x, toolTip.GetComponent<RectTransform>().sizeDelta.y + 23);
        }

        if (abilityScript.flatRangeMod != 0)
        {
            toolTipText.text = toolTipText.text + "Range Mod: " + abilityScript.flatRangeMod + "<br>";
            toolTipTransform.sizeDelta = new Vector2(toolTipTransform.sizeDelta.x, toolTip.GetComponent<RectTransform>().sizeDelta.y + 23);
        }

        if (abilityScript.flatAoeMod != 0)
        {
            toolTipText.text = toolTipText.text + "AoE Mod: " + abilityScript.flatAoeMod + "<br>";
            toolTipTransform.sizeDelta = new Vector2(toolTipTransform.sizeDelta.x, toolTip.GetComponent<RectTransform>().sizeDelta.y + 23);
        }

        if (abilityScript.flatProjSpeedMod != 0)
        {
            toolTipText.text = toolTipText.text + "Proj Speed Mod: " + abilityScript.flatProjSpeedMod + "<br>";
            toolTipTransform.sizeDelta = new Vector2(toolTipTransform.sizeDelta.x, toolTip.GetComponent<RectTransform>().sizeDelta.y + 23);
        }

        if (abilityScript.flatDurationMod != 0)
        {
            toolTipText.text = toolTipText.text + "Duration Mod: " + abilityScript.flatDurationMod + "<br>";
            toolTipTransform.sizeDelta = new Vector2(toolTipTransform.sizeDelta.x, toolTip.GetComponent<RectTransform>().sizeDelta.y + 23);
        }

        if (abilityScript.projectileSize != 0)
        {
            toolTipText.text = toolTipText.text + "Proj Size: " + abilityScript.projectileSize + "<br>";
            toolTipTransform.sizeDelta = new Vector2(toolTipTransform.sizeDelta.x, toolTip.GetComponent<RectTransform>().sizeDelta.y + 23);
        }

        if (abilityScript.projectileSpeed != 0)
        {
            toolTipText.text = toolTipText.text + "Proj Speed: " + abilityScript.projectileSpeed + "<br>";
            toolTipTransform.sizeDelta = new Vector2(toolTipTransform.sizeDelta.x, toolTip.GetComponent<RectTransform>().sizeDelta.y + 23);
        }

        if (abilityScript.piercing == true)
        {
            toolTipText.text = toolTipText.text + "Piercing: " + "Yes" + "<br>";
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
        healthNum.text = playerScript.currentHealth.ToString() + "/" + playerScript.maxHealth.ToString();
        xpNum.text = playerScript.currentXP.ToString() + "/" + playerScript.maxXP.ToString();
        moveSpeedNum.text = playerScript.moveSpeed.ToString();
        powerNum.text = "+" + playerScript.power.ToString() + "%";
        attackSpeedNum.text = "+" + playerScript.attackSpeed.ToString() + "%";
        cdrNum.text = "+" + playerScript.cooldownReduction.ToString() + "%";
        armourNum.text = "+" + playerScript.armour.ToString() + "%";
        rangeNum.text = "+" + playerScript.bonusRange.ToString() + "%";
        areaNum.text = "+" + playerScript.bonusArea.ToString() + "%";
        durationNum.text = "+" + playerScript.bonusDuration.ToString() + "%";
        projectileSpeedNum.text = "+" + playerScript.bonusProjectileSpeed.ToString() + "%";
    }

    public void selectAbility(int abilityNum, bool equipped)
    {
        if (abilitySelected == false)
        {
            abilitySelected = true;
            if (equipped == true)
            {
                selectedAbilityScript = player.transform.Find("ability" + abilityNum.ToString()).gameObject.GetComponent<baseAbilityScript>();

                tempAbilityScript.type = selectedAbilityScript.type;
                tempAbilityScript.targeting = selectedAbilityScript.targeting;
                tempAbilityScript.baseDamage = selectedAbilityScript.baseDamage;
                tempAbilityScript.baseHealing = selectedAbilityScript.baseHealing;
                tempAbilityScript.baseRange = selectedAbilityScript.baseRange;
                tempAbilityScript.baseAoeRadius = selectedAbilityScript.baseAoeRadius;
                tempAbilityScript.baseCastTime = selectedAbilityScript.baseCastTime;
                tempAbilityScript.baseCooldown = selectedAbilityScript.baseCooldown;
                tempAbilityScript.appliesEffect = selectedAbilityScript.appliesEffect;
                tempAbilityScript.dotDamage = selectedAbilityScript.dotDamage;
                tempAbilityScript.hotHealing = selectedAbilityScript.hotHealing;
                tempAbilityScript.effectDuration = selectedAbilityScript.effectDuration;
                tempAbilityScript.stackingEffect = selectedAbilityScript.stackingEffect;
                tempAbilityScript.percentArmourMod = selectedAbilityScript.percentArmourMod;
                tempAbilityScript.percentPowerMod = selectedAbilityScript.percentPowerMod;
                tempAbilityScript.percentAttackSpeedMod = selectedAbilityScript.percentAttackSpeedMod;
                tempAbilityScript.percentMoveSpeedMod = selectedAbilityScript.percentMoveSpeedMod;
                tempAbilityScript.percentCdrMod = selectedAbilityScript.percentCdrMod;
                tempAbilityScript.percentRangeMod = selectedAbilityScript.percentRangeMod;
                tempAbilityScript.percentAoeMod = selectedAbilityScript.percentAoeMod;
                tempAbilityScript.percentProjSpeedMod = selectedAbilityScript.percentProjSpeedMod;
                tempAbilityScript.percentDurationMod = selectedAbilityScript.percentDurationMod;
                tempAbilityScript.flatArmourMod = selectedAbilityScript.flatArmourMod;
                tempAbilityScript.flatPowerMod = selectedAbilityScript.flatPowerMod;
                tempAbilityScript.flatAttackSpeedMod = selectedAbilityScript.flatAttackSpeedMod;
                tempAbilityScript.flatMoveSpeedMod = selectedAbilityScript.flatMoveSpeedMod;
                tempAbilityScript.flatCdrMod = selectedAbilityScript.flatCdrMod;
                tempAbilityScript.flatRangeMod = selectedAbilityScript.flatRangeMod;
                tempAbilityScript.flatAoeMod = selectedAbilityScript.flatAoeMod;
                tempAbilityScript.flatProjSpeedMod = selectedAbilityScript.flatProjSpeedMod;
                tempAbilityScript.flatDurationMod = selectedAbilityScript.flatDurationMod;
                tempAbilityScript.projectileSpeed = selectedAbilityScript.projectileSpeed;
                tempAbilityScript.piercing = selectedAbilityScript.piercing;
                tempAbilityScript.projectileSize = selectedAbilityScript.projectileSize;
                tempAbilityScript.aoeDuration = selectedAbilityScript.aoeDuration;
                tempAbilityScript.offensive = selectedAbilityScript.offensive;
                tempAbilityScript.stun = selectedAbilityScript.stun;

            }
            else if (equipped == false)
            {
                selectedAbilityScript = player.transform.Find("inventory").transform.Find("inventorySlot" + abilityNum.ToString()).gameObject.GetComponent<baseAbilityScript>();

                tempAbilityScript.type = selectedAbilityScript.type;
                tempAbilityScript.targeting = selectedAbilityScript.targeting;
                tempAbilityScript.baseDamage = selectedAbilityScript.baseDamage;
                tempAbilityScript.baseHealing = selectedAbilityScript.baseHealing;
                tempAbilityScript.baseRange = selectedAbilityScript.baseRange;
                tempAbilityScript.baseAoeRadius = selectedAbilityScript.baseAoeRadius;
                tempAbilityScript.baseCastTime = selectedAbilityScript.baseCastTime;
                tempAbilityScript.baseCooldown = selectedAbilityScript.baseCooldown;
                tempAbilityScript.appliesEffect = selectedAbilityScript.appliesEffect;
                tempAbilityScript.dotDamage = selectedAbilityScript.dotDamage;
                tempAbilityScript.hotHealing = selectedAbilityScript.hotHealing;
                tempAbilityScript.effectDuration = selectedAbilityScript.effectDuration;
                tempAbilityScript.stackingEffect = selectedAbilityScript.stackingEffect;
                tempAbilityScript.percentArmourMod = selectedAbilityScript.percentArmourMod;
                tempAbilityScript.percentPowerMod = selectedAbilityScript.percentPowerMod;
                tempAbilityScript.percentAttackSpeedMod = selectedAbilityScript.percentAttackSpeedMod;
                tempAbilityScript.percentMoveSpeedMod = selectedAbilityScript.percentMoveSpeedMod;
                tempAbilityScript.percentCdrMod = selectedAbilityScript.percentCdrMod;
                tempAbilityScript.percentRangeMod = selectedAbilityScript.percentRangeMod;
                tempAbilityScript.percentAoeMod = selectedAbilityScript.percentAoeMod;
                tempAbilityScript.percentProjSpeedMod = selectedAbilityScript.percentProjSpeedMod;
                tempAbilityScript.percentDurationMod = selectedAbilityScript.percentDurationMod;
                tempAbilityScript.flatArmourMod = selectedAbilityScript.flatArmourMod;
                tempAbilityScript.flatPowerMod = selectedAbilityScript.flatPowerMod;
                tempAbilityScript.flatAttackSpeedMod = selectedAbilityScript.flatAttackSpeedMod;
                tempAbilityScript.flatMoveSpeedMod = selectedAbilityScript.flatMoveSpeedMod;
                tempAbilityScript.flatCdrMod = selectedAbilityScript.flatCdrMod;
                tempAbilityScript.flatRangeMod = selectedAbilityScript.flatRangeMod;
                tempAbilityScript.flatAoeMod = selectedAbilityScript.flatAoeMod;
                tempAbilityScript.flatProjSpeedMod = selectedAbilityScript.flatProjSpeedMod;
                tempAbilityScript.flatDurationMod = selectedAbilityScript.flatDurationMod;
                tempAbilityScript.projectileSpeed = selectedAbilityScript.projectileSpeed;
                tempAbilityScript.piercing = selectedAbilityScript.piercing;
                tempAbilityScript.projectileSize = selectedAbilityScript.projectileSize;
                tempAbilityScript.aoeDuration = selectedAbilityScript.aoeDuration;
                tempAbilityScript.offensive = selectedAbilityScript.offensive;
                tempAbilityScript.stun = selectedAbilityScript.stun;
            }
        }
        else if (abilitySelected == true)
        {
            abilitySelected = false;
            if (equipped == true)
            {
                targetAbilityScript = player.transform.Find("ability" + abilityNum.ToString()).gameObject.GetComponent<baseAbilityScript>();

                selectedAbilityScript.type = targetAbilityScript.type;
                selectedAbilityScript.targeting = targetAbilityScript.targeting;
                selectedAbilityScript.baseDamage = targetAbilityScript.baseDamage;
                selectedAbilityScript.baseHealing = targetAbilityScript.baseHealing;
                selectedAbilityScript.baseRange = targetAbilityScript.baseRange;
                selectedAbilityScript.baseAoeRadius = targetAbilityScript.baseAoeRadius;
                selectedAbilityScript.baseCastTime = targetAbilityScript.baseCastTime;
                selectedAbilityScript.baseCooldown = targetAbilityScript.baseCooldown;
                selectedAbilityScript.appliesEffect = targetAbilityScript.appliesEffect;
                selectedAbilityScript.dotDamage = targetAbilityScript.dotDamage;
                selectedAbilityScript.hotHealing = targetAbilityScript.hotHealing;
                selectedAbilityScript.effectDuration = targetAbilityScript.effectDuration;
                selectedAbilityScript.stackingEffect = targetAbilityScript.stackingEffect;
                selectedAbilityScript.percentArmourMod = targetAbilityScript.percentArmourMod;
                selectedAbilityScript.percentPowerMod = targetAbilityScript.percentPowerMod;
                selectedAbilityScript.percentAttackSpeedMod = targetAbilityScript.percentAttackSpeedMod;
                selectedAbilityScript.percentMoveSpeedMod = targetAbilityScript.percentMoveSpeedMod;
                selectedAbilityScript.percentCdrMod = targetAbilityScript.percentCdrMod;
                selectedAbilityScript.percentRangeMod = targetAbilityScript.percentRangeMod;
                selectedAbilityScript.percentAoeMod = targetAbilityScript.percentAoeMod;
                selectedAbilityScript.percentProjSpeedMod = targetAbilityScript.percentProjSpeedMod;
                selectedAbilityScript.percentDurationMod = targetAbilityScript.percentDurationMod;
                selectedAbilityScript.flatArmourMod = targetAbilityScript.flatArmourMod;
                selectedAbilityScript.flatPowerMod = targetAbilityScript.flatPowerMod;
                selectedAbilityScript.flatAttackSpeedMod = targetAbilityScript.flatAttackSpeedMod;
                selectedAbilityScript.flatMoveSpeedMod = targetAbilityScript.flatMoveSpeedMod;
                selectedAbilityScript.flatCdrMod = targetAbilityScript.flatCdrMod;
                selectedAbilityScript.flatRangeMod = targetAbilityScript.flatRangeMod;
                selectedAbilityScript.flatAoeMod = targetAbilityScript.flatAoeMod;
                selectedAbilityScript.flatProjSpeedMod = targetAbilityScript.flatProjSpeedMod;
                selectedAbilityScript.flatDurationMod = targetAbilityScript.flatDurationMod;
                selectedAbilityScript.projectileSpeed = targetAbilityScript.projectileSpeed;
                selectedAbilityScript.piercing = targetAbilityScript.piercing;
                selectedAbilityScript.projectileSize = targetAbilityScript.projectileSize;
                selectedAbilityScript.aoeDuration = targetAbilityScript.aoeDuration;
                selectedAbilityScript.offensive = targetAbilityScript.offensive;
                selectedAbilityScript.stun = targetAbilityScript.stun;

                targetAbilityScript.type = tempAbilityScript.type;
                targetAbilityScript.targeting = tempAbilityScript.targeting;
                targetAbilityScript.baseDamage = tempAbilityScript.baseDamage;
                targetAbilityScript.baseHealing = tempAbilityScript.baseHealing;
                targetAbilityScript.baseRange = tempAbilityScript.baseRange;
                targetAbilityScript.baseAoeRadius = tempAbilityScript.baseAoeRadius;
                targetAbilityScript.baseCastTime = tempAbilityScript.baseCastTime;
                targetAbilityScript.baseCooldown = tempAbilityScript.baseCooldown;
                targetAbilityScript.appliesEffect = tempAbilityScript.appliesEffect;
                targetAbilityScript.dotDamage = tempAbilityScript.dotDamage;
                targetAbilityScript.hotHealing = tempAbilityScript.hotHealing;
                targetAbilityScript.effectDuration = tempAbilityScript.effectDuration;
                targetAbilityScript.stackingEffect = tempAbilityScript.stackingEffect;
                targetAbilityScript.percentArmourMod = tempAbilityScript.percentArmourMod;
                targetAbilityScript.percentPowerMod = tempAbilityScript.percentPowerMod;
                targetAbilityScript.percentAttackSpeedMod = tempAbilityScript.percentAttackSpeedMod;
                targetAbilityScript.percentMoveSpeedMod = tempAbilityScript.percentMoveSpeedMod;
                targetAbilityScript.percentCdrMod = tempAbilityScript.percentCdrMod;
                targetAbilityScript.percentRangeMod = tempAbilityScript.percentRangeMod;
                targetAbilityScript.percentAoeMod = tempAbilityScript.percentAoeMod;
                targetAbilityScript.percentProjSpeedMod = tempAbilityScript.percentProjSpeedMod;
                targetAbilityScript.percentDurationMod = tempAbilityScript.percentDurationMod;
                targetAbilityScript.flatArmourMod = tempAbilityScript.flatArmourMod;
                targetAbilityScript.flatPowerMod = tempAbilityScript.flatPowerMod;
                targetAbilityScript.flatAttackSpeedMod = tempAbilityScript.flatAttackSpeedMod;
                targetAbilityScript.flatMoveSpeedMod = tempAbilityScript.flatMoveSpeedMod;
                targetAbilityScript.flatCdrMod = tempAbilityScript.flatCdrMod;
                targetAbilityScript.flatRangeMod = tempAbilityScript.flatRangeMod;
                targetAbilityScript.flatAoeMod = tempAbilityScript.flatAoeMod;
                targetAbilityScript.flatProjSpeedMod = tempAbilityScript.flatProjSpeedMod;
                targetAbilityScript.flatDurationMod = tempAbilityScript.flatDurationMod;
                targetAbilityScript.projectileSpeed = tempAbilityScript.projectileSpeed;
                targetAbilityScript.piercing = tempAbilityScript.piercing;
                targetAbilityScript.projectileSize = tempAbilityScript.projectileSize;
                targetAbilityScript.aoeDuration = tempAbilityScript.aoeDuration;
                targetAbilityScript.offensive = tempAbilityScript.offensive;
                targetAbilityScript.stun = tempAbilityScript.stun;
            }
            else if (equipped == false)
            {
                targetAbilityScript = player.transform.Find("inventory").transform.Find("inventorySlot" + abilityNum.ToString()).gameObject.GetComponent<baseAbilityScript>();

                selectedAbilityScript.type = targetAbilityScript.type;
                selectedAbilityScript.targeting = targetAbilityScript.targeting;
                selectedAbilityScript.baseDamage = targetAbilityScript.baseDamage;
                selectedAbilityScript.baseHealing = targetAbilityScript.baseHealing;
                selectedAbilityScript.baseRange = targetAbilityScript.baseRange;
                selectedAbilityScript.baseAoeRadius = targetAbilityScript.baseAoeRadius;
                selectedAbilityScript.baseCastTime = targetAbilityScript.baseCastTime;
                selectedAbilityScript.baseCooldown = targetAbilityScript.baseCooldown;
                selectedAbilityScript.appliesEffect = targetAbilityScript.appliesEffect;
                selectedAbilityScript.dotDamage = targetAbilityScript.dotDamage;
                selectedAbilityScript.hotHealing = targetAbilityScript.hotHealing;
                selectedAbilityScript.effectDuration = targetAbilityScript.effectDuration;
                selectedAbilityScript.stackingEffect = targetAbilityScript.stackingEffect;
                selectedAbilityScript.percentArmourMod = targetAbilityScript.percentArmourMod;
                selectedAbilityScript.percentPowerMod = targetAbilityScript.percentPowerMod;
                selectedAbilityScript.percentAttackSpeedMod = targetAbilityScript.percentAttackSpeedMod;
                selectedAbilityScript.percentMoveSpeedMod = targetAbilityScript.percentMoveSpeedMod;
                selectedAbilityScript.percentCdrMod = targetAbilityScript.percentCdrMod;
                selectedAbilityScript.percentRangeMod = targetAbilityScript.percentRangeMod;
                selectedAbilityScript.percentAoeMod = targetAbilityScript.percentAoeMod;
                selectedAbilityScript.percentProjSpeedMod = targetAbilityScript.percentProjSpeedMod;
                selectedAbilityScript.percentDurationMod = targetAbilityScript.percentDurationMod;
                selectedAbilityScript.flatArmourMod = targetAbilityScript.flatArmourMod;
                selectedAbilityScript.flatPowerMod = targetAbilityScript.flatPowerMod;
                selectedAbilityScript.flatAttackSpeedMod = targetAbilityScript.flatAttackSpeedMod;
                selectedAbilityScript.flatMoveSpeedMod = targetAbilityScript.flatMoveSpeedMod;
                selectedAbilityScript.flatCdrMod = targetAbilityScript.flatCdrMod;
                selectedAbilityScript.flatRangeMod = targetAbilityScript.flatRangeMod;
                selectedAbilityScript.flatAoeMod = targetAbilityScript.flatAoeMod;
                selectedAbilityScript.flatProjSpeedMod = targetAbilityScript.flatProjSpeedMod;
                selectedAbilityScript.flatDurationMod = targetAbilityScript.flatDurationMod;
                selectedAbilityScript.projectileSpeed = targetAbilityScript.projectileSpeed;
                selectedAbilityScript.piercing = targetAbilityScript.piercing;
                selectedAbilityScript.projectileSize = targetAbilityScript.projectileSize;
                selectedAbilityScript.aoeDuration = targetAbilityScript.aoeDuration;
                selectedAbilityScript.offensive = targetAbilityScript.offensive;
                selectedAbilityScript.stun = targetAbilityScript.stun;

                targetAbilityScript.type = tempAbilityScript.type;
                targetAbilityScript.targeting = tempAbilityScript.targeting;
                targetAbilityScript.baseDamage = tempAbilityScript.baseDamage;
                targetAbilityScript.baseHealing = tempAbilityScript.baseHealing;
                targetAbilityScript.baseRange = tempAbilityScript.baseRange;
                targetAbilityScript.baseAoeRadius = tempAbilityScript.baseAoeRadius;
                targetAbilityScript.baseCastTime = tempAbilityScript.baseCastTime;
                targetAbilityScript.baseCooldown = tempAbilityScript.baseCooldown;
                targetAbilityScript.appliesEffect = tempAbilityScript.appliesEffect;
                targetAbilityScript.dotDamage = tempAbilityScript.dotDamage;
                targetAbilityScript.hotHealing = tempAbilityScript.hotHealing;
                targetAbilityScript.effectDuration = tempAbilityScript.effectDuration;
                targetAbilityScript.stackingEffect = tempAbilityScript.stackingEffect;
                targetAbilityScript.percentArmourMod = tempAbilityScript.percentArmourMod;
                targetAbilityScript.percentPowerMod = tempAbilityScript.percentPowerMod;
                targetAbilityScript.percentAttackSpeedMod = tempAbilityScript.percentAttackSpeedMod;
                targetAbilityScript.percentMoveSpeedMod = tempAbilityScript.percentMoveSpeedMod;
                targetAbilityScript.percentCdrMod = tempAbilityScript.percentCdrMod;
                targetAbilityScript.percentRangeMod = tempAbilityScript.percentRangeMod;
                targetAbilityScript.percentAoeMod = tempAbilityScript.percentAoeMod;
                targetAbilityScript.percentProjSpeedMod = tempAbilityScript.percentProjSpeedMod;
                targetAbilityScript.percentDurationMod = tempAbilityScript.percentDurationMod;
                targetAbilityScript.flatArmourMod = tempAbilityScript.flatArmourMod;
                targetAbilityScript.flatPowerMod = tempAbilityScript.flatPowerMod;
                targetAbilityScript.flatAttackSpeedMod = tempAbilityScript.flatAttackSpeedMod;
                targetAbilityScript.flatMoveSpeedMod = tempAbilityScript.flatMoveSpeedMod;
                targetAbilityScript.flatCdrMod = tempAbilityScript.flatCdrMod;
                targetAbilityScript.flatRangeMod = tempAbilityScript.flatRangeMod;
                targetAbilityScript.flatAoeMod = tempAbilityScript.flatAoeMod;
                targetAbilityScript.flatProjSpeedMod = tempAbilityScript.flatProjSpeedMod;
                targetAbilityScript.flatDurationMod = tempAbilityScript.flatDurationMod;
                targetAbilityScript.projectileSpeed = tempAbilityScript.projectileSpeed;
                targetAbilityScript.piercing = tempAbilityScript.piercing;
                targetAbilityScript.projectileSize = tempAbilityScript.projectileSize;
                targetAbilityScript.aoeDuration = tempAbilityScript.aoeDuration;
                targetAbilityScript.offensive = tempAbilityScript.offensive;
                targetAbilityScript.stun = tempAbilityScript.stun;
            }
            resetTempAbility();
            fillInventoryUI();
        }
    }

    public void resetTempAbility()
    {
        tempAbilityScript.type = null;
        tempAbilityScript.targeting = null;
        tempAbilityScript.baseDamage = 0;
        tempAbilityScript.baseHealing = 0;
        tempAbilityScript.baseRange = 0;
        tempAbilityScript.baseAoeRadius = 0;
        tempAbilityScript.baseCastTime = 0;
        tempAbilityScript.baseCooldown = 0;
        tempAbilityScript.appliesEffect = false;
        tempAbilityScript.dotDamage = 0;
        tempAbilityScript.hotHealing = 0;
        tempAbilityScript.effectDuration = 0;
        tempAbilityScript.stackingEffect = false;
        tempAbilityScript.percentArmourMod = 0;
        tempAbilityScript.percentPowerMod = 0;
        tempAbilityScript.percentAttackSpeedMod = 0;
        tempAbilityScript.percentMoveSpeedMod = 0;
        tempAbilityScript.percentCdrMod = 0;
        tempAbilityScript.percentRangeMod = 0;
        tempAbilityScript.percentAoeMod = 0;
        tempAbilityScript.percentProjSpeedMod = 0;
        tempAbilityScript.percentDurationMod = 0;
        tempAbilityScript.flatArmourMod = 0;
        tempAbilityScript.flatPowerMod = 0;
        tempAbilityScript.flatAttackSpeedMod = 0;
        tempAbilityScript.flatMoveSpeedMod = 0;
        tempAbilityScript.flatCdrMod = 0;
        tempAbilityScript.flatRangeMod = 0;
        tempAbilityScript.flatAoeMod = 0;
        tempAbilityScript.flatProjSpeedMod = 0;
        tempAbilityScript.flatDurationMod = 0;
        tempAbilityScript.projectileSpeed = 0;
        tempAbilityScript.piercing = false;
        tempAbilityScript.projectileSize = 0;
        tempAbilityScript.aoeDuration = 0;
        tempAbilityScript.offensive = true;
        tempAbilityScript.stun = false;

        selectedAbilityScript = null;
        selectedAbilityScriptInv = null;
        targetAbilityScript = null;
        targetAbilityScriptInv = null;
    }

    public void showBinToolTip()
    {
        toolTipActive = true;
        toolTip.SetActive(true);
        toolTipTransform.sizeDelta = new Vector2(toolTipTransform.sizeDelta.x, 200);
        toolTipTitle.text = "Bin";
        toolTipText.text = "Left click to drop the selected ability in the bin, destroying it but granting 5XP.";
    }

    public void hideBinToolTip()
    {
        toolTipTransform.sizeDelta = new Vector2(toolTipTransform.sizeDelta.x, 95);
        toolTipActive = false;
        toolTip.SetActive(false);
    }

    public void binAbility()
    {
        if (selectedAbilityScript != null)
        {
            selectedAbilityScript.type = null;
            selectedAbilityScript.targeting = null;
            selectedAbilityScript.baseDamage = 0;
            selectedAbilityScript.baseHealing = 0;
            selectedAbilityScript.baseRange = 0;
            selectedAbilityScript.baseAoeRadius = 0;
            selectedAbilityScript.baseCastTime = 0;
            selectedAbilityScript.baseCooldown = 0;
            selectedAbilityScript.appliesEffect = false;
            selectedAbilityScript.dotDamage = 0;
            selectedAbilityScript.hotHealing = 0;
            selectedAbilityScript.effectDuration = 0;
            selectedAbilityScript.stackingEffect = false;
            selectedAbilityScript.percentArmourMod = 0;
            selectedAbilityScript.percentPowerMod = 0;
            selectedAbilityScript.percentAttackSpeedMod = 0;
            selectedAbilityScript.percentMoveSpeedMod = 0;
            selectedAbilityScript.percentCdrMod = 0;
            selectedAbilityScript.percentRangeMod = 0;
            selectedAbilityScript.percentAoeMod = 0;
            selectedAbilityScript.percentProjSpeedMod = 0;
            selectedAbilityScript.percentDurationMod = 0;
            selectedAbilityScript.flatArmourMod = 0;
            selectedAbilityScript.flatPowerMod = 0;
            selectedAbilityScript.flatAttackSpeedMod = 0;
            selectedAbilityScript.flatMoveSpeedMod = 0;
            selectedAbilityScript.flatCdrMod = 0;
            selectedAbilityScript.flatRangeMod = 0;
            selectedAbilityScript.flatAoeMod = 0;
            selectedAbilityScript.flatProjSpeedMod = 0;
            selectedAbilityScript.flatDurationMod = 0;
            selectedAbilityScript.projectileSpeed = 0;
            selectedAbilityScript.piercing = false;
            selectedAbilityScript.projectileSize = 0;
            selectedAbilityScript.aoeDuration = 0;
            selectedAbilityScript.offensive = true;
            selectedAbilityScript.stun = false;

            tempAbilityScript.type = null;
            tempAbilityScript.targeting = null;
            tempAbilityScript.baseDamage = 0;
            tempAbilityScript.baseHealing = 0;
            tempAbilityScript.baseRange = 0;
            tempAbilityScript.baseAoeRadius = 0;
            tempAbilityScript.baseCastTime = 0;
            tempAbilityScript.baseCooldown = 0;
            tempAbilityScript.appliesEffect = false;
            tempAbilityScript.dotDamage = 0;
            tempAbilityScript.hotHealing = 0;
            tempAbilityScript.effectDuration = 0;
            tempAbilityScript.stackingEffect = false;
            tempAbilityScript.percentArmourMod = 0;
            tempAbilityScript.percentPowerMod = 0;
            tempAbilityScript.percentAttackSpeedMod = 0;
            tempAbilityScript.percentMoveSpeedMod = 0;
            tempAbilityScript.percentCdrMod = 0;
            tempAbilityScript.percentRangeMod = 0;
            tempAbilityScript.percentAoeMod = 0;
            tempAbilityScript.percentProjSpeedMod = 0;
            tempAbilityScript.percentDurationMod = 0;
            tempAbilityScript.flatArmourMod = 0;
            tempAbilityScript.flatPowerMod = 0;
            tempAbilityScript.flatAttackSpeedMod = 0;
            tempAbilityScript.flatMoveSpeedMod = 0;
            tempAbilityScript.flatCdrMod = 0;
            tempAbilityScript.flatRangeMod = 0;
            tempAbilityScript.flatAoeMod = 0;
            tempAbilityScript.flatProjSpeedMod = 0;
            tempAbilityScript.flatDurationMod = 0;
            tempAbilityScript.projectileSpeed = 0;
            tempAbilityScript.piercing = false;
            tempAbilityScript.projectileSize = 0;
            tempAbilityScript.aoeDuration = 0;
            tempAbilityScript.offensive = true;
            tempAbilityScript.stun = false;

            selectedAbilityScript = null;
            selectedAbilityScriptInv = null;
            abilitySelected = false;
            playerScript.gainXP(5);

            fillInventoryUI();
        }
    }

    public void storySceneDone()
    {
        dialogueDisplay.SetActive(false);
        storyPlaying = false;
    }

    public void showIntroDialogue()
    {
        dialogueDisplay.SetActive(true);
        dialogueText.text = introText;
        dialogueSpeakerName.text = "O.V";
        dialogueAudioSource.clip = introAudio;
        dialogueAudioSource.Play();
        storyPlaying = true;
    }

    public void showLevelCompleteDialogue()
    {
        dialogueDisplay.SetActive(true);
        dialogueSpeakerName.text = "O.V";
        int levelCompleteInt = Random.Range(1, 6);
        if(levelCompleteInt==1)
        {
            dialogueText.text = levelCompleteText1;
            dialogueAudioSource.clip = levelCompleteAudio1;
        }
        else if(levelCompleteInt == 2)
        {
            dialogueText.text = levelCompleteText2;
            dialogueAudioSource.clip = levelCompleteAudio2;
        }
        else if (levelCompleteInt == 3)
        {
            dialogueText.text = levelCompleteText3;
            dialogueAudioSource.clip = levelCompleteAudio3;
        }
        else if (levelCompleteInt == 4)
        {
            dialogueText.text = levelCompleteText4;
            dialogueAudioSource.clip = levelCompleteAudio4;
        }
        else if (levelCompleteInt == 5)
        {
            dialogueText.text = levelCompleteText5;
            dialogueAudioSource.clip = levelCompleteAudio5;
        }

        dialogueAudioSource.Play();
        storyPlaying = true;

        int hackChance = Random.Range(0, 2);

        if (hackChance > 0)
        {
            StartCoroutine("hackDelay");
        }
    }

    IEnumerator hackDelay()
    {
        yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));

        if (nextHack == 0)
        {
            showHack0Dialogue();
        }
        else if (nextHack == 1)
        {
            showHack1Dialogue();
        }
        else if (nextHack == 2)
        {
            showHack2Dialogue();
        }
        else if (nextHack == 3)
        {
            showHack3Dialogue();
        }
        else if (nextHack == 4)
        {
            showHack4Dialogue();
        }
        else if (nextHack == 5)
        {
            showHack5Dialogue();
        }

        nextHack++;
    }

    public void showHack0Dialogue()
    {
        dialogueDisplay.SetActive(true);
        dialogueText.text = hack0Text;
        dialogueSpeakerName.text = "Hacker";
        dialogueAudioSource.clip = hack0Audio;
        dialogueAudioSource.Play();
        storyPlaying = true;
    }

    public void showHack1Dialogue()
    {
        dialogueDisplay.SetActive(true);
        dialogueText.text = hack1Text;
        dialogueSpeakerName.text = "Hacker";
        dialogueAudioSource.clip = hack1Audio;
        dialogueAudioSource.Play();
        storyPlaying = true;
    }

    public void showHack2Dialogue()
    {
        dialogueDisplay.SetActive(true);
        dialogueText.text = hack2Text;
        dialogueSpeakerName.text = "Hacker";
        dialogueAudioSource.clip = hack2Audio;
        dialogueAudioSource.Play();
        storyPlaying = true;
    }

    public void showHack3Dialogue()
    {
        dialogueDisplay.SetActive(true);
        dialogueText.text = hack3Text;
        dialogueSpeakerName.text = "Hacker";
        dialogueAudioSource.clip = hack3Audio;
        dialogueAudioSource.Play();
        storyPlaying = true;
    }

    public void showHack4Dialogue()
    {
        dialogueDisplay.SetActive(true);
        dialogueText.text = hack4Text;
        dialogueSpeakerName.text = "Hacker";
        dialogueAudioSource.clip = hack4Audio;
        dialogueAudioSource.Play();
        storyPlaying = true;
    }

    public void showHack5Dialogue()
    {
        dialogueDisplay.SetActive(true);
        finalBossButton.SetActive(true);
        dialogueText.text = hack5Text;
        dialogueSpeakerName.text = "Hacker";
        dialogueAudioSource.clip = hack5Audio;
        dialogueAudioSource.Play();
        storyPlaying = true;
    }

    public void showFinalBossDialogue()
    {
        dialogueDisplay.SetActive(true);
        dialogueText.text = finalBossText;
        dialogueSpeakerName.text = "Overlord";
        dialogueAudioSource.clip = finalBossAudio;
        dialogueAudioSource.Play();
        storyPlaying = true;
    }

    public void showFinalBossDefeatedDialogue()
    {
        dialogueDisplay.SetActive(true);
        dialogueText.text = finalBossDefeatedText;
        dialogueSpeakerName.text = "Hacker";
        dialogueAudioSource.clip = finalBossDefeatedAudio;
        dialogueAudioSource.Play();
        storyPlaying = true;
    }

    IEnumerator showNotLeavingDialogue()
    {
        yield return new WaitForSeconds(30);
        dialogueDisplay.SetActive(true);
        dialogueText.text = notLeavingText;
        dialogueSpeakerName.text = "Hacker";
        dialogueAudioSource.clip = notLeavingAudio;
        dialogueAudioSource.Play();
        storyPlaying = true;
    }

    public void launchFinalLevel()
    {
        dialogueAudioSource.clip = launchFinalLevelAudio;
        dialogueAudioSource.Play();
        player.transform.Find("musicPlayer(Clone)").GetComponent<AudioSource>().pitch = 0.5f;
        player.transform.Find("musicPlayer(Clone)").GetComponent<AudioSource>().volume = 1;
        finalBossButton.SetActive(false);
        gameManager.launchFinalLevel();
    }

    public void skipLevel()
    {
        gameManager.skipLevel();
    }
}