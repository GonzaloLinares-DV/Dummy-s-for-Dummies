using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] Transform startPoint;
    [SerializeField] Transform endPoint;

    public float speed;
    public float directionDelay;
    
    private Transform destinationTarget;
    private Transform departTarget;
    private float startTime;
    private float travelLength;
    bool isWaiting;

    void Start()
    {
        departTarget = startPoint;
        destinationTarget = endPoint;

        startTime = Time.time;
        travelLength = Vector3.Distance(departTarget.position, destinationTarget.position);
    }

    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (!isWaiting)
        {
            if (Vector3.Distance(transform.position, destinationTarget.position) > 0.01f)
            {
                float distCovered = (Time.time - startTime) * speed;
                float fractionOfJourney = distCovered / travelLength;

                transform.position = Vector3.Lerp(departTarget.position, destinationTarget.position, fractionOfJourney);
            }
            else
            {
                isWaiting = true;
                StartCoroutine(ChangeDelay());
            }
        }
    }

    void ChangeDestination()
    {
        if (departTarget == endPoint && destinationTarget == startPoint)
        {
            departTarget = startPoint;
            destinationTarget = endPoint;
        }
        else
        {
            departTarget = endPoint;
            destinationTarget = startPoint;
        }
    }

    IEnumerator ChangeDelay()
    {
        yield return new WaitForSeconds(directionDelay);
        ChangeDestination();
        startTime = Time.time;
        travelLength = Vector3.Distance(departTarget.position, destinationTarget.position);
        isWaiting = false;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")        
            col.transform.parent = transform;       
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")        
            col.transform.parent = null;        
    }
}
