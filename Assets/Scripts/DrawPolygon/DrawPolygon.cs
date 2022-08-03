using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPolygon : MonoBehaviour
{
    public float horizentalLength = 5f;
    public float verticalLength = 5f;

    private GameObject dragablePointPrefab;
    [SerializeField]
    private List<DragableObject>allPoints;
    public List<DragableObject> AllPoints => allPoints;

    private void Start() {
        #region TEST
        Init();
        CreateBasicPolygon(5,4);
        #endregion
    }
    private void Update() {
        UpdatePolygon();
    }

    private GameObject DragablePointPrefab{get{return this.dragablePointPrefab;}}
    public void Init()
    {
        dragablePointPrefab = Resources.Load<GameObject>("Prefabs/DragablePoint");
        allPoints = new List<DragableObject>();
    }

    //根据长宽生成长方形
    public void CreateBasicPolygon(float h,float v)
    {
        GameObject fGO = Instantiate(dragablePointPrefab,new Vector3(-h/2f,0,-v/2f),Quaternion.identity,transform);
        GameObject sGO = Instantiate(dragablePointPrefab,new Vector3(h/2f,0,-v/2f),Quaternion.identity,transform);
        GameObject tGO = Instantiate(dragablePointPrefab,new Vector3(h/2f,0,v/2f),Quaternion.identity,transform);
        GameObject foGO = Instantiate(dragablePointPrefab,new Vector3(-h/2f,0,v/2f),Quaternion.identity,transform);
        allPoints.Add(fGO.GetComponent<DragableObject>());
        allPoints.Add(sGO.GetComponent<DragableObject>());
        allPoints.Add(tGO.GetComponent<DragableObject>());
        allPoints.Add(foGO.GetComponent<DragableObject>());

        allPoints[0].Init(allPoints[1]);
        allPoints[1].Init(allPoints[2]);
        allPoints[2].Init(allPoints[3]);
        allPoints[3].Init(allPoints[0]);
    }

    public void AddControlPoint(Vector3 position,DragableObject prePoint)
    {
        GameObject newGO = Instantiate(dragablePointPrefab,position,Quaternion.identity,transform);
        DragableObject dragableObject = newGO.GetComponent<DragableObject>();

        int index = allPoints.IndexOf(prePoint);
        allPoints.Insert(index+1,dragableObject);

        dragableObject.Init(allPoints[index].GetNextPoint());
        allPoints[index].UpdateNextPoint(dragableObject);
    }

    public void RemoveControlPoint(DragableObject item)
    {
        allPoints.Remove(item);
    }

    public void UpdatePolygon()
    {
        for(int i=0;i<allPoints.Count;i++)
        {
            allPoints[i].UpdateLine();
        }
    }

    public void Clear()
    {
        allPoints.Clear();
        allPoints = null;
        dragablePointPrefab = null;
    }
}
