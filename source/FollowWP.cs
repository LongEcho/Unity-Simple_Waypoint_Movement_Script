using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowWP : MonoBehaviour
{
    [Tooltip("Tip: Press the small lock in the top right corner to drag multiple waypoints into the list.")]
    public GameObject[] waypoints;

    [Tooltip("Should the object repeat the path?")]
    public bool loop;
    [Tooltip("How fast should the object move?")] 
    public float speed = 10.0f;
 
    [Header("")]
    [Header("(optional):")]

    [Header("Rotate to Target Settings")]
    [Tooltip("Here you can change whether this object should rotate towards the object.")]
    public bool rotateToTarget;
    [Tooltip("Rotation Speed")]
    public float rotSpeed = 10.0f;

    private float startTime;
    private float distance;

    //use the 'moving' variable to check if the object has stopped
    [HideInInspector]
    public bool moving = true;

    [Header("Debug")]
    [Tooltip("You don't have to change that...")]
    public int currentWP = 0;
    [Tooltip("You don't have to change that...")]
    public int lastWP = 0;

    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        if (Vector3.Distance(this.transform.position, waypoints[currentWP].transform.position) < 0.001 && moving)
        {
            lastWP = currentWP;
            currentWP++;
            startTime = Time.time;
            if (currentWP >= waypoints.Length && loop)
            {
                currentWP = 0;
            }
            if (currentWP >= waypoints.Length && !loop)
            {
                currentWP -= 1;
                moving = false;
                return;
            }

            distance = Vector3.Distance(waypoints[lastWP].transform.position, waypoints[currentWP].transform.position);
        }

        if (rotateToTarget)
        {
            this.transform.LookAt(waypoints[currentWP].transform);

            Quaternion lookatWP = Quaternion.LookRotation(waypoints[currentWP].transform.position - this.transform.position);

            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, lookatWP, rotSpeed * Time.deltaTime);

            this.transform.Translate(0, 0, speed * Time.deltaTime);
        }
        else
        {
            float distanceCompleted = (Time.time - startTime) * speed;
            float distanceCompletedPercent = distanceCompleted / distance;
            this.transform.position = Vector3.Lerp(waypoints[lastWP].transform.position, waypoints[currentWP].transform.position, distanceCompletedPercent);
        }
    }
}
