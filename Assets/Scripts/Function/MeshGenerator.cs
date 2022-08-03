using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolygonTool;
using System.Linq;

public class MeshGenerator
{
    public static int[] PolygonToTriangles(List<Vector3> polygonVertex, bool filpTriangle = true)
    {
        // 存结果的列表
        var resultVertexes = new List<Vector3>();
        
        // 转化器
        var triangulation = new Triangulation(polygonVertex);
        
        // 设置比较轴
        triangulation.SetCompareAxle(CompareAxle.Y);

        // 取得三角形
        var triangles = triangulation.GetTriangles();

        // 反转三角形
        int tempInt;
        if (filpTriangle)
        {
            for (int i = 0; i < triangles.Length; i+=3)
            {
                // 交换两个顶点的顺序令其三角形的顺序相反
                tempInt = triangles[i + 1];
                triangles[i + 1] = triangles[i + 2];
                triangles[i + 2] = tempInt;
            }
        }
        
        return triangles;
    }

    //生成多边形
    public static Mesh GenPolyMesh(List<Vector3> polyVerts)
    {
        var resMesh = new Mesh();

        // 把多边形数据转换为三角形
        var triangles = PolygonToTriangles(polyVerts);

        // 设置顶点
        resMesh.vertices = polyVerts.ToArray();
        // 设置三角形
        resMesh.triangles = triangles;
        // 计算法线
        resMesh.RecalculateNormals();
        
        return resMesh;
    }

    //生成
    public static Mesh GenBuildingMeshSimple(List<Vector3> polyVerts, float height)
    {
        var resMesh = new Mesh();

        // verts作为存放最终顶点的容器
        var verts = polyVerts;
        
        // 计算顶层顶点
        var upperVerts = new List<Vector3>();
        for (var i = 0; i < polyVerts.Count; i++)
        {
            upperVerts.Add(polyVerts[i] + Vector3.up * height);
        }

        // 计算顶层三角形
        var upperTriangle = PolygonToTriangles(upperVerts);
        // 给每一个顶点索引加上长度
        /* 因为加上顶层顶点之后，相应的索引应该延后 */
        for (var i = 0; i < upperTriangle.Length; i++)
        {
            upperTriangle[i] += polyVerts.Count;
        }
        
        // 计算墙壁的三角形
        var wallTriangle = new List<int>();
        int j;
        for (var i = 0; i < verts.Count; i++)
        {
            // 四边形中第一个三角形
            wallTriangle.Add(i);    
            wallTriangle.Add(i + verts.Count);
            wallTriangle.Add(i + 1);

            // 计算四边形中第二个三角形
            j = (i + 1) % verts.Count;
            wallTriangle.Add(j);    
            wallTriangle.Add(j + verts.Count - 1);
            wallTriangle.Add(j + verts.Count);
        }
        
        // 将顶点相加
        verts.AddRange(upperVerts);
        // 将三角相加
        var triangleList = wallTriangle;
        triangleList.AddRange(upperTriangle);

        // 设置顶点
        resMesh.vertices = verts.ToArray();
        // 设置三角形 
        resMesh.triangles = triangleList.ToArray();
        // 计算法线
        resMesh.RecalculateNormals();
        
        return resMesh;
    }

    public static Mesh GenBuildingMesh(List<Vector3> polyVerts, float height,int floors=1)
    {
        // 结果Mesh
        var resMesh = new Mesh();
        
        // 所有顶点的集合
        var allVerts = new List<Vector3>();
        var upperVerts = new List<Vector3>();
        var bottomVerts = new List<Vector3>();

        // 所有法线的集合
        var allNormals = new List<Vector3>();
        var upperNormals = new List<Vector3>();
        var bottomNormals = new List<Vector3>();

        // 所有三角形的集合
        var allTriangles = new List<int>();
        var upperTriangles = new List<int>();
        var bottomTriangles = new List<int>();

        //所有uv的集合
        var allUVs = new List<Vector2>();
        var upperUVs = new List<Vector2>();
        var bottomUVs = new List<Vector2>();

        // 计算顶部的顶点位置
        for (var i = 0; i < polyVerts.Count; i++)
            upperVerts.Add(polyVerts[i] + Vector3.up * height);

        //计算底部的顶点位置
        for(var i = 0; i < polyVerts.Count; i++)
            bottomVerts.Add(polyVerts[i]);
        
        // 计算墙壁的顶点、法线、三角
        var wallVerts = new List<Vector3>();
        var wallNormals = new List<Vector3>();
        var wallTriangles = new List<int>();
        var wallUVs = new List<Vector2>();
        // 遍历一边多边形的所有顶点
        var counter = 0;
        for (var i = 0; i < polyVerts.Count; i++)
        {
            // 先添加这个面的四个顶点(顺时针,左下角为第一个顶点)
            wallVerts.Add(polyVerts[i]);
            wallVerts.Add(upperVerts[i]);
            wallVerts.Add(upperVerts[(i + 1) % polyVerts.Count]);
            wallVerts.Add(polyVerts[(i + 1) % polyVerts.Count]);
            
            // 利用两个向量差乘计算法线
            var normal = Vector3.Cross(upperVerts[i] - polyVerts[i], polyVerts[(i + 1) % polyVerts.Count] - polyVerts[i]).normalized;
            wallNormals.Add(normal);
            wallNormals.Add(normal);
            wallNormals.Add(normal);
            wallNormals.Add(normal);
            
            // 计算三角
            // 第一个三角
            wallTriangles.Add(counter);
            wallTriangles.Add(counter + 1);
            wallTriangles.Add(counter + 2);
            // 第二个三角
            wallTriangles.Add(counter);
            wallTriangles.Add(counter + 2);
            wallTriangles.Add(counter + 3);

            //计算uv
            wallUVs.Add(Vector2.zero);
            wallUVs.Add(Vector2.zero + Vector2.up*floors);
            wallUVs.Add(Vector2.one+Vector2.up*(floors-1));
            wallUVs.Add(Vector2.zero + Vector2.right);

            // 自增
            counter += 4;
        }
        
        // 计算顶部的顶点、法线、三角
        // 法线
        for (var i = 0; i < upperVerts.Count; i++)
            upperNormals.Add(Vector3.up);
        // 三角
        upperTriangles = PolygonToTriangles(upperVerts).ToList();
        // 顺延三角的索引
        for (var i = 0; i < upperTriangles.Count; i++)
        {
            upperTriangles[i] += wallVerts.Count;
        }


        //计算底部的顶点、法线、三角
        //法线
        for (var i = 0; i < bottomVerts.Count; i++)
            bottomNormals.Add(Vector3.down);
        //三角
        bottomTriangles = PolygonToTriangles(bottomVerts).ToList();
        for(var i = 0;i<bottomTriangles.Count;i++)
        {
            bottomTriangles[i] += wallVerts.Count+upperVerts.Count;
        }
        bottomTriangles.Reverse();

        //计算顶部uv 暂时用0，0,以后可以用顶点在aabb包围盒中的位置确定
        for (var i = 0; i < upperVerts.Count; i++)
            upperUVs.Add(Vector2.zero);
        //计算底部uv 暂时用0，0 以后可以用顶点在aabb包围盒中的位置确定
        for (var i = 0;i<bottomVerts.Count;i++)
        {
            bottomUVs.Add(Vector2.zero);
        }

        // 合并数据
        allVerts.AddRange(wallVerts);
        allVerts.AddRange(upperVerts);
        allVerts.AddRange(bottomVerts);
        allNormals.AddRange(wallNormals);
        allNormals.AddRange(upperNormals);
        allNormals.AddRange(bottomNormals);
        allTriangles.AddRange(wallTriangles);
        allTriangles.AddRange(upperTriangles);
        allTriangles.AddRange(bottomTriangles);
        allUVs.AddRange(wallUVs);
        allUVs.AddRange(upperUVs);
        allUVs.AddRange(bottomUVs);
        
        // 设置顶点
        resMesh.vertices = allVerts.ToArray();
        // 设置三角形 
        resMesh.triangles = allTriangles.ToArray();
        // 设置法线
        resMesh.normals = allNormals.ToArray();
        // 设置uv
        resMesh.uv = allUVs.ToArray();

        return resMesh;
    }

    public static Mesh GenBuildingSubmeshs(List<Vector3> polyVerts, float height, int floors = 1)
    {
        Mesh resMesh = new Mesh();
        var buildingVerts = new List<Vector3>();
        var buildingNormals = new List<Vector3>();
        var buildingTriangles = new List<int>();
        var buildingUVs = new List<Vector2>();

        MeshDate bottomData = GetBottomMesh(polyVerts);
        MeshDate wallData = GetWallMesh(polyVerts, height, floors);
        MeshDate topData = GetTopMesh(polyVerts, height);

        buildingVerts.AddRange(bottomData.Vertices);
        buildingVerts.AddRange(wallData.Vertices);
        buildingVerts.AddRange(topData.Vertices);
        
        for(int i=0;i<wallData.Traiangles.Count;i++)
        {
            wallData.Traiangles[i] += bottomData.Vertices.Count;
        }
        for(int i=0;i<topData.Traiangles.Count;i++)
        {
            topData.Traiangles[i] += bottomData.Vertices.Count + wallData.Vertices.Count;
        }
        buildingTriangles.AddRange(bottomData.Traiangles);
        buildingTriangles.AddRange(wallData.Traiangles);
        buildingTriangles.AddRange(topData.Traiangles);

        buildingNormals.AddRange(bottomData.Normals);
        buildingNormals.AddRange(wallData.Normals);
        buildingNormals.AddRange(topData.Normals);

        buildingUVs.AddRange(bottomData.UVs);
        buildingUVs.AddRange(wallData.UVs);
        buildingUVs.AddRange(topData.UVs);

        resMesh.vertices = buildingVerts.ToArray();
        resMesh.triangles = buildingTriangles.ToArray();
        resMesh.normals = buildingNormals.ToArray();
        resMesh.uv = buildingUVs.ToArray();

        //使用submesh的方式
        UnityEngine.Rendering.SubMeshDescriptor subM = new UnityEngine.Rendering.SubMeshDescriptor(0, bottomData.Traiangles.Count + wallData.Traiangles.Count);
        UnityEngine.Rendering.SubMeshDescriptor subMt = new UnityEngine.Rendering.SubMeshDescriptor(bottomData.Traiangles.Count + wallData.Traiangles.Count, topData.Traiangles.Count);
        resMesh.subMeshCount = 2;
        resMesh.SetSubMesh(0, subM);
        resMesh.SetSubMesh(1, subMt);

        return resMesh;
    }

    //生成墙体网格
    private static MeshDate GetWallMesh(List<Vector3> polyVerts, float height, int floors = 1)
    {
        MeshDate wallMesh = new MeshDate();
        var wallVerts = new List<Vector3>();
        var wallNormals = new List<Vector3>();
        var wallTriangles = new List<int>();
        var wallUVs = new List<Vector2>();
        // 计算顶部的顶点位置
        var upperVerts = new List<Vector3>();
        for (var i = 0; i < polyVerts.Count; i++)
            upperVerts.Add(polyVerts[i] + Vector3.up * height);

        var counter = 0;
        for(var i = 0;i<polyVerts.Count;i++)
        {
            // 先添加这个面的四个顶点(顺时针,左下角为第一个顶点)
            wallVerts.Add(polyVerts[i]);
            wallVerts.Add(upperVerts[i]);
            wallVerts.Add(upperVerts[(i + 1) % polyVerts.Count]);
            wallVerts.Add(polyVerts[(i + 1) % polyVerts.Count]);

            // 利用两个向量差乘计算法线
            var normal = Vector3.Cross(upperVerts[i] - polyVerts[i], polyVerts[(i + 1) % polyVerts.Count] - polyVerts[i]).normalized;
            wallNormals.Add(normal);
            wallNormals.Add(normal);
            wallNormals.Add(normal);
            wallNormals.Add(normal);

            // 计算三角
            // 第一个三角
            wallTriangles.Add(counter);
            wallTriangles.Add(counter + 1);
            wallTriangles.Add(counter + 2);
            // 第二个三角
            wallTriangles.Add(counter);
            wallTriangles.Add(counter + 2);
            wallTriangles.Add(counter + 3);

            //计算uv
            wallUVs.Add(Vector2.zero);
            wallUVs.Add(Vector2.zero + Vector2.up * floors);
            wallUVs.Add(Vector2.one + Vector2.up * (floors - 1));
            wallUVs.Add(Vector2.zero + Vector2.right);

            counter += 4;
        }
        wallMesh.Vertices = wallVerts;
        wallMesh.Traiangles = wallTriangles;
        wallMesh.Normals = wallNormals;
        wallMesh.UVs = wallUVs;
        return wallMesh;
    }

    //生成底部网格
    private static MeshDate GetBottomMesh(List<Vector3>polyVerts)
    {
        MeshDate bottomMesh = new MeshDate();
        List<Vector3> bottomVerts = new List<Vector3>();
        List<Vector3> bottomNormals = new List<Vector3>();
        List<int> bottomTriangles;
        List<Vector2> bottomUVs = new List<Vector2>();

        bottomVerts = polyVerts;
        bottomTriangles = PolygonToTriangles(bottomVerts).ToList();
        bottomTriangles.Reverse();
        for (int i = 0; i < bottomVerts.Count; i++)
        {
            bottomNormals.Add(Vector3.down); 
        }
        //计算底部uv 暂时用0，0 以后可以用顶点在aabb包围盒中的位置确定
        for (int i = 0; i < bottomVerts.Count; i++)
        {
            bottomUVs.Add(Vector2.zero);
        }
        bottomMesh.Vertices = bottomVerts;
        bottomMesh.Traiangles = bottomTriangles;
        bottomMesh.Normals = bottomNormals;
        bottomMesh.UVs = bottomUVs;
        return bottomMesh;
    }

    //生成顶部网格
    private static MeshDate GetTopMesh(List<Vector3>polyVerts,float height)
    {
        MeshDate topMesh = new MeshDate();
        List<Vector3> topVerts = new List<Vector3>();
        List<Vector3> topNormals = new List<Vector3>();
        List<int> topTraiangles;
        List<Vector2> topUVs = new List<Vector2>();

        for(int i=0;i<polyVerts.Count;i++)
        {
            topVerts.Add(polyVerts[i] + Vector3.up * height);
        }
        topTraiangles = PolygonToTriangles(topVerts).ToList();
        for(int i=0;i<topVerts.Count;i++)
        {
            topNormals.Add(Vector3.up);
        }
        for(int i=0;i<topVerts.Count;i++)
        {
            topUVs.Add(Vector2.zero);
        }
        topMesh.Vertices = topVerts;
        topMesh.Traiangles = topTraiangles;
        topMesh.Normals = topNormals;
        topMesh.UVs = topUVs;
        return topMesh;
    }
}

public class MeshDate
{
    public List<Vector3> Vertices;
    public List<int> Traiangles;
    public List<Vector3> Normals;
    public List<Vector2> UVs;
}