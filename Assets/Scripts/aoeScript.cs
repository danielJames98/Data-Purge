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

    public void readyToActivate()
    {
        rb = GetComponent<Rigidbody>();

        StartCoroutine("durationTimer");
        if (abilityAppliedBy.appliesEffect&& abilityAppliedBy.stun==false)
        {
            StartCoroutine("applyEffects");
        }
        StartCoroutine("impact");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(offensive==true && other.tag!=charAppliedBy.tag && other.gameObject.GetComponent<baseCharacter>()!=null)
        {
            charsInAoe.Add(other.gameObject.GetComponent<baseCharacter>());
        }
        else if(offensive==false && other.tag == charAppliedBy.tag && other.gameObject.GetComponent<baseCharacter>() != null)
        {
            charsInAoe.Add(other.gameObject.GetComponent<baseCharacter>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(charsInAoe.Contains(other.gameObject.GetComponent<baseCharacter>()))
        {
            charsInAoe.Remove(other.gameObject.GetComponent<baseCharacter>());
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
        Destroy(this.gameObject);
    }
}
