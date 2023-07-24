using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

public class baseAbilityScript : MonoBehaviour
{
    public string type;
    public string targeting;
    public float baseDamage;
    public float baseHealing;
    public float baseRange;
    public float baseAoeRadius;
    public float baseCastTime;
    public float baseCooldown;
    public Camera cam;

    public bool appliesEffect;

    public float dotDamage;
    public float hotHealing;
    public float effectDuration;
    public bool stackingEffect;

    public GameObject parentCharacter;
    public baseCharacter parentCharacterScript;
    public NavMeshAgent parentCharacterNav;

    public bool onCooldown;

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
    public GameObject frontFirePoint;
    public float projectileSize;

    public float aoeDuration;
    public bool offensive;
    public bool stun;

    void Start()
    {
        parentCharacter = transform.parent.gameObject;
        parentCharacterScript= parentCharacter.GetComponent<baseCharacter>();
        parentCharacterNav=parentCharacter.GetComponent<NavMeshAgent>();
        cam = GameObject.Find("Main Camera(Clone)").GetComponent<Camera>();
        frontFirePoint = parentCharacter.transform.Find("frontFirePoint").gameObject;
    }

    public void activateAbility()
    {
        if (onCooldown==false && parentCharacterScript.casting == false && parentCharacterScript.stunned == false) 
        {
            if (targeting == "pointAndClick")
            {
                pointAndClick();
            }
            else if (targeting == "direction")
            {
                direction();
            }
            else if(targeting == "ground")
            {
                groundTarget();
            }
            else if(targeting == "self")
            {
                selfTarget();
            }
        }
    }

    public void pointAndClick()
    {
        if (this.tag == "Player")
        {
            playerPointAndClick();
        }
        else if(this.tag=="Enemy")
        {
            enemyPointAndClick();
        }
    }

    public void playerPointAndClick()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (offensive==true && hit.transform.gameObject.tag == "Enemy")
            {
                if (Vector3.Distance(parentCharacter.transform.position, hit.transform.position) <= (baseRange * (1 + (parentCharacterScript.bonusRange / 100))))
                {
                    parentCharacterScript.targetCharacter = hit.transform.gameObject;
                    StartCoroutine("applyPointandClickEffect");
                }
                else if(Vector3.Distance(parentCharacter.transform.position, hit.transform.position) > (baseRange * (1 + (parentCharacterScript.bonusRange / 100))))
                {
                    parentCharacterScript.targetCharacter = hit.transform.gameObject;
                    moveIntoRange(hit.transform.gameObject);
                }
            }
            else if(offensive==false && hit.transform.gameObject.tag == "Player")
            {
                if (Vector3.Distance(parentCharacter.transform.position, hit.transform.position) <= (baseRange * (1 + (parentCharacterScript.bonusRange / 100))))
                {
                    parentCharacterScript.targetCharacter = hit.transform.gameObject;
                    StartCoroutine("applyPointandClickEffect");
                }
                else if(Vector3.Distance(parentCharacter.transform.position, hit.transform.position) > (baseRange * (1 + (parentCharacterScript.bonusRange / 100))))
                {
                    parentCharacterScript.targetCharacter = hit.transform.gameObject;
                    moveIntoRange(hit.transform.gameObject);
                }
            }
        }
    }

    public void enemyPointAndClick()
    {
        if (Vector3.Distance(parentCharacter.transform.position, parentCharacterScript.aggroTarget.transform.position) <= (baseRange * (1+(parentCharacterScript.bonusRange/100))))
        {
            parentCharacterScript.targetCharacter = parentCharacterScript.aggroTarget.transform.gameObject;
            StartCoroutine("applyPointandClickEffect");
        }
        else if(Vector3.Distance(parentCharacter.transform.position, parentCharacterScript.aggroTarget.transform.position) > (baseRange * (1 + (parentCharacterScript.bonusRange / 100))))
        {
            moveIntoRange(parentCharacterScript.aggroTarget.transform.gameObject);
        }
    }

    IEnumerator applyPointandClickEffect()
    {
        parentCharacterScript.castingCoroutine = "applyPointandClickEffect";
        parentCharacterScript.casting = true;
        parentCharacterScript.castingAbility = this;
        yield return new WaitForSeconds(baseCastTime / (1 + (parentCharacterScript.attackSpeed / 100)));
        parentCharacterScript.casting = false;
        GameObject targetCharacter;

        if (parentCharacter.tag=="Player")
        {
            targetCharacter = parentCharacterScript.targetCharacter;
        }
        else
        {
            targetCharacter = parentCharacterScript.aggroTarget.transform.gameObject;    
        }

        if (baseDamage>0)
        {
            targetCharacter.transform.gameObject.GetComponent<baseCharacter>().takeDamage(baseDamage*(1 + (parentCharacterScript.power / 100)));
        }

        if(appliesEffect)
        {
            createEffect(targetCharacter);
        }

        if(baseHealing>0)
        {
            targetCharacter.transform.gameObject.GetComponent<baseCharacter>().takeHealing(baseHealing * (1 + (parentCharacterScript.power / 100)));
        }

        if(targetCharacter!=parentCharacter)
        {
            parentCharacter.transform.forward = targetCharacter.transform.position - parentCharacter.transform.position;
        }
        
        parentCharacterScript.castingCoroutine = null;
        parentCharacterScript.casting = false;
        parentCharacterScript.castingAbility = null;
        StartCoroutine("cooldown");
    }

    public void createEffect(GameObject targetCharacter)
    {
        if (stackingEffect==true)
        {
            GameObject effect = Instantiate(Resources.Load("effect", typeof(GameObject))) as GameObject;
            effect.transform.parent = targetCharacter.transform;
            effectScript effectScriptRef = effect.GetComponent<effectScript>();
            effectScriptRef.duration = effectDuration*(1+(parentCharacterScript.bonusDuration/100));
            effectScriptRef.abilityAppliedBy = this;
            effectScriptRef.charAppliedBy = parentCharacterScript;
            effectScriptRef.readyToApply();
        }
        else
        {
            bool alreadyApplied=false;

            foreach(Transform child in targetCharacter.transform)
            {
                if(child.GetComponent<effectScript>()!= null)
                {
                    effectScript effectScript = child.GetComponent<effectScript>();

                    if (effectScript.abilityAppliedBy==this)
                    {
                        effectScript.RefreshDuration();
                        alreadyApplied=true;
                    }
                }
            }

            if(alreadyApplied==false)
            {
                GameObject effect = Instantiate(Resources.Load("effect", typeof(GameObject))) as GameObject;
                effect.transform.parent = targetCharacter.transform;
                effectScript effectScriptRef = effect.GetComponent<effectScript>();
                effectScriptRef.duration = effectDuration * (1 + (parentCharacterScript.bonusDuration / 100));
                effectScriptRef.abilityAppliedBy = this;
                effectScriptRef.charAppliedBy = parentCharacterScript;
                effectScriptRef.readyToApply();
            }
        }
    }

    public void moveIntoRange(GameObject targetCharacter)
    {
        parentCharacterScript.targetCharacter = targetCharacter;
        parentCharacterScript.castingAbility = this;
        parentCharacterScript.movingToRange= true;
        parentCharacterNav.destination = targetCharacter.transform.position;
    }

    public void moveIntoRangeOfPoint(Vector3 point)
    {
        parentCharacterNav.destination = point;
        parentCharacterScript.targetPosition = point;
        parentCharacterScript.castingAbility = this;
        parentCharacterScript.movingToRange = true;       
    }

    public void direction()
    {
        if (this.tag == "Player")
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, 1 << LayerMask.NameToLayer("Walkable")))
            {
                parentCharacterNav.destination = parentCharacter.transform.position;
                parentCharacter.transform.LookAt(hit.point);
                if(type=="projectile")
                {
                    StartCoroutine("spawnProjectile");
                }
                else if(type=="dash")
                {
                    StartCoroutine("aimDash");
                }
            }
        }
        else if (this.tag == "Enemy")
        {
            if (Vector3.Distance(parentCharacter.transform.position, parentCharacterScript.aggroTarget.transform.position) <= (baseRange * (1 + (parentCharacterScript.bonusRange / 100))))
            {
                parentCharacterNav.destination = parentCharacter.transform.position;
                parentCharacter.transform.LookAt(parentCharacterScript.aggroTarget.transform.position);
                if (type == "projectile")
                {
                    StartCoroutine("spawnProjectile");
                }
                else if (type == "dash")
                {
                    StartCoroutine("aimDash");
                }
            }
            else
            {
                moveIntoRange(parentCharacterScript.aggroTarget.transform.gameObject);
            }
        }
    }

    IEnumerator spawnProjectile()
    {
        parentCharacterScript.castingCoroutine = "spawnProjectile";
        parentCharacterScript.casting = true;
        parentCharacterScript.castingAbility = this;
        yield return new WaitForSeconds(baseCastTime / (1 + (parentCharacterScript.attackSpeed / 100)));
        parentCharacterScript.casting = false;
        GameObject projectile = Instantiate(Resources.Load("projectile", typeof(GameObject)), frontFirePoint.transform.position, frontFirePoint.transform.rotation) as GameObject;
        projectile.transform.localScale = new Vector3(projectileSize * (1 + (parentCharacterScript.bonusArea / 100)), projectileSize * (1 + (parentCharacterScript.bonusArea / 100)), projectileSize * (1 + (parentCharacterScript.bonusArea / 100)));
        projectileScript projectileScriptRef = projectile.GetComponent<projectileScript>();
        projectileScriptRef.range = baseRange * (1 + (parentCharacterScript.bonusRange / 100));
        projectileScriptRef.abilityAppliedBy = this;
        projectileScriptRef.charAppliedBy = parentCharacterScript;
        projectileScriptRef.speed = projectileSpeed * (1 + (parentCharacterScript.bonusProjectileSpeed / 100));
        projectileScriptRef.piercing = piercing;
        projectileScriptRef.damage = baseDamage * (1 + (parentCharacterScript.power / 100));
        projectileScriptRef.healing = baseHealing * (1 + (parentCharacterScript.power / 100));
        projectileScriptRef.offensive = offensive;
        projectileScriptRef.readyToFire();
        parentCharacterScript.castingCoroutine = null;
        parentCharacterScript.castingAbility = null;
        StartCoroutine("cooldown");
    }

    public void groundTarget()
    {
        if (this.tag == "Player")
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, 1 << LayerMask.NameToLayer("Walkable")))
            {
                if (Vector3.Distance(parentCharacter.transform.position, hit.point) <= (baseRange * (1 + (parentCharacterScript.bonusRange / 100))))
                {
                    parentCharacterScript.targetPosition= hit.point;
                    StartCoroutine("spawnAoe");
                }
                else
                {
                    moveIntoRangeOfPoint(hit.point);
                }
            }
        }
        else if (this.tag == "Enemy")
        {
            if (Vector3.Distance(parentCharacter.transform.position, parentCharacterScript.aggroTarget.transform.position) <= (baseRange * (1 + (parentCharacterScript.bonusRange / 100))))
            {
                parentCharacterScript.targetPosition = new Vector3(parentCharacterScript.aggroTarget.transform.position.x, 0.6f, parentCharacterScript.aggroTarget.transform.position.z);
                StartCoroutine("spawnAoe");
            }
            else
            {
                moveIntoRange(parentCharacterScript.aggroTarget.transform.gameObject);
            }
        }
    }

    IEnumerator spawnAoe()
    {
        Vector3 point;
        point = new Vector3(parentCharacterScript.targetPosition.x, 0.6f, parentCharacterScript.targetPosition.z);               
        parentCharacterScript.castingCoroutine = "spawnAoe";
        parentCharacterScript.castingAbility = this;
        if(targeting!="self")
        {
            parentCharacter.transform.LookAt(point);
        }
        parentCharacterScript.casting = true;
        yield return new WaitForSeconds(baseCastTime / (1 + (parentCharacterScript.attackSpeed / 100)));
        parentCharacterScript.casting = false;
        GameObject aoe = Instantiate(Resources.Load("aoe", typeof(GameObject)), point, Quaternion.identity) as GameObject;
        aoe.transform.localScale = new Vector3(baseAoeRadius * (1 + (parentCharacterScript.bonusArea / 100)), 0.1f,baseAoeRadius * (1 + (parentCharacterScript.bonusArea / 100)));
        aoeScript aoeScriptRef = aoe.GetComponent<aoeScript>();
        aoeScriptRef.abilityAppliedBy = this;
        aoeScriptRef.charAppliedBy = parentCharacterScript;
        aoeScriptRef.damage = baseDamage * (1 + (parentCharacterScript.power / 100));
        aoeScriptRef.healing = baseHealing * (1 + (parentCharacterScript.power / 100));
        aoeScriptRef.duration = aoeDuration;
        aoeScriptRef.offensive = offensive;
        aoeScriptRef.readyToActivate();
        parentCharacterScript.castingCoroutine = null;
        parentCharacterScript.castingAbility = null;
        StartCoroutine("cooldown");
    }

    public void selfTarget()
    {
        if(type=="pointAndClick")
        {
            parentCharacterScript.targetCharacter = parentCharacter;
            StartCoroutine("applyPointandClickEffect");
        }
        else if(type=="aoe")
        {
            parentCharacterScript.targetPosition= new Vector3(parentCharacter.transform.position.x, 0.6f, parentCharacter.transform.position.z);
            StartCoroutine("spawnAoe");
        }
    }

    public void aimDash()
    {
        StartCoroutine(dash(parentCharacter.transform.position + parentCharacter.transform.forward * (baseRange * (1+(parentCharacterScript.bonusRange/100)))));
    }

    IEnumerator cooldown()
    {
        onCooldown = true;
        yield return new WaitForSeconds(baseCooldown/(1+(parentCharacterScript.cooldownReduction/100)));
        onCooldown= false;
        StopCoroutine("cooldown");
    }

    IEnumerator dash(Vector3 targetPosition)
    {
        parentCharacterNav.enabled = false;

        parentCharacterScript.rb.velocity=parentCharacter.transform.forward * parentCharacterScript.moveSpeed*baseRange*parentCharacterScript.bonusRange;

        yield return new WaitForSeconds(0.1f);

        parentCharacterNav.enabled = true;
        parentCharacterScript.rb.angularVelocity = new Vector3(0, 0, 0);
        parentCharacterScript.rb.velocity = new Vector3(0, 0, 0);
    }
}