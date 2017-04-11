using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;

public class Wall
{
    // Field
    public string name;
    public GameObject gb = new GameObject();

    // Constructor that takes no arguments.
    public Wall(float size,Vector3 pos)
    {
        gb.name = "wall";
        gb.transform.position = pos;
        //gb.AddComponent<Renderer>();
        gb.AddComponent<MeshFilter>();
        gb.AddComponent<MeshRenderer>();
        gb.AddComponent<BoxCollider>();
        
        Material Mat = Resources.Load("wall", typeof(Material)) as Material;
       
       
        gb.gameObject.GetComponent<MeshFilter>().mesh=createCube(new Vector3(-size/2,-size,-size/2),new Vector3(size / 2, size , size / 2));
        //gb.GetComponent<Renderer>().material = Mat;
        gb.gameObject.GetComponent<MeshRenderer>().material = Mat;
        
        //gb.GetComponent<MeshRenderer>().lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.BlendProbes;
        gb.gameObject.GetComponent<MeshRenderer>().receiveShadows = true;
        // gb.GetComponent<Rigidbody>().freezeRotation = true;

        gb.GetComponent<BoxCollider>().size = new Vector3(size, 2*size, size);


        gb.GetComponent<BoxCollider>().material = Resources.Load("NPM", typeof(PhysicMaterial)) as PhysicMaterial;
       
        
        
         
        //material
    }

    private Mesh createCube(Vector3 min, Vector3 max)
    {
        Mesh mesh = new Mesh();

        int length = 36;

        Vector3[] vertices = new Vector3[length];
        Vector2[] uvs = new Vector2[length];
        int[] triangles = new int[length];

        for (int j = 0; j < length; j++)
        {
            uvs[j] = Vector2.zero;
            triangles[j] = j;
        }


        //left
        vertices[0] = new Vector3(min.x, min.y, min.z);
        uvs[0] = new Vector2(1, 0);
        vertices[1] = new Vector3(min.x, min.y, max.z);
        uvs[1] = new Vector2(0, 0);
        vertices[2] = new Vector3(min.x, max.y, max.z);
        uvs[2] = new Vector2(0, 1);

        vertices[3] = new Vector3(min.x, min.y, min.z);
        uvs[3] = new Vector2(1, 0);
        vertices[4] = new Vector3(min.x, max.y, max.z);
        uvs[4] = new Vector2(0, 1);
        vertices[5] = new Vector3(min.x, max.y, min.z);
        uvs[5] = new Vector2(1, 1);

        //front
        vertices[6] = new Vector3(min.x, min.y, max.z);
        uvs[6] = new Vector2(0, 0);
        vertices[7] = new Vector3(max.x, min.y, max.z);
        uvs[7] = new Vector2(1, 0);
        vertices[8] = new Vector3(min.x, max.y, max.z);
        uvs[8] = new Vector2(0, 1);


        vertices[9] = new Vector3(min.x, max.y, max.z);
        uvs[9] = new Vector2(0, 1);
        vertices[10] = new Vector3(max.x, min.y, max.z);
        uvs[10] = new Vector2(1, 0);
        vertices[11] = new Vector3(max.x, max.y, max.z);
        uvs[11] = new Vector2(1, 1);

        //bottom
        vertices[12] = new Vector3(min.x, min.y, max.z);
        uvs[12] = new Vector2(0, 1);
        vertices[13] = new Vector3(min.x, min.y, min.z);
        uvs[13] = new Vector2(0, 0);
        vertices[14] = new Vector3(max.x, min.y, max.z);
        uvs[14] = new Vector2(1, 1);


        vertices[15] = new Vector3(min.x, min.y, min.z);
        uvs[15] = new Vector2(0, 0);
        vertices[16] = new Vector3(max.x, min.y, min.z);
        uvs[16] = new Vector2(1, 0);
        vertices[17] = new Vector3(max.x, min.y, max.z);
        uvs[17] = new Vector2(1, 1);

        //top
        vertices[18] = new Vector3(min.x, max.y, max.z);
        uvs[18] = new Vector2(0, 1);
        vertices[19] = new Vector3(max.x, max.y, max.z);
        uvs[19] = new Vector2(1, 1);
        vertices[20] = new Vector3(min.x, max.y, min.z);
        uvs[20] = new Vector2(0, 0);


        vertices[21] = new Vector3(min.x, max.y, min.z);
        uvs[21] = new Vector2(0, 0);
        vertices[22] = new Vector3(max.x, max.y, max.z);
        uvs[22] = new Vector2(1, 1);
        vertices[23] = new Vector3(max.x, max.y, min.z);
        uvs[23] = new Vector2(1, 0);

        //back
        vertices[24] = new Vector3(min.x, max.y, min.z);
        uvs[24] = new Vector2(0, 1);
        vertices[25] = new Vector3(max.x, max.y, min.z);
        uvs[25] = new Vector2(1, 1);
        vertices[26] = new Vector3(min.x, min.y, min.z);
        uvs[26] = new Vector2(0, 0);

        vertices[27] = new Vector3(min.x, min.y, min.z);
        uvs[27] = new Vector2(0, 0);
        vertices[28] = new Vector3(max.x, max.y, min.z);
        uvs[28] = new Vector2(1, 1);
        vertices[29] = new Vector3(max.x, min.y, min.z);
        uvs[29] = new Vector2(1, 0);

        //right
        vertices[30] = new Vector3(max.x, max.y, min.z);
        uvs[30] = new Vector2(1, 1);
        vertices[31] = new Vector3(max.x, max.y, max.z);
        uvs[31] = new Vector2(0, 1);
        vertices[32] = new Vector3(max.x, min.y, max.z);
        uvs[32] = new Vector2(0, 0);

        vertices[33] = new Vector3(max.x, max.y, min.z);
        uvs[33] = new Vector2(1, 1);
        vertices[34] = new Vector3(max.x, min.y, max.z);
        uvs[34] = new Vector2(0, 0);
        vertices[35] = new Vector3(max.x, min.y, min.z);
        uvs[35] = new Vector2(1, 0);

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.uv = uvs;

        return mesh;

    }

}