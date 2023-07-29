using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class inventorySlotScript : MonoBehaviour
{
    public string type="";
    public string targeting="";
    public float baseDamage=0;
    public float baseHealing=0;
    public float baseRange = 0;
    public float baseAoeRadius=0;
    public float baseCastTime = 0;
    public float baseCooldown = 0;

    public bool appliesEffect=false;

    public float dotDamage = 0;
    public float hotHealing = 0;
    public float effectDuration=0;
    public bool stackingEffect=false;

    public float percentArmourMod = 0;
    public float percentPowerMod = 0;
    public float percentAttackSpeedMod = 0;
    public float percentMoveSpeedMod = 0;
    public float percentCdrMod = 0;
    public float percentRangeMod = 0;
    public float percentAoeMod=0;
    public float percentProjSpeedMod=0;
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
    public bool piercing=false;
    public float projectileSize = 0;

    public float aoeDuration = 0;
    public bool offensive=true;
    public bool stun=false;    
}
