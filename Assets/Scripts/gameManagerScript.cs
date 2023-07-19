using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManagerScript : MonoBehaviour
{
    public GameObject startingLevel;
    public GameObject player;
    public GameObject activeLevel;
    void Start()
    {
        startingLevel = Instantiate(Resources.Load<GameObject>("levels/level" + Random.Range(0, 4).ToString()));
        activeLevel = startingLevel;
        player = Instantiate(Resources.Load<GameObject>("PlayerCharacter"), new Vector3(-48,1.5f,0), Quaternion.identity);
        player.transform.Rotate(0, 90, 0);
        startingLevel.GetComponent<levelManagerScript>().lockDown();
    }
}
