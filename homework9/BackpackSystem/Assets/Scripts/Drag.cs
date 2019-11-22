using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class Drag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Transform trans;
    RectTransform rect_trans;
    CanvasGroup canvas_group;
    GameObject grid = null;
    Color grid_color_1;
    Color grid_color_2 = Color.grey;
    public Vector3 v3;

    void Start()
    {
        // Debug.Log("000");
        trans = this.transform;
        v3 = trans.position;
        rect_trans = this.transform as RectTransform;
        canvas_group = GetComponent<CanvasGroup>();
    }

    void Update()
    {
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Debug.Log("OnBeginDrag");
        canvas_group.blocksRaycasts = false;
        grid = eventData.pointerEnter;
        grid_color_1 = grid.GetComponent<Image>().color;
        v3 = trans.position;
        gameObject.transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Debug.Log("OnDrag");
        bool flag = true;
        Vector3 mouse_position;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rect_trans, eventData.position, eventData.pressEventCamera, out mouse_position))
        {
            rect_trans.position = mouse_position;
        }
        GameObject now_grid_p = eventData.pointerEnter;
        if (now_grid_p == null)
        {
            flag = false;
        }
        else if (now_grid_p.name != "bag_grid")
        {
            flag = false;
        }
        if (flag)
        {
            Image pic = now_grid_p.GetComponent<Image>();
            grid.GetComponent<Image>().color = grid_color_1;
            if (grid != now_grid_p)
            {
                grid.GetComponent<Image>().color = grid_color_1;
                grid = now_grid_p;
            }
            pic.color = grid_color_2;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Debug.Log("OnEndDrag");
        GameObject now_grid_p = eventData.pointerEnter;
        if (now_grid_p == null)
        {
            trans.position = v3;
        }
        else
        {
            if (now_grid_p.name == "bag_grid")
            {
                trans.position = now_grid_p.transform.position;
                v3 = trans.position;
                now_grid_p.GetComponent<Image>().color = grid_color_1;
            }
            else
            {
                if (now_grid_p.name == eventData.pointerDrag.name && now_grid_p != eventData.pointerDrag)
                {
                    Vector3 target_pos = now_grid_p.transform.position;
                    now_grid_p.transform.position = v3;
                    trans.position = target_pos;
                    v3 = trans.position;
                }
                else
                {
                    trans.position = v3;
                }
            }
        }
        grid.GetComponent<Image>().color = grid_color_1;
        canvas_group.blocksRaycasts = true;
    }
}