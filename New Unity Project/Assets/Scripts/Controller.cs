using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {

    private List<KeyValuePair<float, float>> laserReadings = new List<KeyValuePair<float, float>>();
    private List<Vector3> visitedPositions = new List<Vector3>();
    private Vector3 nextDestination;
    private MovementState movementState;
    private int maxDegrees = 360;
    private int degressRotated = 0;
    private float radius = 0.5f;
    private bool moved = false;
    public Sensor mainSensor;

    public enum MovementState
    {
        Stop, Prepare, Moving
    }

	// Use this for initialization
	void Start () {
        this.visitedPositions.Add(this.transform.position);
        this.movementState = MovementState.Stop;
	}
	
	// Update is called once per frame
	void Update () {

        this.HandleMovement(this.movementState);
	}

    private void HandleMovement(MovementState movementState)
    {
        switch (movementState)
        {
            case MovementState.Stop:
                if (degressRotated < maxDegrees)
                {
                    var distance = this.mainSensor.MeasureDistance();
                    this.mainSensor.transform.Rotate(this.mainSensor.transform.up, 1.0f);
                    this.laserReadings.Add(new KeyValuePair<float, float>(degressRotated, distance));
                    degressRotated++;
                } else
                {
                    this.movementState = MovementState.Prepare;
                }
                break;
            case MovementState.Prepare:
                this.laserReadings = this.laserReadings.OrderByDescending(x => x.Value).ToList();
                KeyValuePair<float, float> reading = new KeyValuePair<float, float>(0, 0);
                reading = this.laserReadings[0];
                var nextDestination = this.transform.position + this.transform.forward * reading.Value * 0.2f;
                this.nextDestination = nextDestination;
                this.transform.Rotate(this.transform.up, reading.Key);
                this.nextDestination = this.transform.position + this.transform.forward * reading.Value * 0.2f;
                this.movementState = MovementState.Moving;
                break;
            case MovementState.Moving:
                this.ResetLasers();
                this.transform.position = Vector3.MoveTowards(this.transform.position, this.nextDestination, 0.05f);
                if (this.transform.position == this.nextDestination)
                {
                    this.visitedPositions.Add(this.transform.position);
                    this.degressRotated = 0;
                    this.nextDestination = this.transform.position;
                    this.movementState = MovementState.Stop;
                    this.laserReadings.Clear();
                }
                break;
        }
    }

    private void ResetLasers()
    {
        this.mainSensor.ResetLaser();
    }

}
