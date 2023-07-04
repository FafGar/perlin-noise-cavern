using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CavernGlobals;

public class MoveBoat : MonoBehaviour
{
    CavernPathSingleton pathGenerator = CavernPathSingleton.singleton;
    const float MOVE_SPEED = 3f;
    float t = 0f;

    float radius;
    float waterLevel;

    void Start()
    {
        transform.position = pathGenerator.functionMachine(1);
        transform.rotation *= Quaternion.FromToRotation(pathGenerator.functionMachine(1), transform.position);
        radius = CavernGlobals.radius;
        waterLevel = CavernGlobals.waterLevel;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.LeftAlt)){

            t += MOVE_SPEED * Time.deltaTime;
            Vector3 new_pos = pathGenerator.functionMachine(t)- new Vector3(0,waterLevel*radius,0);

            transform.LookAt(new_pos);
            transform.rotation *= new Quaternion(0.707106829f,0f,0f,0.707106829f); //Just turning the placeholder capsule on its side
            transform.position = new_pos;
            
        }
    }
}
