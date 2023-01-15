using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSelectionScript : MonoBehaviour
{
    [SerializeField] List<GameObject> unitList = new List<GameObject>();
    [SerializeField] RectTransform boxVisual;

    Dictionary<GameObject, Color> unitColor = new Dictionary<GameObject, Color>();

    Camera cam;
    Rect boxSelection;
    Vector2 startPos, endPos;
    private void Start()
    {
        unitList.AddRange(GameObject.FindGameObjectsWithTag("Player"));

        foreach (var unit in unitList)
        {
            if(!unitColor.ContainsKey(unit))
            {
                unitColor[unit] = unit.GetComponent<Renderer>().material.color;
            }
        }
        cam = Camera.main;
        startPos = Vector2.zero;
        endPos = Vector2.zero;
        boxVisual.sizeDelta = Vector2.zero;
    }
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
        }
        if(Input.GetMouseButton(0))
        {
            endPos = Input.mousePosition;
            DrawVisual();
            DrawSelection();
        }
        if(Input.GetMouseButtonUp(0))
        {
            SelectObjects();
            startPos = Vector2.zero;
            endPos = Vector2.zero;
            DrawVisual();
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
        foreach (var unit in unitList)
        {
            if(boxSelection.Contains(cam.WorldToScreenPoint(unit.transform.position)))
            {
                 unit.GetComponent<Renderer>().material.color = Color.green;
            }
            else
            {
                if(unitColor.ContainsKey(unit))
                {
                    unit.GetComponent<Renderer>().material.color = unitColor[unit];
                }
            }
        }
    }
}
