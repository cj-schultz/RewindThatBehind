using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoPointOscilate : MonoBehaviour
{    
    public Transform pointTwo;
    public float oscillateSpeed = 5;

    public float forceDamper = 2;

    private bool oscillating;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    
    private Vector3 currentTarget;

    public void Reset()
    {
        transform.position = originalPosition;
        transform.rotation = originalRotation;

        currentTarget = pointTwo.position;

        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        GetComponent<Rigidbody>().useGravity = false;

        oscillating = true;
    }

    private void Awake()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        currentTarget = pointTwo.position;
        oscillating = true;        
    }

    private void Update()
    {       
        if(oscillating)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentTarget, Time.deltaTime * oscillateSpeed);

            if(transform.position == currentTarget)
            {
                if (currentTarget == originalPosition)
                {
                    currentTarget = pointTwo.position;
                }
                else
                {                    
                    currentTarget = originalPosition;
                }
            }
        }        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(oscillating)
        {
            GetComponent<Rigidbody>().useGravity = true;

            if(collision.rigidbody.GetComponent<Rigidbody>().isKinematic)
            {                
                GetComponent<Rigidbody>().AddForceAtPosition(collision.relativeVelocity / forceDamper, collision.contacts[0].point, ForceMode.Impulse);
            }            

            oscillating = false;
        }        
    }
}
