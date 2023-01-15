using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class popUpExe : MonoBehaviour
{
    Queue<string> popUpQ=new Queue<string>();
    public TMP_Text popUpText;
    public void AddQueue(string text)
    {
        //text = TimeUI.currentMonth;
        popUpQ.Enqueue(text);
        Deneme();
    }
    void ShowPopUp(string text)
    {
        popUpText.text = text;
    }
    public void Deneme()
    {
        ShowPopUp(popUpQ.Dequeue());
    }
}
