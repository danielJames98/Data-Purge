using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class overHeadCanvasScript : MonoBehaviour
{
    public GameObject parentCharacter;
    public float yOffset;

    void Update()
    {
        if(parentCharacter!=null && transform.position!=new Vector3(parentCharacter.transform.position.x, parentCharacter.transform.position.y+ yOffset, parentCharacter.transform.position.z))
        {
            transform.position = new Vector3(parentCharacter.transform.position.x, parentCharacter.transform.position.y + yOffset, parentCharacter.transform.position.z);
        }
    }
}
