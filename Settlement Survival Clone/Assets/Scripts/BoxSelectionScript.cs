using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSelectionScript : MonoBehaviour
{
    [SerializeField] public static List<GameObject> objs = new List<GameObject>();
    [SerializeField] RectTransform boxVisual;

    Camera cam;
    Rect boxSelection;
    Vector2 startPos, endPos;
    private void Start()
    {
        cam = Camera.main;
        startPos = Vector2.zero;
        endPos = Vector2.zero;
        boxVisual.sizeDelta = Vector2.zero;
    }
    private void Update()
    {
        if(boxVisual.gameObject.activeSelf)
        {
            if (Input.GetMouseButtonDown(0))
            {
                startPos = Input.mousePosition;
            }
            if (Input.GetMouseButton(0))
            {
                endPos = Input.mousePosition;
                DrawVisual();
                DrawSelection();
            }
            if (Input.GetMouseButtonUp(0))
            {
                SelectObjects();
                startPos = Vector2.zero;
                endPos = Vector2.zero;
                DrawVisual();
            }
        }      
    }
    void DrawVisual()
    {
        Vector2 boxStart = startPos;
        Vector2 boxEnd = endPos;

        Vector2 center = (boxStart + boxEnd) / 2;     
        Vector2 boxSize = new Vector2(Mathf.Abs(boxStart.x - boxEnd.x), Mathf.Abs(boxStart.y - boxEnd.y));

        boxVisual.position = center;
        boxVisual.sizeDelta = boxSize;
    }

    void DrawSelection()
    {
        if(Input.mousePosition.x<startPos.x)
        {
            boxSelection.xMin = Input.mousePosition.x;
            boxSelection.xMax = startPos.x;
        }
        else
        {
            boxSelection.xMin = startPos.x;
            boxSelection.xMax = Input.mousePosition.x;
        }
        if(Input.mousePosition.y<startPos.y)
        {
            boxSelection.yMin = Input.mousePosition.y;
            boxSelection.yMax = startPos.y;
        }
        else
        {
            boxSelection.yMin = startPos.y;
            boxSelection.yMax = Input.mousePosition.y;
        }
    }
    void SelectObjects()
    {
        foreach(var obj in objs)
        {
            if(boxSelection.Contains(cam.WorldToScreenPoint(obj.transform.position)))
            {
                MarkController.marks[obj.GetComponentInChildren<MarkOnObject>()].gameObject.SetActive(true);
            }
            if (boxSelection.Contains(cam.WorldToScreenPoint(obj.transform.position)) && ButtonManager.axeName== "Remove Pickable")
            {
                MarkController.marks[obj.GetComponentInChildren<MarkOnObject>()].gameObject.SetActive(false);
            }
        }
    }
}
