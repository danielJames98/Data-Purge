using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lootScript : MonoBehaviour
{
    public Rigidbody rb;
    public AudioSource audioSource;
    public float upForce;
    public float sideForceMin;
    public float sideForceMax;
    public bool frozen;
    public GameObject canvas;
    public AudioClip pickUpSound;

    public string type = "";
    public string targeting = "";
    public float baseDamage = 0;
    public float baseHealing = 0;
    public float baseRange = 0;
    public float baseAoeRadius = 0;
    public float baseCastTime = 0;
    public float baseCooldown = 0;

    public bool appliesEffect = false;

    public float dotDamage = 0;
    public float hotHealing = 0;
    public float effectDuration = 0;
    public bool stackingEffect = false;

    public float percentArmourMod = 0;
    public float percentPowerMod = 0;
    public float percentAttackSpeedMod = 0;
    public float percentMoveSpeedMod = 0;
    public float percentCdrMod = 0;
    public float percentRangeMod = 0;
    public float percentAoeMod = 0;
    public float percentProjSpeedMod = 0;
    public float percentDurationMod = 0;

    public float flatArmourMod = 0;
    public float flatPowerMod = 0;
    public float flatAttackSpeedMod = 0;
    public float flatMoveSpeedMod = 0;
    public float flatCdrMod = 0;
    public float flatRangeMod = 0;
    public float flatAoeMod = 0;
    public float flatProjSpeedMod = 0;
    public float flatDurationMod = 0;

    public float projectileSpeed = 0;
    public bool piercing = false;
    public float projectileSize = 0;

    public float aoeDuration = 0;
    public bool offensive = true;
    public bool stun = false;

    void Start()
    {
        rb=GetComponent<Rigidbody>();
        audioSource=GetComponent<AudioSource>();
        rb.AddForce(Random.Range(sideForceMin,sideForceMax), upForce, Random.Range(sideForceMin, sideForceMax));
        canvas= Instantiate(Resources.Load("lootCanvas", typeof(GameObject)), this.transform.position, Quaternion.identity) as GameObject;
        canvas.GetComponent<overHeadCanvasScript>().parentCharacter = this.gameObject;
        canvas.GetComponent<overHeadCanvasScript>().yOffset = 1f;
    }

    void Update()
    {
        if(transform.position.y<=1f && frozen==false)
        {
            frozen=true;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            rb.constraints = RigidbodyConstraints.FreezePosition;
            rb.velocity = new Vector3(0,0,0);
        }
    }

    public void pickUp(playerController pc, baseAbilityScript slot)
    {
        AudioSource.PlayClipAtPoint(pickUpSound, this.transform.position, 1f);
        slot.type = type;
        slot.targeting = targeting;
        slot.baseDamage = baseDamage;
        slot.baseHealing = baseHealing;
        slot.baseRange = baseRange;
        slot.baseAoeRadius = baseAoeRadius;
        slot.baseCastTime = baseCastTime;
        slot.baseCooldown = baseCooldown;
        slot.appliesEffect = appliesEffect;
        slot.dotDamage = dotDamage;
        slot.hotHealing = hotHealing;
        slot.effectDuration = effectDuration;
        slot.stackingEffect = stackingEffect;
        slot.percentArmourMod = percentArmourMod;
        slot.percentPowerMod = percentPowerMod;
        slot.percentAttackSpeedMod = percentAttackSpeedMod;
        slot.percentMoveSpeedMod = percentMoveSpeedMod;
        slot.percentCdrMod = percentCdrMod;
        slot.percentRangeMod = percentRangeMod;
        slot.percentAoeMod = percentAoeMod;
        slot.percentProjSpeedMod = percentProjSpeedMod;
        slot.percentDurationMod = percentDurationMod;
        slot.flatArmourMod = flatArmourMod;
        slot.flatPowerMod = flatPowerMod;
        slot.flatAttackSpeedMod = flatAttackSpeedMod;
        slot.flatMoveSpeedMod = flatMoveSpeedMod;
        slot.flatCdrMod = flatCdrMod;
        slot.flatRangeMod = flatRangeMod;
        slot.flatAoeMod = flatAoeMod;
        slot.flatProjSpeedMod = flatProjSpeedMod;
        slot.flatDurationMod = flatDurationMod;
        slot.projectileSpeed = projectileSpeed;
        slot.piercing = piercing;
        slot.projectileSize = projectileSize;
        slot.aoeDuration = aoeDuration;
        slot.offensive = offensive;
        slot.stun = stun;
        pc.firstEmptyInventorySlot++;
        pc.ui.GetComponent<uiManagerScript>().fillInventoryUI();
        Destroy(canvas);
        Destroy(this.gameObject);
    }
}
