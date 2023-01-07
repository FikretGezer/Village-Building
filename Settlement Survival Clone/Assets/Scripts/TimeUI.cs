using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI DateAndTimeText;
    [SerializeField] bool changeDateHierarchy;//Changes date hierarchy between dd/mm/yy and mm/dd//yy
    private void OnEnable()
    {
        TimeManager.OnTimeChanged += UpdateDateAndTime;
    }
    private void OnDisable()
    {
        TimeManager.OnTimeChanged -= UpdateDateAndTime;
    }
    void UpdateDateAndTime()
    {
        if(!changeDateHierarchy)
            DateAndTimeText.text= $"Date: {TimeManager.day:00}/{TimeManager.month:00}/{TimeManager.year:0000}," +
                                  $" Time: {TimeManager.hour:00}:{TimeManager.minute:00}";
        else
            DateAndTimeText.text = $"Date: {TimeManager.month:00}/{TimeManager.day:00}/{TimeManager.year:0000}," +
                                  $" Time: {TimeManager.hour:00}:{TimeManager.minute:00}";
    }
}
