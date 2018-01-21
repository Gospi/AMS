using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour {

    public Color color = Color.green;
    public List<Transform> waypoints = new List<Transform>();

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnDrawGizmos()
    {
        waypoints.Clear();
        var children = gameObject.GetComponentInChildren<Transform>();
        foreach (Transform child in children)
        {
            waypoints.Add(child);
        }


        for (int i = 0; i < waypoints.Count; i++)
        {
            Gizmos.color = color;

            if(i > 0)
            {
                Debug.DrawLine(waypoints[i].position, waypoints[i - 1].position, color);
                Gizmos.DrawWireSphere(waypoints[i].position, 0.5f);
            }
        }
    }
}
