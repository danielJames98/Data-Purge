using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class projectileScript : MonoBehaviour
{
    public baseAbilityScript abilityAppliedBy;
    public baseCharacter charAppliedBy;
    public baseCharacter charAppliedTo;
    public float range;
    public float speed;
    public Rigidbody rb;
    public Vector3 startPoint;
    public bool piercing;
    public float damage;
    public float healing;
    public bool offensive;

    void Update()
    {
        if(Vector3.Distance(transform.position, startPoint) >= range)
        {
            destroySelf();
        }
    }

    public void readyToFire()
    {
        rb = GetComponent<Rigidbody>();
        startPoint = transform.position;
        rb.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(offensive == true && other.tag != charAppliedBy.tag && other.gameObject.GetComponent<baseCharacter>() != null)
        {
            if (damage>0)
            {
                other.GetComponent<baseCharacter>().takeDamage(damage);
            }

            if (healing > 0)
            {
                other.GetComponent<baseCharacter>().takeHealing(healing);
            }

            if (abilityAppliedBy.appliesEffect)
            {
                abilityAppliedBy.createEffect(other.gameObject);
            }

            if (piercing == false)
            {
                destroySelf();
            }
        }

        else if (offensive == false && other.tag == charAppliedBy.tag && other.gameObject.GetComponent<baseCharacter>() != null)
        {
            if (damage > 0)
            {
                other.GetComponent<baseCharacter>().takeDamage(damage);
            }

            if (healing > 0)
            {
                other.GetComponent<baseCharacter>().takeHealing(healing);
            }

            if (abilityAppliedBy.appliesEffect)
            {
                abilityAppliedBy.createEffect(other.gameObject);
            }

            if (piercing == false)
            {
                destroySelf();
            }
        }
    }

    public void destroySelf()
    {
        Destroy(this.gameObject);
    }
}
