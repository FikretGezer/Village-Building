using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkHolder : MonoBehaviour
{
    [SerializeField] float heightOfMark;
    [SerializeField] Image icons;
    MarkOnObject markOnObject;
    Camera cam;
    private void Start()
    {
        cam = Camera.main;
    }
    public void SetMark(MarkOnObject markOnObject)
    {
        this.markOnObject = markOnObject;
    }
    private void Update()
    {
        if(markOnObject.tag=="tree")
            transform.position = cam.WorldToScreenPoint(markOnObject.transform.position+Vector3.up*6f);
        else if(markOnObject.tag=="rock")
            transform.position = cam.WorldToScreenPoint(markOnObject.transform.position + Vector3.up * 2f);
    }
}
