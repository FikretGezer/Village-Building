using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkController : MonoBehaviour
{
    [SerializeField] MarkHolder markPrefab;
    [SerializeField] Sprite iconAxe, iconPickaxe;
    public static Dictionary<MarkOnObject, MarkHolder> marks = new Dictionary<MarkOnObject, MarkHolder>();

    private void Awake()
    {
        MarkOnObject.OnMarkAdded += AddMark;
        MarkOnObject.OnMarkRemoved += RemoveMark;
    }

    void AddMark(MarkOnObject obj)
    {
        if(!marks.ContainsKey(obj))
        {
            var mark = Instantiate(markPrefab, transform);
            if(obj.tag=="tree")
            {
                mark.gameObject.GetComponent<Image>().sprite = iconAxe;
                mark.name = "Mark_Tree";
            }              
            else
            {
                mark.gameObject.GetComponent<Image>().sprite = iconPickaxe;
                mark.name = "Mark_Rock";
            }
       
            marks.Add(obj, mark);
            mark.SetMark(obj);
        }
    }
    void RemoveMark(MarkOnObject obj)
    {
        if(marks.ContainsKey(obj))
        {
            //if (marks[obj] != null)
            //    Destroy(marks[obj].gameObject);
            //if (marks[obj] == null)
            //    marks.Remove(obj);
            if (marks[obj] != null)
                marks[obj].gameObject.SetActive(false);
        }
    }
}
