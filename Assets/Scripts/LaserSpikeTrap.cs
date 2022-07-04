using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSpikeTrap : MonoBehaviour
{
    public float damage;


    public void OnTriggerEnter(Collider col)
    {
        PlayerCharacter player = col.GetComponent<PlayerCharacter>();
        if (player != null)
        {
            player.Damage(damage);
        }
    }
}
