using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollision : MonoBehaviour
{

    void OnCollisionEnter(Collision col)
    {
        Debug.Log("collision detected");
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger detected: " + other.name);
    }
}
