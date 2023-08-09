using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aoeScript : MonoBehaviour
{
    public baseAbilityScript abilityAppliedBy;
    public baseCharacter charAppliedBy;
    public Rigidbody rb;
    public float duration;
    public float damage;
    public float healing;
    public List<baseCharacter> charsInAoe;
    public bool offensive;
    public GameObject vfx;
    public ParticleSystem portal;
    public ParticleSystem smoke;
    public ParticleSystem sparks;
    public string charAppliedByTag;

    public void readyToActivate()
    {
        rb = GetComponent<Rigidbody>();
        charAppliedByTag = charAppliedBy.tag;
        StartCoroutine("durationTimer");
        if (abilityAppliedBy.appliesEffect&& abilityAppliedBy.stun==false)
        {
            StartCoroutine("applyEffects");
        }
        StartCoroutine("impact");

        startVfx();
    }

    public void startVfx()
    {
        portal=vfx.GetComponent<ParticleSystem>();
        smoke=portal.transform.Find("Smoke").gameObject.GetComponent<ParticleSystem>();
        sparks = portal.transform.Find("CircleSparks").gameObject.GetComponent<ParticleSystem>();

        if(charAppliedBy.gameObject.tag=="Player")
        {
            portal.startColor = Color.cyan;
            smoke.startColor= Color.cyan;
            sparks.startColor= Color.cyan;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != null)
        {
            if (offensive == true && other.tag != charAppliedByTag)
            {
                if (other.gameObject.GetComponent<baseCharacter>() != null)
                {
                    charsInAoe.Add(other.gameObject.GetComponent<baseCharacter>());
                    other.gameObject.GetComponent<baseCharacter>().aoesColliding.Add(this);
                }
            }
            else if (offensive == false && other.tag == charAppliedByTag && other.gameObject.GetComponent<baseCharacter>() != null)
            {
                charsInAoe.Add(other.gameObject.GetComponent<baseCharacter>());
                other.gameObject.GetComponent<baseCharacter>().aoesColliding.Add(this);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(charsInAoe.Contains(other.gameObject.GetComponent<baseCharacter>()))
        {
            charsInAoe.Remove(other.gameObject.GetComponent<baseCharacter>());
            if(other.gameObject.GetComponent<baseCharacter>().aoesColliding.Contains(this))
            {
                other.gameObject.GetComponent<baseCharacter>().aoesColliding.Remove(this);
            }
            
        }
    }

    IEnumerator impact()
    {
        yield return new WaitForSeconds(0.1f);

        if (charsInAoe.Count > 0)
        {
            foreach (baseCharacter character in charsInAoe)
            {
                if (character!=null)
                {
                    if (damage > 0)
                    {
                        character.takeDamage(damage);
                    }

                    if (healing > 0)
                    {
                        character.GetComponent<baseCharacter>().takeHealing(healing);
                    }

                    if (abilityAppliedBy.stun == true)
                    {
                        abilityAppliedBy.createEffect(character.gameObject);
                    }
                }
            }
        }


    }

    IEnumerator applyEffects()
    {
        foreach (baseCharacter character in charsInAoe)
        {
            if (character != null)
            {
                abilityAppliedBy.createEffect(character.gameObject);
            }      
        }      

        yield return new WaitForSeconds(0.5f);

        StartCoroutine("applyEffects");
    }

    IEnumerator durationTimer()
    {
        yield return new WaitForSeconds(duration);
        foreach(baseCharacter character in charsInAoe)
        {
            if(character.aoesColliding.Contains(this))
            {
                character.aoesColliding.Remove(this);
            }
        }
        Destroy(this.gameObject);
    }

    public void triggerRemoveTarget(baseCharacter target)
    {
        StartCoroutine(removeTarget(target));
    }

    public IEnumerator removeTarget(baseCharacter target)
    {
        yield return new WaitForEndOfFrame();
        charsInAoe.Remove(target);
    }
}
