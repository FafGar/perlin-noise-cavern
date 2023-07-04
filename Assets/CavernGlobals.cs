using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CavernGlobals: MonoBehaviour
{
   public float setRadius;
   public int setDistance;
   public float setWaterLevel;

   public static float radius;
   public static int distance;
   public static float waterLevel;

   private void Start() {
        radius = setRadius;
        distance = setDistance;
        waterLevel = setWaterLevel;
   }
}
