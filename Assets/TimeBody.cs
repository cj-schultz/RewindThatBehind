using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBody : MonoBehaviour
{
    public bool rewindable = true;

    public bool conserveVelocity = false;

    [Header("Optional")]
    public Material rewindingMaterial;
    public Material nonRewindableMaterial;
    public MeshRenderer[] meshesToChange;

    private bool isRewinding = false;
    
    [SerializeField]
    private List<PointInTime> pointsInTime;

    // Index of point in time -> how many timesteps it stays in this position/rotation
    private Dictionary<int, uint> stillFrameCounter;

    private Rigidbody rb;

    private Material originalMaterial;

    public int IndexOfMostRecentPointInTime
    {
        get
        {
            if (pointsInTime.Count == 0)
            {
                return 0;
            }
            else
            {
                return pointsInTime.Count - 1;
            }
        }
    }

    void Start()
    {
        pointsInTime = new List<PointInTime>();
        stillFrameCounter = new Dictionary<int, uint>();

        rb = GetComponent<Rigidbody>();

        if(meshesToChange.Length > 0)
        {
            originalMaterial = meshesToChange[0].material;
        }
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartRewind();
        }
            
        if (Input.GetKeyUp(KeyCode.Return))
        {
            StopRewind();
        }            
    }

    void FixedUpdate()
    {
        if (isRewinding)
        {
            Rewind();
        }            
        else
        {
            RecordThisTimestep();
        }            
    }
    
    private void SetObjectToPointInTime(PointInTime pointInTime)
    {
        transform.position = pointInTime.position;
        transform.rotation = pointInTime.rotation;

        if(conserveVelocity)
        {
            rb.velocity = -pointInTime.velocity;
            rb.angularVelocity = -pointInTime.angularVelocity;
        }        
    }

    private void Rewind()
    {
        if (pointsInTime.Count > 0)
        {
            // Set transform and velocities
            PointInTime pointInTime = pointsInTime[IndexOfMostRecentPointInTime];
            SetObjectToPointInTime(pointInTime);            

            // Manage the stillFrameCounter and the pointsInTime
            if(stillFrameCounter.ContainsKey(IndexOfMostRecentPointInTime))
            {
                stillFrameCounter[IndexOfMostRecentPointInTime]--;

                //Debug.Log("Time --> " + Time.time + " --- Popping key " + IndexOfMostRecentPointInTime + ": indices left --> " + stillFrameCounter[IndexOfMostRecentPointInTime]);               
                if(stillFrameCounter[IndexOfMostRecentPointInTime] == 0)
                {
                    int index = IndexOfMostRecentPointInTime;
                    pointsInTime.RemoveAt(index);
                    stillFrameCounter.Remove(index);                    
                }                
            }
            else
            {
                pointsInTime.RemoveAt(IndexOfMostRecentPointInTime);
            }                        
        }
        else
        {
            StopRewind();
        }
    }

    private void RecordThisTimestep()
    {        
        if(IndexOfMostRecentPointInTime > 0)
        {
            PointInTime lastPoint = pointsInTime[IndexOfMostRecentPointInTime];
            if (transform.position == lastPoint.position && transform.rotation == lastPoint.rotation)
            {
                if (stillFrameCounter.ContainsKey(IndexOfMostRecentPointInTime))
                {
                    stillFrameCounter[IndexOfMostRecentPointInTime]++;
                }
                else
                {
                    stillFrameCounter.Add(IndexOfMostRecentPointInTime, 1);
                }                
            }
            else
            {                
                pointsInTime.Add(new PointInTime(transform.position, transform.rotation, rb.velocity, rb.angularVelocity));
            }
        }
        else
        {
            pointsInTime.Add(new PointInTime(transform.position, transform.rotation, rb.velocity, rb.angularVelocity));
        }                
    }

    public void StartRewind()
    {        
        if(rewindable)
        {
            isRewinding = true;
            rb.isKinematic = true;

            for (int i = 0; i < meshesToChange.Length; i++)
            {
                meshesToChange[i].material = rewindingMaterial;
            }
        }        
    }

    public void StopRewind()
    {        
        if(rewindable)
        {
            isRewinding = false;
            rb.isKinematic = false;

            if (pointsInTime.Count > 0)
            {
                PointInTime pointInTime = pointsInTime[IndexOfMostRecentPointInTime];
                SetObjectToPointInTime(pointInTime);
            }

            for (int i = 0; i < meshesToChange.Length; i++)
            {
                meshesToChange[i].material = originalMaterial;
            }
        }        
    }   
    
    public void SetToUnrewindableMaterial()
    {
        for (int i = 0; i < meshesToChange.Length; i++)
        {
            meshesToChange[i].material = nonRewindableMaterial;
        }
    }

    public void ToggleRewindable()
    {
        if(isRewinding)
        {
            StopRewind();
        }

        rewindable = !rewindable;
        
        if(rewindable)
        {
            for (int i = 0; i < meshesToChange.Length; i++)
            {
                meshesToChange[i].material = originalMaterial;
            }
        }     
        else
        {
            for (int i = 0; i < meshesToChange.Length; i++)
            {
                meshesToChange[i].material = nonRewindableMaterial;
            }
        }   
    } 
}
