using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CavernPathSingleton{
    public static CavernPathSingleton singleton = new CavernPathSingleton();
    int xBasic,xComplex,xRapid,xMod = 1;// Variables for determining variability in the x and y directions
    int yBasic,yComplex,yRapid,yMod = 1;
    float tOffset = 0f; //offset the t variable for the function to increase randomness.

    public void updateX(int xBasic, int xComplex, int xRapid, int xMod){
        this.xBasic = xBasic;
        this.xComplex = xComplex;
        this.xRapid = xRapid;
        this.xMod = xMod;
        tOffset = Random.Range(0f, 201f);
    }

    public void updateY(int yBasic, int yComplex, int yRapid, int yMod){
        this.yBasic = yBasic;
        this.yComplex = yComplex;
        this.yRapid = yRapid;
        this.yMod = yMod;
        tOffset = Random.Range(0f, 201f);
    }

    public Vector3 functionMachine(float t){
        float tModified = t + tOffset;
        float x = xBasic*3/(1+Mathf.Sin(t/7)/2) + xComplex*Mathf.Cos(t/xMod)*Mathf.Cos(1/(6+xComplex*Mathf.Pow(Mathf.Sin(t/9),2))) + xRapid*Mathf.Sin(t/20/xMod+xRapid/xMod);
        float y = yBasic*3/(1+Mathf.Cos(t/10)/2) + yComplex*Mathf.Sin(t/(2*yMod))*Mathf.Cos(1/(8+yComplex*Mathf.Pow(Mathf.Sin(t/5),2))) + yRapid*Mathf.Cos(t/40/yMod+yRapid/(2*yMod));
        return new Vector3(x,y,t);
    }
}
