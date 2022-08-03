using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshUtility : MonoBehaviour
{
    Mesh _mesh;
    Mesh mesh { get { if (_mesh == null) _mesh = GetComponent<MeshFilter>().sharedMesh;return _mesh; } }

    //绘制物体网格顶点位置和顶点索引
    private void OnDrawGizmosSelected()
    {
        Matrix4x4 matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        for (int i=0;i<mesh.vertices.Length;i++)
        {
            Vector3 tm3 = matrix * mesh.vertices[i];
            Gizmos.DrawSphere(tm3+transform.position, 0.01f);
            UnityEditor.Handles.Label(tm3 + transform.position, i.ToString());
        }
    }
}