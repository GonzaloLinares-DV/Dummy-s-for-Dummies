using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurretChase : Turret
{
    public Transform part1;
    public Transform part2;
    public Transform player;
    public float range = 30f;
    public float speed = 5f;
    public GameObject laserSight;
    public ParticleSystem laserLight;


    void Start()
    {
        canFire = true;
    }

    void Update()
    {
        LockOn();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);

    }

    public void LockOn()
    {
        float dist = Vector3.Distance(player.position, transform.position);
        if (dist <= range)
        {
            Vector3 dir = player.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation1 = Quaternion.Lerp(part2.rotation, lookRotation, Time.deltaTime * speed).eulerAngles;
            Vector3 rotation2 = Quaternion.Lerp(part1.rotation, lookRotation, Time.deltaTime * speed).eulerAngles;
            part2.rotation = Quaternion.Euler(0f, rotation1.y, 0f);
            part1.rotation = Quaternion.Euler(rotation2.x, rotation2.y, rotation2.z);

            laserSight.SetActive(true);
            laserLight.Play();

            if (canFire) StartCoroutine(FireRate());

            IEnumerator FireRate()
            {
                canFire = false;

                GameObject currentBullet = Instantiate(enemyBullet, spawnPoint.position, Quaternion.identity);
                
                currentBullet.transform.forward = spawnPoint.forward;

                yield return new WaitForSeconds(fireRate);
                canFire = true;
            }
        }
        else
        {
            laserSight.SetActive(false);
            laserLight.Stop();
        }
    }
}
