using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CavernGlobals;


[RequireComponent(typeof(MeshFilter))]
public class MakeWater : MonoBehaviour
{

    CavernPathSingleton pathGenerator = CavernPathSingleton.singleton;
    private float radius;
    private int distance;
    float waterDepth;//Floating point representing what percentage of a radius the water is below the center


    private Vector3 [] verts;
    private int [] polygons;

    
    public int segmentsPerDistance;

    float yOffset;
    Vector3 leftVertexOffset;
    Vector3 rightVertexOffset;

    Mesh mesh;

    public GameObject debugObject;

    // Start is called before the first frame update
    void Start()
    {

        if(Mathf.Abs(waterDepth)<0.000001){
            waterDepth = 0.00001f;
        }
        
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        makeMesh();
        updateMesh();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void makeMesh(){
        radius = CavernGlobals.radius;
        distance = CavernGlobals.distance;
        waterDepth = CavernGlobals.waterLevel;

        yOffset = -radius*waterDepth;
        leftVertexOffset = new Vector3(-2*radius, yOffset,0);
        rightVertexOffset = new Vector3(2*radius, yOffset,0);
        
        verts = new Vector3[((distance+1)*segmentsPerDistance)*2];
        polygons = new int[((distance+1)*segmentsPerDistance)*6];

        int vertCount = 0;
        for(int dist = 0; dist<distance; dist ++){
            //making functions to follow based on parameterized 3d functions.
            if(dist%segmentsPerDistance == 0){
                Vector3 center = pathGenerator.functionMachine((float)dist);
                verts[vertCount] = center+leftVertexOffset;
                // Instantiate(debugObject, center+leftVertexOffset, Quaternion.identity);
                verts[vertCount+1] = center+rightVertexOffset;
                // Instantiate(debugObject, center+rightVertexOffset, Quaternion.identity);
                vertCount+=2;
            }
        }
        Vector3 finalCenter = pathGenerator.functionMachine((float)distance+1);
        verts[vertCount] = finalCenter+leftVertexOffset;
        // Instantiate(debugObject, finalCenter+leftVertexOffset, Quaternion.identity);
        verts[vertCount+1] = finalCenter+rightVertexOffset;
        // Instantiate(debugObject, finalCenter+rightVertexOffset, Quaternion.identity);
        vertCount+=2;


        vertCount = 0;
        for(int loc = 1; loc<distance*segmentsPerDistance+1; loc ++){
                //Creating the first triangle for this segment
                int dist = loc*2;
                polygons[vertCount] = (dist-2); //Bottom left vertex
                polygons[vertCount+1] = (dist); //Top Left vertex
                polygons[vertCount+2] = (dist-1);//Bottom right Vertex
                vertCount+=3;

                //Creating the second triangle
                polygons[vertCount] = (dist);//Top Left Vertex
                polygons[vertCount+1] = (dist+1);//Top Right Vertex
                polygons[vertCount+2] = (dist-1);//Bottom Right Vertex
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
