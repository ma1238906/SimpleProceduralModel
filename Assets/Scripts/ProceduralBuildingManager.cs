using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralBuildingManager : MonoBehaviour
{
    public DrawPolygon FoundationPolygon;
    public DrawWall2 drawWall;
    [Range(1,20)]
    public int Floors=2;
    [Range(2,100)]
    public float TotalHeight=5;

    [ContextMenu("创建")]
    public void GenerationBuilding()
    {
        List<Vector3> temp = new List<Vector3>();
        for(int i=0;i<FoundationPolygon.AllPoints.Count;i++)
        {
            temp.Add(FoundationPolygon.AllPoints[i].transform.position);
        }
        drawWall.CreateBuildingObject(temp,Floors,TotalHeight);
    }
}
