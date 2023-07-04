using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using static CavernGlobals;


[RequireComponent(typeof(MeshFilter))]
public class CreateCavern : MonoBehaviour
{
    Mesh mesh;

    Vector3[] verts;
    int[] polygons;

    float radius;
    int distance;


    public int segmentCount;//Number of points in each ring of the tun

    public float perlinOffset;
    public float perlinScale;
    public float perlinWeight;
    public float distanceStep;
    public float gemScarcity;

    private CavernPathSingleton pathGenerator = CavernPathSingleton.singleton;

    public GameObject debugObject;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        System.Random rand = new System.Random();
        pathGenerator.updateX(rand.Next(0,5),rand.Next(0,5),rand.Next(0,5),rand.Next(0,5));
        pathGenerator.updateY(rand.Next(0,5),rand.Next(0,5),rand.Next(0,5),rand.Next(0,5));

        radius = CavernGlobals.radius;
        distance = CavernGlobals.distance;

        makeMesh();
        updateMesh();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void makeMesh(){
        verts = new Vector3[segmentCount*distance];
        polygons = new int[(distance-1)*segmentCount*6];

        if(perlinOffset <=0){
            perlinOffset = 0.0001f;
        }
        if(perlinScale <=0){
            perlinScale = 0.0001f;
        }
        if(perlinWeight <=0){
            perlinWeight = 1f;
        }

        int vertCount = 0;
        for(int dist = 0; dist<distance; dist ++){
            //making functions to follow based on parameterized 3d functions.
            float t = dist;
            Vector3 center = pathGenerator.functionMachine(t);
            // Instantiate(debugObject, center, Quaternion.identity); //DEBUG OBJECTS
            for(int theta = 0; theta<segmentCount-1; theta++){
                float noise = Mathf.PerlinNoise((theta+perlinOffset)/(float)perlinScale, (dist+perlinOffset)/(float)perlinScale)*perlinWeight;
                float phi = (float)Math.PI*2f /segmentCount *theta+ (float)Math.PI*0.5f; //determining the angle of each segment from the center. Offest to hide the seam from the last bit of it.
                Vector3 radialOffset = new Vector3(Mathf.Cos(phi)*(radius+noise), -Mathf.Sin(phi)*(radius+noise), 0);
                verts[vertCount] = center+radialOffset;  
                vertCount++;
                if(phi-(float)Math.PI<Math.PI*1.5f && phi-(float)Math.PI>Math.PI*-0.5f){
                    System.Random rand  = new System.Random();
                    if(rand.NextDouble() > gemScarcity){
                        Instantiate(Resources.Load<GameObject>("EmeraldOre"), center+radialOffset, Quaternion.LookRotation(-radialOffset)*Quaternion.AngleAxis(90f,Vector3.right));
                    }
                }
            }
            //Creating a smoother transition from beginning to end vertices in each vertex ring
            verts[vertCount] = Vector3.Lerp(verts[vertCount-1], verts[vertCount-vertCount%segmentCount], 0.5f);
            vertCount++;
        }
        vertCount = 0;
        for(int dist = 1; dist<distance; dist ++){
            for(int theta = 0; theta<segmentCount-1; theta++){
                //Creating the first triangle for this segment
                polygons[vertCount] = (dist-1)*segmentCount + theta;// The vertices must be in the mesh triangles array in clockwise order from the desired direction of viewing
                polygons[vertCount+1] = (dist-1)*segmentCount + (theta+1);// We are looking from the inside out, hence the interesting ordering
                polygons[vertCount+2] = (dist)*segmentCount + (theta);
                vertCount+=3;

                //Creating the second triangle
                polygons[vertCount] = (dist)*segmentCount + (theta);
                polygons[vertCount+1] = (dist-1)*segmentCount + (theta+1);
                polygons[vertCount+2] = (dist)*segmentCount + (theta+1);
                vertCount+=3;
            }
            /* Handling the inherent loop case of this creating a circular object.
                This covers the last strip of polygons which must loop to the beginning of the array.
            */
                polygons[vertCount] = (dist)*segmentCount + (segmentCount-1);
                polygons[vertCount+1] = (dist-1)*segmentCount + (0);
                polygons[vertCount+2] = (dist)*segmentCount + (0);
                vertCount+=3;
                polygons[vertCount] = (dist-1)*segmentCount + (segmentCount-1);
                polygons[vertCount+1] = (dist-1)*segmentCount + (0);
                polygons[vertCount+2] = (dist)*segmentCount + (segmentCount-1);
                vertCount+=3;

        }

    }

    void updateMesh(){
        mesh.Clear();

        mesh.vertices = verts;
        mesh.triangles = polygons;
        mesh.RecalculateNormals();
    }

}
