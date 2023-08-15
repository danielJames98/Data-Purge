using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camController : MonoBehaviour
{
    public GameObject player;
    public Camera cam;
    public float zoomSpeed;
    public float minFOV;
    public float maxFOV;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("playerCharacter(Clone)");
        cam = this.GetComponent<Camera>();
        GameObject.Find("gameManager").GetComponent<gameManagerScript>().cam = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(player!= null) 
        {
            this.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 15f, player.transform.position.z - 9f);
        }
        
        if((Input.GetAxis("Mouse ScrollWheel") > 0) && cam.fieldOfView > minFOV)
        {
            cam.fieldOfView = cam.fieldOfView - zoomSpeed;
            
            if(cam.fieldOfView < minFOV)
            {
                cam.fieldOfView = minFOV;
            }
            
        }

        if ((Input.GetAxis("Mouse ScrollWheel") < 0) && cam.fieldOfView < maxFOV)
        {
            cam.fieldOfView = cam.fieldOfView + zoomSpeed;

            if (cam.fieldOfView > maxFOV)
            {
                cam.fieldOfView = maxFOV;
            }
        }
    }    
}