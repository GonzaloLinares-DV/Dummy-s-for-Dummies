using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadParticles : MonoBehaviour
{
    // Start is called before the first frame update
    public void Start()
    {
        Destroy(gameObject, 2);
    }
}
