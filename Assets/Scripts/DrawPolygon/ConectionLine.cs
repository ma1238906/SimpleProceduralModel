using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConectionLine : MonoBehaviour
{
    private DragableObject dragableObject;

    private void Start() {
        dragableObject = transform.parent.GetComponent<DragableObject>();
    }
    private void OnMouseUpAsButton() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits;
        hits = Physics.RaycastAll(ray);
        for(int i=0;i<hits.Length;i++)
        {
            if(hits[i].collider.tag == "Ground")
            {
                GetComponentInParent<DrawPolygon>().AddControlPoint(hits[i].point,dragableObject);
            }
        }
    }
}
