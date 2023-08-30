using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class projectileScript : MonoBehaviour
{
    public baseAbilityScript abilityAppliedBy;
    public baseCharacter charAppliedBy;
    public baseCharacter charAppliedTo;
    public GameObject homingTarget;
    public SphereCollider homingRange;
    public float range;
    public float speed;
    public Rigidbody rb;
    public Vector3 startPoint;
    public bool piercing;
    public bool returning;
    public bool homing;
    public float damage;
    public float healing;
    public bool offensive;
    public Material enemyMat;
    public float maxLifeTime;
    public bool aoeOnHit;

    void Update()
    {
        if(Vector3.Distance(transform.position, startPoint) >= range)
        {
            if (returning == false)
            {
                destroySelf();
            }
            else if (returning == true && homing == false)
            {
                rb.velocity = rb.velocity * -1;
                startPoint = transform.position;
                returning = false;
            }
            else if (returning == true && homing == true && charAppliedBy!=null && charAppliedBy.gameObject != null)
            {
                homingTarget=charAppliedBy.gameObject;
                charAppliedBy.GetComponent<baseCharacter>().projectilesHoming.Add(this);
            }
            else if (returning==true && homing == true && (charAppliedBy==null || charAppliedBy.gameObject == null))
            {
                homingTarget = null;
                homing = false;
                returning=false;
                transform.LookAt(startPoint);
                rb.velocity = transform.forward * speed;             
            }
        }

        if (homingTarget != null && homingTarget.gameObject != null)
        {
            transform.LookAt(homingTarget.transform.position);
            rb.velocity = transform.forward * speed;
        }
        
    }

    public void readyToFire()
    {
        rb = GetComponent<Rigidbody>();
        startPoint = transform.position;
        rb.velocity = transform.forward * speed;
        if (charAppliedBy.gameObject.tag=="Enemy")
        {
            gameObject.GetComponent<MeshRenderer>().material = enemyMat;
        }
        if(homing==true)
        {
            homingRange.enabled = true;
        }
        StartCoroutine("lifeTime");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != null && other.gameObject.GetComponent<baseCharacter>() != null)
        {
            if (other.tag != tag && damage > 0)
            {
                other.GetComponent<baseCharacter>().takeDamage(damage);
            }
            else if (other.tag == tag && healing > 0)
            {
                other.GetComponent<baseCharacter>().takeHealing(healing);
            }

            if (abilityAppliedBy.appliesEffect)
            {
                abilityAppliedBy.createEffect(other.gameObject);
            }

            if(aoeOnHit==true&& other.tag!=tag)
            {
                abilityAppliedBy.spawnAoe(new Vector3(transform.position.x, transform.position.y-1, transform.position.z));
            }
        }

        if (homingTarget != null && other.gameObject == homingTarget && returning == true && homingTarget != charAppliedBy.gameObject)
        {
            homingTarget = charAppliedBy.gameObject;
            startPoint = transform.position;
        }
        else if(homingTarget != null && other.gameObject == homingTarget && charAppliedBy!=null && homingTarget == charAppliedBy.gameObject)
        {
            destroySelf();
        }
        else if (homingTarget != null && other.gameObject == homingTarget && returning == false)
        {
            destroySelf();
        }

        if (piercing == false && other.gameObject.layer==3 && other.tag!=tag)
        {
            destroySelf();
        }
    }

    public void destroySelf()
    {
        Destroy(this.gameObject);
    }

    IEnumerator lifeTime()
    {
        yield return new WaitForSeconds(maxLifeTime);
        destroySelf();
    }
}
