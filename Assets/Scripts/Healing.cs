using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healing : MonoBehaviour
{
    public float heal;

    public void OnTriggerEnter(Collider col)
    {
        AudioManager.instance.Play("Heal");

        PlayerCharacter player = col.GetComponent<PlayerCharacter>();
        if (player != null)
        {
            player.Heal(heal);
            Destroy(gameObject);
        }
    }
}
