using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class homingRangeScript : MonoBehaviour
{
    private void Update()
    {
        if(transform.position != transform.parent.transform.position)
        {
            transform.position = transform.parent.transform.position;
        }
    
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject !=null && other.tag != transform.parent.tag && other.gameObject.GetComponent<baseCharacter>() != null && transform.parent.GetComponent<projectileScript>().homingTarget==null)
        {
            transform.parent.GetComponent<projectileScript>().homingTarget=other.gameObject;
            gameObject.GetComponent<SphereCollider>().enabled = false;
            other.GetComponent<baseCharacter>().projectilesHoming.Add(transform.parent.GetComponent<projectileScript>());
        }
        /*
        else if (transform.parent.GetComponent<projectileScript>().offensive == false && other.tag == transform.parent.GetComponent<projectileScript>().charAppliedBy.tag && other.gameObject.GetComponent<baseCharacter>() != null && transform.parent.GetComponent<projectileScript>().homingTarget == null)
        {
            transform.parent.GetComponent<projectileScript>().homingTarget = other.gameObject;
            gameObject.GetComponent<SphereCollider>().enabled = false;
            other.GetComponent<baseCharacter>().projectilesHoming.Add(transform.parent.GetComponent<projectileScript>());
        }
        */
    }
}
