using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSensor : MonoBehaviour {

    public float steeringInfluence;
    public float sensorLength;

    public bool HitObstacle(out RaycastHit hit)
    {
        RaycastHit raycastHit;
        if (Physics.Raycast(this.transform.position, this.transform.forward, out raycastHit, sensorLength))
        {
            hit = raycastHit;
            return true;
        } else
        {
            hit = raycastHit;
            return false;
        }
    }

    public void DrawLaser()
    {
        Color color;
        if (Physics.Raycast(this.transform.position, this.transform.forward, sensorLength))
            color = Color.red;
        else
            color = Color.green;

        Debug.DrawLine(this.transform.position, this.transform.position + this.transform.forward * sensorLength, color); 
    }
}
