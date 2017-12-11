using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{

    public float MAX_DISTANCE = 10.0f;
    private const float MIN_DISTANCE = 0.01f;
    private LineRenderer laserLineRenderer;
    public float laserWidth = 0.05f;

    public float Output
    {
        get;
        private set;
    }

    // Use this for initialization
    void Start()
    {
        laserLineRenderer = this.GetComponent<LineRenderer>();
        this.ResetLaser();
        laserLineRenderer.startWidth = laserWidth;
        laserLineRenderer.endWidth = laserWidth;

    }

    private void DrawLaser(bool hitDetected, RaycastHit hitInfo)
    {
        this.laserLineRenderer.SetPosition(0, this.transform.position);
        if (hitDetected)
        {
            this.laserLineRenderer.startColor = Color.red;
            laserLineRenderer.endColor = Color.red;
            laserLineRenderer.SetPosition(1, hitInfo.point);
        }
        else
        {
            laserLineRenderer.startColor = Color.green;
            laserLineRenderer.endColor = Color.green;
            laserLineRenderer.SetPosition(1, this.transform.position + this.transform.forward * MAX_DISTANCE);
        }
    }

    public float MeasureDistance()
    {
        RaycastHit hitInfo;
        var hitDetected = Physics.Raycast(this.transform.position, this.transform.forward, out hitInfo, MAX_DISTANCE);
        this.DrawLaser(hitDetected, hitInfo);
        if(hitDetected)
        {
            return Vector3.Distance(this.transform.position, hitInfo.point);
        } else
        {
            return MAX_DISTANCE;
        }
    }

    public void ResetLaser()
    {
        Vector3[] initLaserPositions = new Vector3[2] { Vector3.zero, Vector3.zero };
        laserLineRenderer.SetPositions(initLaserPositions);
    }
}
