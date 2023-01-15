using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI DateAndTimeText;
    [SerializeField] bool changeDateHierarchy;//Changes date hierarchy between dd/mm/yy and mm/dd//yy

    public static string[] monthsText = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
    int[] monthDay = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
    public static string currentMonth;

    private void Awake()
    {
        currentMonth = monthsText[0];
    }
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
        //DateAndTimeText.text = $"Year {TimeManager.yearCount} / {currentMonth}, {TimeManager.hour:00}:{TimeManager.minute:00}";
        DateAndTimeText.text = $"Year {TimeManager.yearCount} / {currentMonth}";
       // Debug.Log($"{TimeManager.minute:000}");
        //if (!changeDateHierarchy)
        //    DateAndTimeText.text = $"Date: {TimeManager.day:00}/{TimeManager.month:00}/{TimeManager.year:0000}," +
        //                          $" Time: {TimeManager.hour:00}:{TimeManager.minute:00}";
        //else
        //    DateAndTimeText.text = $"Date: {TimeManager.month:00}/{TimeManager.day:00}/{TimeManager.year:0000}," +
        //                          $" Time: {TimeManager.hour:00}:{TimeManager.minute:00}";
    }
   
}
