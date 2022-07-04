using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookingCamera : MonoBehaviour
{
    public Transform player;
    public Transform rotatingHorizontal;

    public float rotSpeed;

    public bool follow;
    public bool _lock = true;

    public void Start()
    {
        if (_lock) rotatingHorizontal.transform.LookAt(player);

    }

    private void Update()
    {
        if (follow)
        {
            var position = rotatingHorizontal.position;
            var target = player.position - ((new Vector3(position.x + 0.3f, position.y, position.z)));
            float singleStep = rotSpeed * Time.deltaTime;
            var newDirection = Vector3.RotateTowards(rotatingHorizontal.forward, target, singleStep, 0f);
            rotatingHorizontal.transform.rotation = Quaternion.LookRotation(newDirection);

        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.name == "Player")
            follow = true;
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
            follow = false;
    }
}
