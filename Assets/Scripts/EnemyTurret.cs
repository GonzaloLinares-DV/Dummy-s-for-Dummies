using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurret : Turret
{
    public ParticleSystem muzzle;

    void Start()
    {
        canFire = true;
    }

    void Update()
    {
        enemyShoot();
    }

    public void enemyShoot()
    {
        if (canFire) StartCoroutine(FireRate());
        
        IEnumerator FireRate()
        {
            canFire = false;
            
            GameObject currentBullet = Instantiate(enemyBullet, spawnPoint.position, Quaternion.identity);

            muzzle.Play();
            currentBullet.transform.forward = spawnPoint.forward;

            yield return new WaitForSeconds(fireRate);
            canFire = true;
        }
    }
}
