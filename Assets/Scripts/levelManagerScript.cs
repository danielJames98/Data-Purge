using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class levelManagerScript : MonoBehaviour
{
    public bool objectiveComplete=false;
    public List<GameObject> firewalls;
    public Material openDoorMat;
    public Material closedDoorMat;

    public void completeObjective()
    {
        if(objectiveComplete==false) 
        {
            objectiveComplete = true;

            var overlaps = Physics.OverlapSphere(new Vector3(this.transform.position.x + 27, this.transform.position.y, this.transform.position.z), 1);

            if(overlaps.Length==0)
            {
                GameObject newLevel0 = Instantiate(Resources.Load<GameObject>("levels/level" + Random.Range(0, 4).ToString()), new Vector3(this.transform.position.x + 50, this.transform.position.y, this.transform.position.z), Quaternion.identity);
                turnNewLevel(newLevel0);
            }

            overlaps = Physics.OverlapSphere(new Vector3(this.transform.position.x - 27, this.transform.position.y, this.transform.position.z), 1);

            if (overlaps.Length == 0)
            {
                GameObject newLevel1 = Instantiate(Resources.Load<GameObject>("levels/level" + Random.Range(0, 4).ToString()), new Vector3(this.transform.position.x - 50, this.transform.position.y, this.transform.position.z), Quaternion.identity);
                turnNewLevel(newLevel1);
            }

            overlaps = Physics.OverlapSphere(new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z+27), 1);

            if (overlaps.Length == 0)
            {
                GameObject newLevel2 = Instantiate(Resources.Load<GameObject>("levels/level" + Random.Range(0, 4).ToString()), new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + 50), Quaternion.identity);
                turnNewLevel(newLevel2);
            }

            overlaps = Physics.OverlapSphere(new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - 27), 1);

            if (overlaps.Length == 0)
            {
                GameObject newLevel3 = Instantiate(Resources.Load<GameObject>("levels/level" + Random.Range(0, 4).ToString()), new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - 50), Quaternion.identity);
                turnNewLevel(newLevel3);
            }

            foreach (GameObject firewall in firewalls)
            {
                Destroy(firewall);
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag=="Player" && objectiveComplete==false)
        {
            lockDown();
        }
    }

    public void lockDown()
    {
        this.GetComponent<BoxCollider>().enabled = false;

        foreach(GameObject firewall in firewalls)
        {
            firewall.GetComponent<NavMeshLink>().enabled = false;
            firewall.GetComponent<Renderer>().material = closedDoorMat;
        }
    }

    public void turnNewLevel(GameObject levelToTurn)
    {
        int direction = Random.Range(0, 4);

        if (direction == 1)
        {
            levelToTurn.transform.Rotate(0, 90, 0, 0);
        }
        else if (direction == 2)
        {
            levelToTurn.transform.Rotate(0, 180, 0, 0);
        }
        else if (direction == 3)
        {
            levelToTurn.transform.Rotate(0, 270, 0, 0);
        }
    }
}
