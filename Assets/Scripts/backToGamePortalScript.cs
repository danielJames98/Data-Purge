using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class backToGamePortalScript : MonoBehaviour
{

    public Vector3 warpLocation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameObject.Find("inGameUI(Clone").GetComponent<uiManagerScript>().finalBossButton.SetActive(true);
            other.gameObject.GetComponent<NavMeshAgent>().Warp(warpLocation);
        }
    }
}
