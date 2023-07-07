using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class fireScript : MonoBehaviour
{
    public float damage;
    public List<GameObject> targets;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(dealDamage());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name =="playerCharacter")
        {
            targets.Add(other.gameObject);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "playerCharacter")
        {
            targets.Remove(other.gameObject);
        }
    }
    
    IEnumerator dealDamage()
    {
        if (targets.Count>0)
        {
            foreach (GameObject target in targets)
            {
                target.GetComponent<playerController>().takeDamage(damage);
            }
        }
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(dealDamage());
    }
}