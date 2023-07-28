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

    public string type;
    public string targeting;
    public float baseDamage;
    public float baseHealing;
    public float baseRange;
    public float baseAoeRadius;
    public float baseCastTime;
    public float baseCooldown;
    public bool appliesEffect;
    public float dotDamage;
    public float hotHealing;
    public float effectDuration;
    public bool stackingEffect;
    public float percentArmourMod;
    public float percentPowerMod;
    public float percentAttackSpeedMod;
    public float percentMoveSpeedMod;
    public float percentCdrMod;
    public float percentRangeMod;
    public float percentAoeMod;
    public float percentProjSpeedMod;
    public float percentDurationMod;
    public float flatArmourMod;
    public float flatPowerMod;
    public float flatAttackSpeedMod;
    public float flatMoveSpeedMod;
    public float flatCdrMod;
    public float flatRangeMod;
    public float flatAoeMod;
    public float flatProjSpeedMod;
    public float flatDurationMod;
    public float projectileSpeed;
    public bool piercing;
    public float projectileSize;
    public float aoeDuration;
    public bool offensive;
    public bool stun;

    // Start is called before the first frame update
    void Start()
    {
        rb=GetComponent<Rigidbody>();
        audioSource=GetComponent<AudioSource>();
        rb.AddForce(Random.Range(sideForceMin,sideForceMax), upForce, Random.Range(sideForceMin, sideForceMax));
        canvas= Instantiate(Resources.Load("lootCanvas", typeof(GameObject)), this.transform.position, Quaternion.identity) as GameObject;
        canvas.GetComponent<overHeadCanvasScript>().parentCharacter = this.gameObject;
        canvas.GetComponent<overHeadCanvasScript>().yOffset = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y<=0.6f && frozen==false)
        {
            frozen=true;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            rb.constraints = RigidbodyConstraints.FreezePosition;
            rb.velocity = new Vector3(0,0,0);
        }
    }

    public void pickUp(playerController pc, inventorySlotScript slot)
    {
        AudioSource.PlayClipAtPoint(pickUpSound, this.transform.position);
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
