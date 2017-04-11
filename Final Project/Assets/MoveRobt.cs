using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;

public class MoveRobt : MonoBehaviour
{
    float CX = 0;
    float CY = 3.1f;
    float CZ = 0;

    
    float root_y_a = 0;
   

    float root_ro_l_timer = 0;
    float root_ro_r_timer = 0;


    float Arm_l_a = 0;
    float Arm_r_a = 0;
    
    float Leg_l_a = 0;
    float Leg_r_a = 0;

    bool arm_mode = true;
  
    String walk_mode = "walk";

    int boxesCount = 0;

    int level = 0;

    bool finished = false;

    Rect windowRect = new Rect(Screen.width/2-160, Screen.height / 2-50, 320, 100);

    float startTime;

    float timeNow;

    float CameraR = 4;

    int viewMode = 1;

    void Start()
    {
        LoadMap("map" + level + ".txt");
        startTime = Time.fixedTime;
    }
    
    
    // Update is called once per frame
    void Update()
    {
        checkCollison();
        checkBoxes();
        

        GameObject.Find("Label3").GetComponent<Text>().text = "Time: " + Math.Floor(Time.fixedTime-startTime).ToString() + "s";
    }

    void OnGUI()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
           
                viewMode = 2;

                GameObject.Find("Main Camera").transform.position = new Vector3(CX, CY+1, CZ);

                GameObject.Find("Main Camera").transform.LookAt(new Vector3(CX + 4 * (float)Math.Sin(root_y_a * 3.14f / 180), CY, CZ + 4 * (float)Math.Cos(root_y_a * 3.14f / 180)));
           
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
                viewMode = 1;

                GameObject.Find("Main Camera").transform.position = new Vector3(CX - 4 * (float)Math.Sin(root_y_a * 3.14f / 180), CY + 2, CZ - 4 * (float)Math.Cos(root_y_a * 3.14f / 180));

                GameObject.Find("Main Camera").transform.LookAt(new Vector3(CX, CY, CZ));
            
        }

        if (GUI.Button(new Rect(270, 5, 100, 40), "Reset")){
            startTime = Time.fixedTime;
            CX = 0;
            CY = 3.22f;
            CZ = 0;


            root_y_a = 0;


            root_ro_l_timer = 0;
            root_ro_r_timer = 0;


            Arm_l_a = 0;
            Arm_r_a = 0;

            Leg_l_a = 0;
            Leg_r_a = 0;

            GameObject.Find("Robot").gameObject.transform.rotation = Quaternion.AngleAxis(0, Vector3.up);

            //left arm
            GameObject.Find("la").gameObject.transform.localRotation = Quaternion.AngleAxis(0, Vector3.right);

            //right arm
            GameObject.Find("ra").gameObject.transform.localRotation = Quaternion.AngleAxis(0, Vector3.right);

            //left leg
            GameObject.Find("ll").gameObject.transform.localRotation = Quaternion.AngleAxis(0, Vector3.right);

            //right leg
            GameObject.Find("rl").gameObject.transform.localRotation = Quaternion.AngleAxis(0, Vector3.right);

            finished = false;

            walk_mode = "walk";

            LoadMap("map" + level + ".txt");
        }



        if (finished == true)
        {
            windowRect = GUI.Window(0, windowRect, DoMyWindow, "Congratulations!");  
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            
            if (root_ro_l_timer == 0 && root_ro_r_timer == 0)
            {
                root_y_a = GameObject.Find("Robot").gameObject.transform.rotation.eulerAngles.y;
                float speed = 0.05f;
                
                CX += speed * (float)Math.Sin(root_y_a * 3.14f / 180);
                CZ += speed * (float)Math.Cos(root_y_a * 3.14f / 180);

                RaycastHit hit;
                RaycastHit hit2;
                RaycastHit hit3;

                float tx = (float)Math.Sin(root_y_a * 3.14f / 180);
                float tz = (float)Math.Cos(root_y_a * 3.14f / 180);

                Ray ray = new Ray(new Vector3(CX, CY, CZ), new Vector3(tx, 0, tz));

                Vector3 v2=new Vector3(CX - 0.55f * (float)Math.Sin((root_y_a + 90) * 3.14f / 180), CY , CZ - 0.55f * (float)Math.Cos((root_y_a + 90) * 3.14f / 180));

                Ray ray2 = new Ray(v2, new Vector3(tx, 0, tz));

                Vector3 v3 = new Vector3(CX - 0.55f * (float)Math.Sin((root_y_a - 90) * 3.14f / 180), CY, CZ - 0.55f * (float)Math.Cos((root_y_a - 90) * 3.14f / 180));

                Ray ray3 = new Ray(v3, new Vector3(tx, 0, tz));

                //make it three based on tx tz

                if (Physics.Raycast(ray, out hit)&&Physics.Raycast(ray2, out hit2) && Physics.Raycast(ray3, out hit3))
                {
                    
                    //

                    if (hit.collider != null&&(hit.collider.gameObject.name.Equals("wall")||hit.collider.gameObject.name.Equals("box"))&& hit2.collider != null 
                        && (hit2.collider.gameObject.name.Equals("wall") || hit2.collider.gameObject.name.Equals("box"))
                         && (hit3.collider.gameObject.name.Equals("wall") || hit3.collider.gameObject.name.Equals("box")))
                    {
                        Vector3 nv = hit.point;
                        Vector3 nv2 = hit2.point;
                        Vector3 nv3 = hit3.point;

                        if (Math.Round(tx) != 0)
                        {
                            Debug.Log(nv.x + "   " + nv2.x);
                            if (Math.Round(tx) >0)
                            {
                                float smaller=small(nv.x,nv2.x,nv3.x);
                                 
                               
                                
                                if (CX >= smaller - 0.6f)
                                {
                                     CX = smaller - 0.6f;

                                }
                            }
                            else if (Math.Round(tx) < 0)
                            {
                                float bigger = big(nv.x, nv2.x, nv3.x);

                                if (CX <= bigger + 0.6f)
                                {
                                    CX = bigger + 0.6f;

                                }
                            }
                        }else{
                            if (Math.Round(tz) >0)
                            {
                                float smaller = small(nv.z, nv2.z, nv3.z);

                                if (CZ >= smaller - 0.6f)
                                {
                                    CZ = smaller - 0.6f;
                                 
                                }
                            }
                            else if (Math.Round(tz) < 0)
                            {
                                float bigger = big(nv.z, nv2.z, nv3.z);
                                if (CZ <= bigger + 0.6f)
                                {
                                    CZ = bigger + 0.6f;
                                 
                                }
                            }
                        }

                    }

                }

                transform.position = new Vector3(CX, CY, CZ);
                if (viewMode == 1)
                {
                        GameObject.Find("Main Camera").transform.position = new Vector3(CX - 4 * (float)Math.Sin(root_y_a * 3.14f / 180), CY + 2, CZ - 4 * (float)Math.Cos(root_y_a * 3.14f / 180));

                        GameObject.Find("Main Camera").transform.LookAt(new Vector3(CX, CY, CZ));
                }else
                {
                        GameObject.Find("Main Camera").transform.position = new Vector3(CX, CY+1, CZ);

                        GameObject.Find("Main Camera").transform.LookAt(new Vector3(CX + 4 * (float)Math.Sin(root_y_a * 3.14f / 180), CY, CZ + 4 * (float)Math.Cos(root_y_a * 3.14f / 180)));
                }

                

            }

            //left arm
            GameObject.Find("la").gameObject.transform.localRotation = Quaternion.AngleAxis(Arm_l_a, Vector3.right);

            //right arm
            GameObject.Find("ra").gameObject.transform.localRotation = Quaternion.AngleAxis(Arm_r_a, Vector3.right);
            
            //left leg
            GameObject.Find("ll").gameObject.transform.localRotation = Quaternion.AngleAxis(Leg_l_a, Vector3.right);
        
            //right leg
            GameObject.Find("rl").gameObject.transform.localRotation = Quaternion.AngleAxis(Leg_r_a, Vector3.right);

            if (walk_mode.Equals("walk"))
            { 
                if (arm_mode == true)
                {
                    Arm_l_a += 5;
                    Arm_r_a -= 5;
                    Leg_l_a -= 5;
                    Leg_r_a += 5;
                    if (Arm_l_a >= 45) arm_mode = false;
                }
                 else
                {
                    Arm_l_a -= 5;
                    Arm_r_a += 5;
                    Leg_l_a += 5;
                    Leg_r_a -= 5;
                    if (Arm_l_a <= -45) arm_mode = true;
                }
            }else if (walk_mode.Equals("push"))
            {
                GameObject.Find("la").gameObject.transform.localRotation = Quaternion.AngleAxis(-90, Vector3.right);

                //right arm
                GameObject.Find("ra").gameObject.transform.localRotation = Quaternion.AngleAxis(-90, Vector3.right);

                if (arm_mode == true)
                {
                    Arm_l_a += 5;
                    Arm_r_a -= 5;
                    Leg_l_a -= 5;
                    Leg_r_a += 5;
                    if (Arm_l_a >= 45) arm_mode = false;
                }
                else
                {
                    Arm_l_a -= 5;
                    Arm_r_a += 5;
                    Leg_l_a += 5;
                    Leg_r_a -= 5;
                    if (Arm_l_a <= -45) arm_mode = true;
                }
            } 

        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (root_ro_l_timer == 0 && root_ro_r_timer == 0) root_ro_l_timer = 1.8f;
        }

        if (root_ro_l_timer > 0)
        {
           
            root_ro_l_timer -= 0.1f;
            if (root_ro_l_timer <= 0) root_ro_l_timer = 0;

            root_y_a -= 5;
            //bones[5].transform.rotation = Quaternion.AngleAxis(root_y_a, Vector3.up);

            GameObject.Find("Robot").gameObject.transform.rotation = Quaternion.AngleAxis(root_y_a, Vector3.up);

            if (viewMode == 1)
            {
                GameObject.Find("Main Camera").transform.position = new Vector3(CX - 4 * (float)Math.Sin(root_y_a * 3.14f / 180), CY + 2, CZ - 4 * (float)Math.Cos(root_y_a * 3.14f / 180));

                GameObject.Find("Main Camera").transform.LookAt(new Vector3(CX, CY, CZ));
            }else
            {
                GameObject.Find("Main Camera").transform.position = new Vector3(CX, CY+1, CZ);

                GameObject.Find("Main Camera").transform.LookAt(new Vector3(CX + 4 * (float)Math.Sin(root_y_a * 3.14f / 180), CY, CZ + 4 * (float)Math.Cos(root_y_a * 3.14f / 180)));
            }
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (root_ro_l_timer == 0 && root_ro_r_timer == 0) root_ro_r_timer = 1.8f;
        }

        if (root_ro_r_timer > 0)
        {
            
            root_ro_r_timer -= 0.1f;
            if (root_ro_r_timer <= 0) root_ro_r_timer = 0;

            root_y_a += 5;

            GameObject.Find("Robot").gameObject.transform.rotation = Quaternion.AngleAxis(root_y_a, Vector3.up);

            if (viewMode == 1)
            {
                GameObject.Find("Main Camera").transform.position = new Vector3(CX - 4 * (float)Math.Sin(root_y_a * 3.14f / 180), CY + 2, CZ - 4 * (float)Math.Cos(root_y_a * 3.14f / 180));

                GameObject.Find("Main Camera").transform.LookAt(new Vector3(CX, CY, CZ));
            }else
            {
                GameObject.Find("Main Camera").transform.position = new Vector3(CX, CY+1, CZ);

                GameObject.Find("Main Camera").transform.LookAt(new Vector3(CX + 4 * (float)Math.Sin(root_y_a * 3.14f / 180), CY, CZ + 4 * (float)Math.Cos(root_y_a * 3.14f / 180)));
            }
        }


       // checkCollison();
    }

    float small(float a, float b,float c)
    {
        if (a <= b && a <= c) return a;
        else if (b <= a && b <= c) return b;
        else return c;
    }

    float big(float a, float b, float c)
    {
        if (a >= b && a >= c) return a;
        else if (b >= a && b >= c) return b;
        else return c;
    }



    void DoMyWindow(int windowID)
    {

        if (GUI.Button(new Rect(110, 45, 100, 40), "Next Level"))
        {
            level++;
            if (level > 2) level = 0;
            startTime = Time.fixedTime;

            CX = 0;
            CY = 3.22f;
            CZ = 0;

           
            root_y_a = 0;
           

            root_ro_l_timer = 0;
            root_ro_r_timer = 0;


            Arm_l_a = 0;
            Arm_r_a = 0;

            Leg_l_a = 0;
            Leg_r_a = 0;

            GameObject.Find("Robot").gameObject.transform.rotation = Quaternion.AngleAxis(0, Vector3.up);

            //left arm
            GameObject.Find("la").gameObject.transform.localRotation = Quaternion.AngleAxis(0, Vector3.right);

            //right arm
            GameObject.Find("ra").gameObject.transform.localRotation = Quaternion.AngleAxis(0, Vector3.right);

            //left leg
            GameObject.Find("ll").gameObject.transform.localRotation = Quaternion.AngleAxis(0, Vector3.right);

            //right leg
            GameObject.Find("rl").gameObject.transform.localRotation = Quaternion.AngleAxis(0, Vector3.right);

            finished = false;

            walk_mode = "walk";

            LoadMap("map" + level + ".txt");
            

        }

    }

    void checkCollison()
    {
        GameObject[] tempBoxes = GameObject.FindGameObjectsWithTag("box");
        GameObject[] tempWalls = GameObject.FindGameObjectsWithTag("wall");
        //box 1.8 wall 2.0
        for (int i = 0; i < tempBoxes.Length; i++)
        {
            for(int j = 0; j < tempWalls.Length; j++)
            {
                float dx = tempBoxes[i].transform.position.x - tempWalls[j].transform.position.x;
                float dz = tempBoxes[i].transform.position.z - tempWalls[j].transform.position.z;
                if (Math.Abs(dx) < 1.9 && Math.Abs(dz) < 1.9)
                {
                    Debug.Log("before: " + dz);
                    if(Math.Abs(dz)> Math.Abs(dx))
                    {
                        if (dz > 0)
                        {
                            tempBoxes[i].transform.position =new Vector3(tempBoxes[i].transform.position.x, tempBoxes[i].transform.position.y, tempWalls[j].transform.position.z + 1.93f);
                        }
                        else
                        {
                            tempBoxes[i].transform.position = new Vector3(tempBoxes[i].transform.position.x, tempBoxes[i].transform.position.y, tempWalls[j].transform.position.z - 1.93f);
                        }
                    }else if (Math.Abs(dx) > Math.Abs(dz))
                    {
                        if (dx > 0)
                        {
                            tempBoxes[i].transform.position = new Vector3(tempWalls[j].transform.position.x + 1.93f, tempBoxes[i].transform.position.y, tempBoxes[i].transform.position.z);
                        }
                        else
                        {
                            tempBoxes[i].transform.position = new Vector3(tempWalls[j].transform.position.x - 1.93f, tempBoxes[i].transform.position.y, tempBoxes[i].transform.position.z);
                        }

                    }
                    Debug.Log("after: " + dz);
                }
            }
        }
    }

    private void checkBoxes()
    {

        GameObject[] tempBoxes = GameObject.FindGameObjectsWithTag("box");
        GameObject[] tempGB = GameObject.FindGameObjectsWithTag("target");

        int solvedCount = 0;

        for (int j = 0; j < tempBoxes.Length; j++)
        {
            Vector3 tempVec = tempBoxes[j].transform.position;
            Boolean IsArrived = false;
            for (int i = 0; i < tempGB.Length; i++)
            {
                if ((tempVec.x - tempGB[i].transform.position.x) * (tempVec.x - tempGB[i].transform.position.x) + (tempVec.z - tempGB[i].transform.position.z) * (tempVec.z - tempGB[i].transform.position.z) < 0.25)
                {
                    tempBoxes[j].GetComponent<MeshRenderer>().material = Resources.Load("destiny", typeof(Material)) as Material;
                    IsArrived = true;
                    solvedCount++;
                    break;
                }
            }
            if (IsArrived == false) tempBoxes[j].GetComponent<MeshRenderer>().material = Resources.Load("box", typeof(Material)) as Material;
        }

        GameObject.Find("Label1").GetComponent<Text>().text = "Finished: " + solvedCount;
        GameObject.Find("Label2").GetComponent<Text>().text = "Unsolved: " + (boxesCount - solvedCount);

        if (boxesCount - solvedCount == 0)
        {
            finished = true;
        }

    }

    public void LoadMap(String file)
    {
        GameObject[] walls = GameObject.FindGameObjectsWithTag("wall");

        foreach (GameObject w in walls)
        {
            w.SetActive(false);
        }

        GameObject[] boxes = GameObject.FindGameObjectsWithTag("box");

        foreach (GameObject w in boxes)
        {
            w.tag = "null";
            w.SetActive(false);

        }

        GameObject[] targets = GameObject.FindGameObjectsWithTag("target");

        foreach (GameObject w in targets)
        {
            w.tag = "null";
            w.SetActive(false);

        }

        StreamReader sr = new StreamReader(file);
        int row = 0;
        boxesCount = 0;
        while (sr.Peek() >= 0)
        {
            string temp = sr.ReadLine();
            for (int i = 0; i < temp.Length; i++)
            {
                if (temp.Substring(i, 1).Equals("*"))
                {
                    GroundBox gb = new GroundBox(2, new Vector3(-4 + i * 2, 1, -4 + row * 2), "target");
                    gb.gb.gameObject.tag = "target";
                }
                else if (temp.Substring(i, 1).Equals("#"))
                {
                    Wall wall = new Wall(2.0f, new Vector3(-4 + i * 2, 2, -4 + row * 2));
                    wall.gb.gameObject.tag = "wall";
                }
                else if (temp.Substring(i, 1).Equals("-"))
                {
                    GroundBox gb = new GroundBox(2, new Vector3(-4 + i * 2, 1, -4 + row * 2), "floor");
                    gb.gb.gameObject.tag = "floor";
                }
                else if (temp.Substring(i, 1).Equals("?"))
                {
                    GroundBox gb = new GroundBox(2, new Vector3(-4 + i * 2, 1, -4 + row * 2), "floor");
                    gb.gb.gameObject.tag = "floor";
                    Box box = new Box(1.8f, new Vector3(-4 + i * 2, 3, -4 + row * 2));
                    box.gb.gameObject.tag = "box";
                    boxesCount++;
                }
                else if (temp.Substring(i, 1).Equals("!"))
                {
                    GroundBox gb = new GroundBox(2, new Vector3(-4 + i * 2, 1, -4 + row * 2), "floor");
                    GameObject.Find("Robot").transform.position = new Vector3(-4 + i * 2, 3.1f, -4 + row * 2);
                    CX = -4 + i * 2;
                    CY = 3.22f;
                    CZ = -4 + row * 2;
                    if (viewMode == 1)
                    {
                        GameObject.Find("Main Camera").transform.position = new Vector3(CX - CameraR * (float)Math.Sin(root_y_a * 3.14f / 180), CY + 2, CZ - CameraR * (float)Math.Cos(root_y_a * 3.14f / 180));

                     GameObject.Find("Main Camera").transform.LookAt(new Vector3(CX, CY, CZ));
                    }else{
                        GameObject.Find("Main Camera").transform.position = new Vector3(CX, CY+1, CZ);

                        GameObject.Find("Main Camera").transform.LookAt(new Vector3(CX + 4 * (float)Math.Sin(root_y_a * 3.14f / 180), CY, CZ + 4 * (float)Math.Cos(root_y_a * 3.14f / 180)));
                    }

                }

            }

            row++;
        }

        GameObject.Find("Label1").GetComponent<Text>().text = "Finished: 0";
        GameObject.Find("Label2").GetComponent<Text>().text = "Unsolved: " + boxesCount;
    }



    void OnCollisionEnter(Collision collision)
    {
       // Debug.Log(collision.gameObject.name);
        
        if (collision.gameObject.CompareTag("box"))
        {
            walk_mode = "push";
        }
        
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("box"))
        {
            walk_mode = "push";
        }
        

    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("box"))
        {
            
            GameObject.Find("la").gameObject.transform.localRotation = Quaternion.AngleAxis(Arm_l_a, Vector3.right);

            //right arm
            GameObject.Find("ra").gameObject.transform.localRotation = Quaternion.AngleAxis(Arm_r_a, Vector3.right);
            walk_mode = "walk";
        }
    }
}
