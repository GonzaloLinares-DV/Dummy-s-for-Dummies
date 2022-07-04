using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject enemyBullet;
    public float fireRate = 1f;
    public bool canFire = false;
    
    
    void Start()
    {
        canFire = true;

    }

}
