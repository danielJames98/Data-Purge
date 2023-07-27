using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class inventorySlotScript : MonoBehaviour
{
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
