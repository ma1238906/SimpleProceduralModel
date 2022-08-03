using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DrawWall2 : MonoBehaviour
{
    // 是否实时更新mesh
    public bool updateMesh = false;
    // 生成物体的材质
    public Material mat;

    private GameObject _targetObj;
    private MeshFilter _targetMeshFilter;
    private List<Vector3> posList = new List<Vector3>();

    private void Start()
    {
        
    }

    public void CreateBuildingObject(List<Vector3> polygon,int floors,float height)
    {
        posList = polygon;

        // 创建物体
        if (_targetObj) Destroy(_targetObj);
        _targetObj = new GameObject("Target");

        // 网格 
        _targetMeshFilter = _targetObj.AddComponent<MeshFilter>();
        //解决阴影有缝隙
        _targetObj.AddComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.TwoSided;
        _targetMeshFilter.mesh = MeshGenerator.GenBuildingSubmeshs(posList, height, floors);
        

        // 材质
        if (mat)
        {
            Material[] mats = new Material[_targetMeshFilter.mesh.subMeshCount];
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i] = mat;
            }
            _targetObj.GetComponent<Renderer>().materials = mats;
        }
        else
        {
            Material[] mats = new Material[_targetMeshFilter.mesh.subMeshCount];
            Material diffuse = new Material(Shader.Find("Diffuse"));
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i] = diffuse;
            }
            _targetObj.GetComponent<Renderer>().materials = mats;
        }
    }
}