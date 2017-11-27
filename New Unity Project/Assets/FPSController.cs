using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour {

    //public float speed = 2f;
    //public float sensitivity = 2f;
    CharacterController player;
 //   public GameObject eyes;
 //   float moveFB;
 //   float moveLR;
 //   float moveUD;
 //   float rotx;
 //   float roty;
	// Use this for initialization
	void Start () {
        player = GetComponent<CharacterController>();
        Debug.Log("Scprit added to: " + gameObject.name);
	}
	
	// Update is called once per frame
	void Update () {

        //if (Input.GetButton("space"))
        //{

        //}
        //else { }
        transform.position += transform.forward * Time.deltaTime * 1.0f;
        transform.Rotate(Input.GetAxis("Vertical"),0.0f,-Input.GetAxis("Horizontal"));
        float TerrainHeight = Terrain.activeTerrain.SampleHeight(transform.position);
        if (TerrainHeight > transform.position.y)
        {
            
            transform.position = new Vector3(transform.position.x, TerrainHeight, transform.position.z);
        }
        //moveFB = Input.GetAxis("Jump") * speed;
        
        //Vector3 movement = new Vector3(0, 0, moveFB);
        //player.Move(movement * Time.deltaTime);
    }
}
