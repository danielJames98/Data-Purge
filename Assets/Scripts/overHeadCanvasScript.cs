using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class overHeadCanvasScript : MonoBehaviour
{
    public GameObject parentCharacter;

    void Update()
    {
        if(parentCharacter!=null && transform.position!=new Vector3(parentCharacter.transform.position.x, parentCharacter.transform.position.y+2, parentCharacter.transform.position.z))
        {
            transform.position = new Vector3(parentCharacter.transform.position.x, parentCharacter.transform.position.y + 2, parentCharacter.transform.position.z);
        }
    }
}
