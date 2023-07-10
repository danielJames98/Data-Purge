using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManagerScript : MonoBehaviour
{
    void Start()
    {
        GameObject startingLevel = Instantiate(Resources.Load<GameObject>("levels/level" + Random.Range(0, 4).ToString()));
        int direction = Random.Range(0, 4);
        if(direction == 1)
        {
            startingLevel.transform.Rotate(0,90,0);
        }
        else if (direction == 2)
        {
            startingLevel.transform.Rotate(0, 180, 0);
        }
        else if (direction == 3)
        {
            startingLevel.transform.Rotate(0, 270, 0);
        }

        GameObject player = Instantiate(Resources.Load<GameObject>("PlayerCharacter"), new Vector3(-48,1.5f,0), Quaternion.identity);
        player.transform.Rotate(0, 90, 0);
        startingLevel.GetComponent<levelManagerScript>().lockDown();
    }
}
