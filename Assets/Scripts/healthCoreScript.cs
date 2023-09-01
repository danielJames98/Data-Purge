using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthCoreScript : MonoBehaviour
{
    public Rigidbody rb;
    public AudioSource audioSource;
    public float upForce;
    public float sideForceMin;
    public float sideForceMax;
    public bool frozen;
    public GameObject canvas;
    public AudioClip pickUpSound;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        rb.AddForce(Random.Range(sideForceMin, sideForceMax), upForce, Random.Range(sideForceMin, sideForceMax));
        canvas = Instantiate(Resources.Load("lootCanvas", typeof(GameObject)), this.transform.position, Quaternion.identity) as GameObject;
        canvas.GetComponent<overHeadCanvasScript>().parentCharacter = this.gameObject;
        canvas.GetComponent<overHeadCanvasScript>().yOffset = 1f;
        canvas.transform.Find("Text").GetComponent<TMPro.TextMeshProUGUI>().text = "Health";
    }

    void Update()
    {
        if ((transform.position.y <= 1f || transform.position.y >= 500f && transform.position.y <= 501f) && frozen == false)
        {
            frozen = true;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            rb.constraints = RigidbodyConstraints.FreezePosition;
            rb.velocity = new Vector3(0, 0, 0);
        }
    }

    public void pickUp(playerController pc)
    {
        AudioSource.PlayClipAtPoint(pickUpSound, this.transform.position, 0.2f);
        pc.takeHealing(pc.maxHealth/4);
        Destroy(canvas);
        Destroy(this.gameObject);
    }
}
