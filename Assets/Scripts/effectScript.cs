using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effectScript : MonoBehaviour
{
    public float duration;

    public float percentArmourMod;
    public float percentPowerMod;
    public float percentAttackSpeedMod;
    public float percentMoveSpeedMod;
    public float percentCdrMod;
    public float percentRangeMod;
    public float percentAoeMod;
    public float percentProjSpeedMod;
    public float percentDurationMod;

    public float armourModApplied;
    public float powerModApplied;
    public float attackSpeedModApplied;
    public float moveSpeedModApplied;
    public float cdrModApplied;
    public float rangeModApplied;
    public float aoeModApplied;
    public float projSpeedModApplied;
    public float durationModApplied;

    public float flatArmourMod;
    public float flatPowerMod;
    public float flatAttackSpeedMod;
    public float flatMoveSpeedMod;
    public float flatCdrMod;
    public float flatRangeMod;
    public float flatAoeMod;
    public float flatProjSpeedMod;
    public float flatDurationMod;

    public bool flatArmourModApplied;
    public bool flatPowerModApplied;
    public bool flatAttackSpeedModApplied;
    public bool flatMoveSpeedModApplied;
    public bool flatCdrModApplied;
    public bool flatRangeModApplied;
    public bool flatAoeModApplied;
    public bool flatProjSpeedModApplied;
    public bool flatDurationModApplied;

    public float damage;
    public float healing;
    public bool stun;

    public baseAbilityScript abilityAppliedBy;
    public baseCharacter charAppliedBy;
    public baseCharacter charAppliedTo;

    public void readyToApply()
    {
        charAppliedTo = transform.parent.gameObject.GetComponent<baseCharacter>();
        if (tag!=charAppliedBy.tag)
        {
            tag = charAppliedBy.tag;
        }
        percentArmourMod = abilityAppliedBy.percentArmourMod * (1+(charAppliedBy.power/100));
        percentPowerMod = abilityAppliedBy.percentPowerMod * (1 + (charAppliedBy.power / 100));
        percentAttackSpeedMod = abilityAppliedBy.percentAttackSpeedMod * (1 + (charAppliedBy.power / 100));
        percentMoveSpeedMod = abilityAppliedBy.percentMoveSpeedMod * (1 + (charAppliedBy.power / 100));
        percentCdrMod = abilityAppliedBy.percentCdrMod * (1 + (charAppliedBy.power / 100));
        percentRangeMod = abilityAppliedBy.percentRangeMod * (1 + (charAppliedBy.power / 100));
        percentAoeMod = abilityAppliedBy.percentAoeMod * (1 + (charAppliedBy.power / 100));
        percentProjSpeedMod = abilityAppliedBy.percentProjSpeedMod * (1 + (charAppliedBy.power / 100));
        percentAoeMod = abilityAppliedBy.percentAoeMod * (1 + (charAppliedBy.power / 100));

        flatArmourMod = abilityAppliedBy.flatArmourMod * (1 + (charAppliedBy.power / 100));
        flatPowerMod = abilityAppliedBy.flatPowerMod * (1 + (charAppliedBy.power / 100));
        flatAttackSpeedMod = abilityAppliedBy.flatAttackSpeedMod * (1 + (charAppliedBy.power / 100));
        flatMoveSpeedMod = abilityAppliedBy.flatMoveSpeedMod * (1 + (charAppliedBy.power / 100));
        flatCdrMod = abilityAppliedBy.flatCdrMod * (1 + (charAppliedBy.power / 100));
        flatRangeMod = abilityAppliedBy.flatRangeMod * (1 + (charAppliedBy.power / 100));
        flatAoeMod = abilityAppliedBy.flatAoeMod * (1 + (charAppliedBy.power / 100));
        flatProjSpeedMod = abilityAppliedBy.flatProjSpeedMod * (1 + (charAppliedBy.power / 100));
        flatDurationMod = abilityAppliedBy.flatDurationMod * (1 + (charAppliedBy.power / 100));

        damage = abilityAppliedBy.dotDamage * (1 + (charAppliedBy.power / 100));
        healing= abilityAppliedBy.hotHealing * (1 + (charAppliedBy.power / 100));
        stun = abilityAppliedBy.stun;

        modStats();

        if (damage > 0 && charAppliedTo.tag!=tag)
        {
            StartCoroutine("DoT");
        }

        if (healing > 0 && charAppliedTo.tag==tag)
        {
            StartCoroutine("HoT");
        }

        StartCoroutine("Duration");
    }

    IEnumerator DoT()
    {
        charAppliedTo.takeDamage(damage);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine("DoT");
    }

    IEnumerator HoT()
    {
        charAppliedTo.takeHealing(healing);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine("HoT");
    }

    public void modStats()
    {
        if(stun==true && charAppliedTo.tag!=tag)
        {
            charAppliedTo.interruptCast();
            charAppliedTo.stunned = true;
            charAppliedTo.stuns++;
            charAppliedTo.spawnCombatText((duration), "Stun");
        }
        /*
        if (percentArmourMod != 0)
        {
            armourModApplied = charAppliedTo.armour * (percentArmourMod/100);
            charAppliedTo.armour = charAppliedTo.armour + (charAppliedTo.armour * (percentArmourMod / 100));
            charAppliedTo.spawnCombatText((charAppliedTo.armour * (percentArmourMod / 100)), "Armour");
        }

        if (percentPowerMod != 0)
        {
            powerModApplied = charAppliedTo.power * (percentPowerMod/100);
            charAppliedTo.power = charAppliedTo.power + (charAppliedTo.power * (percentPowerMod / 100));
            charAppliedTo.spawnCombatText((charAppliedTo.power * (percentPowerMod / 100)), "Power");
        }

        if (percentAttackSpeedMod != 0)
        {
            attackSpeedModApplied = charAppliedTo.attackSpeed * (percentAttackSpeedMod / 100);
            charAppliedTo.attackSpeed = charAppliedTo.attackSpeed + (charAppliedTo.attackSpeed * (percentAttackSpeedMod / 100));
            charAppliedTo.spawnCombatText((charAppliedTo.attackSpeed * (percentAttackSpeedMod / 100)), "Attack Speed");
        }

        if (percentMoveSpeedMod != 0)
        {
            moveSpeedModApplied = charAppliedTo.moveSpeed * (percentMoveSpeedMod / 100);
            charAppliedTo.moveSpeed = charAppliedTo.moveSpeed + (charAppliedTo.moveSpeed * (percentMoveSpeedMod / 100));
            charAppliedTo.spawnCombatText((charAppliedTo.moveSpeed * (percentMoveSpeedMod / 100)), "Move Speed");
        }

        if (percentCdrMod != 0)
        {
            cdrModApplied = charAppliedTo.cooldownReduction * (percentCdrMod / 100);
            charAppliedTo.cooldownReduction = charAppliedTo.cooldownReduction + (charAppliedTo.cooldownReduction * (percentCdrMod / 100));
            charAppliedTo.spawnCombatText((charAppliedTo.cooldownReduction * (percentCdrMod / 100)), "CDR");
        }

        if (percentRangeMod != 0)
        {
            rangeModApplied = charAppliedTo.bonusRange * (percentRangeMod / 100);
            charAppliedTo.bonusRange = charAppliedTo.bonusRange + (charAppliedTo.bonusRange * (percentRangeMod / 100));
            charAppliedTo.spawnCombatText((charAppliedTo.bonusRange * (percentRangeMod / 100)), "Range");
        }

        if (percentAoeMod != 0)
        {
            aoeModApplied = charAppliedTo.bonusArea * (percentAoeMod / 100);
            charAppliedTo.bonusArea = charAppliedTo.bonusArea + (charAppliedTo.bonusArea * (percentAoeMod / 100));
            charAppliedTo.spawnCombatText((charAppliedTo.bonusArea * (percentAoeMod / 100)), "AoE");
        }

        if (percentProjSpeedMod != 0)
        {
            projSpeedModApplied = charAppliedTo.bonusProjectileSpeed * (percentProjSpeedMod / 100);
            charAppliedTo.bonusProjectileSpeed = charAppliedTo.bonusProjectileSpeed + (charAppliedTo.bonusProjectileSpeed * (percentProjSpeedMod / 100));
            charAppliedTo.spawnCombatText((charAppliedTo.bonusProjectileSpeed * (percentProjSpeedMod / 100)), "Projectile Speed");
        }

        if (percentDurationMod != 0)
        {
            durationModApplied = charAppliedTo.bonusDuration * (percentDurationMod / 100);
            charAppliedTo.bonusDuration = charAppliedTo.bonusDuration + (charAppliedTo.bonusDuration * (percentDurationMod / 100));
            charAppliedTo.spawnCombatText((charAppliedTo.bonusDuration * (percentDurationMod / 100)), "Duration");
        }
        */
        if ((flatArmourMod < 0 && charAppliedTo.tag != tag) || (flatArmourMod > 0 && charAppliedTo.tag == tag))
        {
            charAppliedTo.armour = charAppliedTo.armour + flatArmourMod;
            charAppliedTo.spawnCombatText(flatArmourMod, "Armour");
            flatArmourModApplied = true;
        }

        if ((flatPowerMod < 0 && charAppliedTo.tag != tag) || (flatPowerMod > 0 && charAppliedTo.tag == tag))
        {
            charAppliedTo.power = charAppliedTo.power + flatPowerMod;
            charAppliedTo.spawnCombatText(flatPowerMod, "Power");
            flatPowerModApplied = true;
        }

        if ((flatAttackSpeedMod < 0 && charAppliedTo.tag != tag) || (flatAttackSpeedMod > 0 && charAppliedTo.tag == tag))
        {
            charAppliedTo.attackSpeed = charAppliedTo.attackSpeed + flatAttackSpeedMod;
            charAppliedTo.spawnCombatText(flatAttackSpeedMod, "Attack Speed");
            flatAttackSpeedModApplied = true;
        }

        if ((flatMoveSpeedMod < 0 && charAppliedTo.tag != tag) || (flatMoveSpeedMod > 0 && charAppliedTo.tag == tag))
        {
            charAppliedTo.moveSpeed = charAppliedTo.moveSpeed + flatMoveSpeedMod;
            charAppliedTo.spawnCombatText(flatMoveSpeedMod, "Move Speed");
            flatMoveSpeedModApplied = true;
        }

        if ((flatCdrMod < 0 && charAppliedTo.tag != tag) || (flatCdrMod > 0 && charAppliedTo.tag == tag))
        {
            charAppliedTo.cooldownReduction = charAppliedTo.cooldownReduction + flatCdrMod;
            charAppliedTo.spawnCombatText(flatCdrMod, "CDR");
            flatCdrModApplied = true;
        }

        if ((flatRangeMod < 0 && charAppliedTo.tag != tag) || (flatRangeMod > 0 && charAppliedTo.tag == tag))
        {
            charAppliedTo.bonusRange = charAppliedTo.bonusRange + flatRangeMod;
            charAppliedTo.spawnCombatText(flatRangeMod, "Range");
            flatRangeModApplied = true;
        }

        if ((flatAoeMod < 0 && charAppliedTo.tag != tag) || (flatAoeMod > 0 && charAppliedTo.tag == tag))
        {
            charAppliedTo.bonusArea = charAppliedTo.bonusArea + flatAoeMod;
            charAppliedTo.spawnCombatText(flatAoeMod, "AoE");
            flatAoeModApplied = true;
        }

        if ((flatProjSpeedMod < 0 && charAppliedTo.tag != tag) || (flatProjSpeedMod > 0 && charAppliedTo.tag == tag))
        {
            charAppliedTo.bonusProjectileSpeed = charAppliedTo.bonusProjectileSpeed + flatProjSpeedMod;
            charAppliedTo.spawnCombatText(flatProjSpeedMod, "Projectile Speed");
            flatProjSpeedModApplied = true;
        }

        if ((flatDurationMod < 0 && charAppliedTo.tag != tag) || (flatDurationMod > 0 && charAppliedTo.tag == tag))
        {
            charAppliedTo.bonusDuration = charAppliedTo.bonusDuration + flatDurationMod;
            charAppliedTo.spawnCombatText(flatDurationMod, "Duration");
            flatDurationModApplied = true;
        }
    }

    public void removeEffect()
    {
        if(stun==true)
        {
            charAppliedTo.stuns--;
            if(charAppliedTo.stuns<1)
            {
                charAppliedTo.stunned = false;
            }
        }
        /*
        if (percentArmourMod != 0)
        {
            charAppliedTo.armour = charAppliedTo.armour - armourModApplied;
            charAppliedTo.spawnCombatText(armourModApplied * -1, "Armour");
        }

        if (percentPowerMod != 0)
        {
            charAppliedTo.power = charAppliedTo.power - powerModApplied;
            charAppliedTo.spawnCombatText(powerModApplied * -1, "Power");
        }

        if (percentAttackSpeedMod != 0)
        {
            charAppliedTo.attackSpeed = charAppliedTo.attackSpeed - attackSpeedModApplied;
            charAppliedTo.spawnCombatText(attackSpeedModApplied * -1, "Attack Speed");
        }

        if (percentMoveSpeedMod != 0)
        {
            charAppliedTo.moveSpeed = charAppliedTo.moveSpeed - moveSpeedModApplied;
            charAppliedTo.spawnCombatText(moveSpeedModApplied * -1, "Move Speed");
        }

        if (percentCdrMod != 0)
        {
            charAppliedTo.cooldownReduction = charAppliedTo.cooldownReduction - cdrModApplied;
            charAppliedTo.spawnCombatText(cdrModApplied * -1, "CDR");
        }

        if (percentRangeMod != 0)
        {
            charAppliedTo.bonusRange = charAppliedTo.bonusRange - rangeModApplied;
            charAppliedTo.spawnCombatText(rangeModApplied * -1, "Range");
        }

        if (percentAoeMod != 0)
        {
            charAppliedTo.bonusArea = charAppliedTo.bonusArea - aoeModApplied;
            charAppliedTo.spawnCombatText(aoeModApplied * -1, "AoE");
        }
        */
        if (flatArmourMod != 0 && flatArmourModApplied == true)
        {
            charAppliedTo.armour = charAppliedTo.armour - flatArmourMod;
            charAppliedTo.spawnCombatText(flatArmourMod * -1, "Armour");
        }

        if (flatPowerMod != 0 && flatPowerModApplied == true)
        {
            charAppliedTo.power = charAppliedTo.power - flatPowerMod;
            charAppliedTo.spawnCombatText(flatPowerMod * -1, "Power");
        }

        if (flatAttackSpeedMod != 0 && flatAttackSpeedModApplied == true)
        {
            charAppliedTo.attackSpeed = charAppliedTo.attackSpeed - flatAttackSpeedMod;
            charAppliedTo.spawnCombatText(flatAttackSpeedMod * -1, "Attack Speed");
        }

        if (flatMoveSpeedMod != 0 && flatMoveSpeedModApplied == true)
        {
            charAppliedTo.moveSpeed = charAppliedTo.moveSpeed - flatMoveSpeedMod;
            charAppliedTo.spawnCombatText(flatMoveSpeedMod * -1, "Move Speed");
        }

        if (flatCdrMod != 0 && flatCdrModApplied == true)
        {
            charAppliedTo.cooldownReduction = charAppliedTo.cooldownReduction - flatCdrMod;
            charAppliedTo.spawnCombatText(flatCdrMod * -1, "CDR");
        }

        if (flatRangeMod != 0 && flatRangeModApplied == true)
        {
            charAppliedTo.bonusRange = charAppliedTo.bonusRange - flatRangeMod;
            charAppliedTo.spawnCombatText(flatRangeMod * -1, "Range");
        }

        if (flatAoeMod != 0 && flatAoeModApplied == true)
        {
            charAppliedTo.bonusArea = charAppliedTo.bonusArea - flatAoeMod;
            charAppliedTo.spawnCombatText(flatAoeMod * -1, "AoE");
        }

        if (flatProjSpeedMod != 0 && flatProjSpeedModApplied == true)
        {
            charAppliedTo.bonusProjectileSpeed = charAppliedTo.bonusProjectileSpeed - flatProjSpeedMod;
            charAppliedTo.spawnCombatText(flatProjSpeedMod * -1, "Projectile Speed");
        }

        if (flatDurationMod != 0 && flatDurationModApplied == true)
        {
            charAppliedTo.bonusDuration = charAppliedTo.bonusDuration - flatDurationMod;
            charAppliedTo.spawnCombatText(flatDurationMod * -1, "Duration");
        }

        Destroy(this.gameObject);
    }

    IEnumerator Duration()
    {
        yield return new WaitForSeconds(duration);
        removeEffect();       
    }

    public void RefreshDuration()
    {
        StopCoroutine("Duration");
        StartCoroutine("Duration");
    }
}
