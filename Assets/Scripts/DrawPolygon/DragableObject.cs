using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragableObject : MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 oriPoint;
    private Vector3 offset;
    [SerializeField]
    private DragableObject nextPoint;
    private LineRenderer lineRenderer;
    private CapsuleCollider lineCollider;
     
    public void Init(DragableObject point)
    {
        this.nextPoint = point;
        lineRenderer = transform.Find("Line").GetComponent<LineRenderer>();
        lineCollider = transform.Find("Line").GetComponent<CapsuleCollider>();
    }

    public void UpdateNextPoint(DragableObject point)
    {
        this.nextPoint = point;
    }

    public DragableObject GetNextPoint()
    {
        return this.nextPoint;
    }

    public void UpdateLine()
    {
        lineRenderer.SetPosition(0,transform.position);
        lineRenderer.SetPosition(1,nextPoint.transform.position);
        SetCollider();
    }

    void OnMouseDown()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hitInfo = Physics.RaycastAll(ray);
        for (int i = 0; i < hitInfo.Length; i++)
        {
            if (hitInfo[i].collider.tag == "Ground")
            {
                screenPoint = hitInfo[i].point;
                oriPoint = transform.position;
            }
        }
    }
     
    void OnMouseDrag()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hitInfo = Physics.RaycastAll(ray);
        for (int i = 0; i < hitInfo.Length; i++)
        {
            if (hitInfo[i].collider.tag == "Ground")
            {
                offset = hitInfo[i].point - screenPoint;
            }
        }
        transform.position = oriPoint + offset;
    }

    void SetCollider()
    {
        lineCollider.direction = 2;
        lineCollider.radius = 0.1f;
        lineCollider.center = Vector3.zero;
        lineCollider.transform.position = transform.position + (nextPoint.transform.position - transform.position) / 2;
        lineCollider.transform.LookAt(transform.position);
        lineCollider.height = (nextPoint.transform.position - transform.position).magnitude;
    }
}
