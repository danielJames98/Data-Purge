using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawnerScript : MonoBehaviour
{
    public void spawn()
    {
        GameObject enemy = Instantiate (Resources.Load<GameObject>("baseEnemy"), new Vector3(this.transform.position.x, this.transform.position.y + 2, this.transform.position.z), Quaternion.identity);
    }
}