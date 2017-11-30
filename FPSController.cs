using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Direction { X,Y,Z}

public class FPSController : MonoBehaviour
{
    public float speed = 2f;
    public float sensitivity = 2f;
    public bool invertAxis = false;    
    public GameObject eyes;

    CharacterController player;
    int fpsCnt = 0;
    float moveFB;
    float moveLR;
    float moveUD;
    float rotx;
    float roty;
    // Use this for initialization
    void Start()
    {
        player = GetComponent<CharacterController>();

        float accuracy = 0.00001f;
        int minRange = 0;
        int maxRange = 10;
        int startAngle = 0;
        int EndAngle = 1;
        int stepAngle = 1;
        int zAngle = 10;

        get2DDistance(accuracy, minRange, maxRange, startAngle, EndAngle, stepAngle, zAngle);
    }

    // Update is called once per frame
    void Update()
    {
        playerMovement();
        //------------NUR ZUM TESTEN---------------------
        /*
        fpsCnt++;
        float accuracy = 0.0001f;
        int minRange = 0;
        int maxRange = 10;
        int startAngle = 0;
        int EndAngle = 360;
        int stepAngle = 90;
        int zAngle = 1;

        if(fpsCnt == 60)
        {
            float start_time = Time.time;
            get2DDistance(accuracy,minRange, maxRange, startAngle, EndAngle, stepAngle, zAngle);
            float Zeit = Time.time - start_time;
            Debug.Log("Zeit: " + Zeit);
            fpsCnt = 0;
        }
        */
        //---------------------------------------------------
        //Debug.Log(scanRange());
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

    //this function simulates a Lidar Sensor
    List<float> get2DDistance(float accuracy = 0.1f,
        int minRange = 0, int maxRange = 10, int startAngle = 0, int endAngle = 360, int stepAngle = 1, int zAngle = 0)
    {
        float Angle = 0.0f;
        float Tilt = -zAngle * Mathf.Deg2Rad;
        List<float> list = new List<float> { };
        Vector3 dir = new Vector3(1f, 0f, 0f);
        dir.Set(Mathf.Cos(Angle), Mathf.Sin(Angle), Mathf.Sin(Tilt)); //TODO: support diffrent directions + z angle
        Ray ray = new Ray(this.transform.position, dir);
        for (int j = -zAngle; j < zAngle+1 ; j++) { 
            for (int i = startAngle; i < endAngle; i += stepAngle) {    //iterate through all given angles        
                float dist = get1DDistance(ray, maxRange, accuracy, minRange);
                Debug.Log(j + "  " + i + " Abstand: " + dist);
                list.Add(dist);
                //Winkel setzen
                Angle = ((float)i) * Mathf.Deg2Rad;
                
                //dir und ray anpassen
                dir.Set(Mathf.Cos(Angle), Mathf.Sin(Angle), Mathf.Sin(Tilt));
                ray.direction = dir;
            }
            Tilt = ((float)j) * Mathf.Deg2Rad;
        }
        return list;
    }

    private float get1DDistance(Ray ray, int maxRange, float accuracy = 0.1f ,int minRange =0)
    {
        float tmp = 1 / accuracy;
        maxRange *= (int)tmp;
        minRange *= (int)tmp;
        float distance = minRange * accuracy;
        //Schleife misst den Abstand mit dem aktuellen Raycast  
        for (int i = minRange; maxRange - minRange > 1; i = ((maxRange - minRange) / 2) + minRange)       
        {
            distance = i * accuracy;
            if (Physics.Raycast(ray, distance))
            {                
                maxRange = i;
            }
            else
            {
                minRange = i;
            }
        }
        return distance;
    }
}


/* ---------------------alte Schleife um distanz zu bestimmen
 *             for (int i = minRange; i < maxRange; i++)//Schleife misst den Abstand mit dem aktuellen Raycast
            {
                distance = i * accuracy;
                if (Physics.Raycast(ray, distance))
                {                    
                    list.Add(distance);
                    Debug.Log("alte Methode:" + distance);
                    //Debug.DrawRay(this.transform.position, dir * distance);
                    break;
                }                
            }
*/