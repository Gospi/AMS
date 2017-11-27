using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Direction { X,Y,Z}

public class FPSController : MonoBehaviour
{
    public float speed = 2f;
    public float sensitivity = 2f;
    public bool invertAxis = false;
    CharacterController player;
    public GameObject eyes;
    float moveFB;
    float moveLR;
    float moveUD;
    float rotx;
    float roty;
    // Use this for initialization
    void Start()
    {
        player = GetComponent<CharacterController>();
        get2DDistance();
    }

    // Update is called once per frame
    void Update()
    {
        playerMovement();
        //Debug.Log(scanRange());
        

        /*
        if (CheckDistance(1f, Direction.X))
        {
            Debug.Log("Gegenstand in X Richtung");
        }
        if (CheckDistance(1f, Direction.Y))
        {
            Debug.Log("Gegenstand in Y Richtung");
        }
        if (CheckDistance(1f, Direction.Z))
        {
            Debug.Log("Gegenstand in Z Richtung");
        }
        */
    }

    void playerMovement(){
        moveFB = Input.GetAxis("Vertical") * speed;
        moveLR = Input.GetAxis("Horizontal") * speed;
        moveUD = Input.GetAxis("ZAxis") * speed;

        rotx = Input.GetAxis("Mouse X") * sensitivity;
        roty = Input.GetAxis("Mouse Y") * sensitivity;

        Vector3 movement = new Vector3(moveLR, moveUD, moveFB);
        transform.Rotate(0, rotx, 0);
        eyes.transform.Rotate(-roty, 0, 0);
        movement = transform.rotation * movement;
        player.Move(movement * Time.deltaTime);
    }

    float scanRange()
    {
        int maxRange = 50;
        float tmp = 0f;
        for (int i = 0; i < maxRange; i++)
        {
            tmp = 0.1f * i;
            if (CheckDistance(tmp, Direction.X))
            {
                Debug.Log(" X");
                break;
            }
            if(CheckDistance(tmp, Direction.Y))
            {
                Debug.Log(" Y");
                break;
            }
            if(CheckDistance(tmp, Direction.Z))
            {
                Debug.Log(" Z");
                break;
            }
        }
        return tmp;
    }
    bool CheckDistance(float distance , Direction dir)
    {
        Vector3 dirf = new Vector3(0f, 0f, 0f);
        Vector3 dirb = new Vector3(0f, 0f, 0f);
        switch (dir){
            //X-Direction
            case Direction.X:
                dirf.Set(1f, 0f, 0f);
                dirb.Set(-1f, 0f, 0f);
                break;
            //Y-Direction
            case Direction.Y:
                dirf.Set(0f, 1f, 0f);
                dirb.Set(0f, -1f, 0f);
                break;
            //Z-Direction
            case Direction.Z:
                dirf.Set(0f, 0f, 1f);
                dirb.Set(0f, 0f, -1f);
                break;
            default:
                break;
         }
        Ray rayFront = new Ray(this.transform.position, dirf);
        Ray rayBack = new Ray(this.transform.position, dirb);

        if(Physics.Raycast(rayFront, distance)||
        Physics.Raycast(rayBack, distance))
        {
            return true;
        }
        return false;

    }
    List<float> get2DDistance()        
    {
        float accuracy = 0.01f;
        List<float> list = new List<float> { };
        Vector3 dir = new Vector3(1f, 0f, 0f);
        dir = this.transform.forward;
        //Quaternion q = Quaternion.AngleAxis(1, dir);
        Ray ray = new Ray(this.transform.position, dir);
        float xAxisVal = 0.0f;
        float yAxisVal = 0.0f;
        for (int j = 0; j < 50; j++) {
            
            for (int i = 0; i < 10000; i++)//Schleife misst den Abstand mit dem aktuellen Raycast
            {
                float tmp = i * accuracy;
                if (Physics.Raycast(ray, tmp))
                {
                    Debug.DrawRay(this.transform.position, dir * 3);
                    list.Add(tmp);
                    Debug.Log(j+ ": Abstand" + tmp);
                    break;
                }
                
            }
            //q = Quaternion.AngleAxis(1, dir);
            xAxisVal = ((float)j) / 3.6f;
            yAxisVal = ((float)j) / 3.6f;
            //Debug.Log("xAxisVal " + xAxisVal);
            //Debug.Log("yAxisVal " + yAxisVal);
            dir.Set(Mathf.Cos(xAxisVal) , Mathf.Sin(yAxisVal),0);
            Debug.Log("dir: " + dir);
            ray.direction =  dir;
        }
        return list;
    }
}