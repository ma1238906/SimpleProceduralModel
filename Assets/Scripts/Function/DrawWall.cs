using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
/// <summary>
/// 点选角点生成墙，左键添加点，右键生成
/// </summary>
public class DrawWall : MonoBehaviour
{
    public float WallHeight = 3.5f;
    public List<Vector3> WallWayPoint;
    private void Start() {
        WallWayPoint = new List<Vector3>();
    }

    private void Update() {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray,out hit) && hit.collider.tag == "Ground")
            {
                WallWayPoint.Add(hit.point);
            }
        }

        if(Input.GetMouseButtonDown(1))
        {
            CreatWallMesh(WallWayPoint);
        }
    }
    private void CreatWallMesh(List<Vector3> wallWay)
    {
        if(wallWay.Count <3)return;
        for(int i=0;i<wallWay.Count-1;i++)
        {
            CreateSingleWall(wallWay[i],wallWay[i+1]);
            CreateReverseWall(wallWay[i],wallWay[i+1]);
        }
        CreateSingleWall(wallWay[wallWay.Count-1],wallWay[0]);
        CreateReverseWall(wallWay[wallWay.Count-1],wallWay[0]);
    }

    private void CreateSingleWall(Vector3 start,Vector3 end)
    {
        Mesh mesh = new Mesh();
        Vector3[] vertics = new Vector3[4]
        {
            start,
            start+WallHeight*Vector3.up,
            end+WallHeight*Vector3.up,
            end
        };
        Vector2[] uv = new Vector2[4]
        {
            new Vector2(0.0f, 0.0f),
            new Vector2(0.0f, 1.0f),
            new Vector2(1.0f, 1.0f),
            new Vector2(1.0f, 0.0f)
        };
        Vector3[] normal = new Vector3[4]
        {
            new Vector3(0, 0, 1),
            new Vector3(0, 0, 1),
            new Vector3(0, 0, 1),
            new Vector3(0, 0, 1)
        };
        int[] triangles = new int[]
        {
            0, 1, 3,
            1, 2, 3
        };
        mesh.vertices = vertics;
        mesh.uv = uv;
        mesh.normals = normal;
        mesh.triangles = triangles;
        GameObject singleWall = Resources.Load<GameObject>("Prefabs/WallPrefab");
        Instantiate(singleWall).GetComponent<MeshFilter>().mesh = mesh;
    }

    private void CreateReverseWall(Vector3 start,Vector3 end)
    {
        Mesh mesh = new Mesh();
        Vector3[] vertics = new Vector3[4]
        {
            start,
            start+WallHeight*Vector3.up,
            end+WallHeight*Vector3.up,
            end
        };
        Vector2[] uv = new Vector2[4]
        {
            new Vector2(0.0f, 0.0f),
            new Vector2(0.0f, 1.0f),
            new Vector2(1.0f, 1.0f),
            new Vector2(1.0f, 0.0f)
        };
        Vector3[] normal = new Vector3[4]
        {
            new Vector3(0, 0, 1),
            new Vector3(0, 0, 1),
            new Vector3(0, 0, 1),
            new Vector3(0, 0, 1)
        };
        int[] triangles = new int[]
        {
            3, 1, 0,
            3, 2, 1
        };
        mesh.vertices = vertics;
        mesh.uv = uv;
        mesh.normals = normal;
        mesh.triangles = triangles;
        GameObject singleWall = Resources.Load<GameObject>("Prefabs/WallPrefab");
        Instantiate(singleWall).GetComponent<MeshFilter>().mesh = mesh;
    }

    private void CreateRoof(List<Vector3> wallWay)
    {

    }
}
