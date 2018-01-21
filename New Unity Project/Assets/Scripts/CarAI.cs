using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAI : MonoBehaviour {

    private int currentWaypointIndex = 0;
    private bool isAvoidingObstacle = false;
    private float stuckCounter = 0.0f;
    private bool reversing = false;
    private float reversingDuration = 0.0f;

    public Path path;
    public float sensorLength = 3.0f;

    public CarSensor frontSensor;
    public CarSensor leftForwardSensor;
    public CarSensor rightForwardSensor;
    public CarSensor leftAngledSensor;
    public CarSensor rightAngledSensor;


    private SimpleCarController carController;

    // Use this for initialization
    void Start () {
        carController = this.GetComponent<SimpleCarController>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        this.CheckObstacle();
        this.ApplySteer();
        this.CheckPosition();
	}

    void CheckObstacle()
    {
        this.isAvoidingObstacle = false;
        float steeringInfluence = 0.0f;
        RaycastHit hit;

        if (this.leftForwardSensor.HitObstacle(out hit))
        {
            this.isAvoidingObstacle = true;
            steeringInfluence += leftForwardSensor.steeringInfluence;

        } else if (this.leftAngledSensor.HitObstacle(out hit))
        {
            this.isAvoidingObstacle = true;
            steeringInfluence += leftAngledSensor.steeringInfluence;
        }

        if (this.rightForwardSensor.HitObstacle(out hit))
        {
            this.isAvoidingObstacle = true;
            steeringInfluence -= rightForwardSensor.steeringInfluence;

        }
        else if (this.rightAngledSensor.HitObstacle(out hit))
        {
            this.isAvoidingObstacle = true;
            steeringInfluence -= rightAngledSensor.steeringInfluence;
        }

        if(steeringInfluence == 0.0f)
        {
            if (this.frontSensor.HitObstacle(out hit))
            {
                this.isAvoidingObstacle = true;
                if(hit.normal.x < 0)
                {
                    steeringInfluence = -1.0f;
                } else
                {
                    steeringInfluence = 1.0f;
                }

            }
        }

        if(this.gameObject.GetComponent<Rigidbody>().velocity.magnitude <= 0.1f && !this.carController.reversing && this.isAvoidingObstacle)
        {
            // Count time the car is stuck
            this.stuckCounter += Time.deltaTime;
        }
        else
        {
            this.stuckCounter = 0.0f;
        }

        this.CheckDeadEnd();

        if(this.carController.reversing)
        {
            // Count time the car is rerversing
            this.reversingDuration += Time.deltaTime;
        }

        this.CheckReverseDuration();
       

        if (this.isAvoidingObstacle)
        {
            this.carController.steerDirection = steeringInfluence;
        }

    }

    void ApplySteer()
    {

        if (this.carController.reversing)
            return;

        if (this.isAvoidingObstacle)
            return;

        var inversePoint = this.transform.InverseTransformPoint(path.waypoints[currentWaypointIndex].position);
        var steerDirection = inversePoint.x / inversePoint.magnitude;
        this.carController.steerDirection = steerDirection;
    }

    void CheckPosition()
    {
        if((this.transform.position - path.waypoints[currentWaypointIndex].position).magnitude < 10.0f)
        {
            currentWaypointIndex++;
            if(currentWaypointIndex == path.waypoints.Count)
            {
                currentWaypointIndex = 0;
            }
        }
    }

    void CheckDeadEnd()
    {
        if (this.stuckCounter >= 5.0f)
        {
            this.carController.reversing = true;
        }
    }

    void CheckReverseDuration()
    {
        if (this.reversingDuration >= 10.0f)
        {
            this.stuckCounter = 0.0f;
            this.reversingDuration = 0.0f;
            this.carController.reversing = false;
        }
    }

    void OnDrawGizmos()
    {
        this.DrawSensors();
    }

    private void DrawSensors()
    {
        this.frontSensor.DrawLaser();
        this.leftForwardSensor.DrawLaser();
        this.rightForwardSensor.DrawLaser();
        this.leftAngledSensor.DrawLaser();
        this.rightAngledSensor.DrawLaser();
    }
}
