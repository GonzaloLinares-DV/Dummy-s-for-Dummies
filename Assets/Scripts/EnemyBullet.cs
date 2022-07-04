using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 3f;
    public float damage;
    public GameObject deadParticles;   

    void Start()
    {      
        Destroy(gameObject, 3);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    public void OnTriggerEnter(Collider col)
    {
        Instantiate(deadParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);

        PlayerCharacter player = col.GetComponent<PlayerCharacter>();
        if(player != null)
        {
            player.Damage(damage);
            Destroy(gameObject);
        }
    }
}
