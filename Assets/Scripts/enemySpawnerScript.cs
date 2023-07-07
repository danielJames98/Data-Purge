using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawnerScript : MonoBehaviour
{
    void Start()
    {
        GameObject enemy = Instantiate(Resources.Load<GameObject>("baseEnemy"), new Vector3(this.transform.position.x, 1.5f, this.transform.position.z), Quaternion.identity);
    }
}