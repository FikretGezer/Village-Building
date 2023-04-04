using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class hoverOverText : MonoBehaviour,IPointerEnterHandler/*,IPointerExitHandler*/
{
    [SerializeField] Image image;
    [SerializeField] TMP_Text textWhenHover;
    [SerializeField] float yValue;

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        
        textWhenHover.gameObject.SetActive(true);
    }
    private void Update()
    {
        textWhenHover.transform.position = transform.position + new Vector3(0, yValue);
    }
    //void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    //{
    //    textWhenHover.gameObject.SetActive(false);
    //}
}
