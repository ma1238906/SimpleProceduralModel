using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragableObjectIcon : MonoBehaviour
{
    public Transform target;
    private Canvas can;
    private void Start()
    {
        can = transform.root.GetComponent<Canvas>();
    }

    void Update()
    {
        World2UI(target.position);
    }

    private void World2UI(Vector3 wPos)
    {
        Vector3 dir = wPos - Camera.main.transform.position;
        float dot = Vector3.Dot(dir, Camera.main.transform.forward);
        if (dot < 0)
        {
            GetComponent<RectTransform>().anchoredPosition = new Vector2(10000, 10000);
            return;
        }
        Vector2 tempV2 = Vector2.zero;
        Vector3 spos = Camera.main.WorldToScreenPoint(wPos);
        tempV2.Set(spos.x, spos.y);
        Vector2 rePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(can.GetComponent<RectTransform>(),
        tempV2, can.worldCamera, out rePos);
        rePos += Vector2.up;
        GetComponent<RectTransform>().anchoredPosition = rePos;
    }
}
