using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorSwitchScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            transform.parent.GetComponent<levelManagerScript>().completeObjective();
        }
    }
}