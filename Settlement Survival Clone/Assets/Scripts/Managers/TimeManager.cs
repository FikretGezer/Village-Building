using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public static Action OnTimeChanged;
    public static Action OnPopUpChanged;
    public static int year { get; private set; }
    public static int yearCount { get; private set; }
    public static int month { get; private set; }
    public static int day { get; private set; }
    public static int hour { get; private set; }
    public static int minute { get; private set; }

    [SerializeField] float minuteInGame = .05f;
    [SerializeField] float minutePerSecond = 1f;
    [SerializeField] float aMonthForAmountOfMinutes = 99f;
    //[SerializeField] TMP_Text popUpText;

    //float popTextShowTime = 50f;//10'un katlarýný yapzmamýn sebebi 10x hýzda 5,10,.. sn sürsün diye.
    float timer;
    int[,] daysInAMonthArray = {{1, 31}, {2, 28}, {3, 31}, {4, 30}, {5, 31}, {6, 30}, {7, 31}, {8, 31}, {9, 30}, {10, 31}, {11, 30}, {12, 31}};
    

    //Queue<string> popUp = new Queue<string>(); 

    void Awake()
    {
        minute = 0;
        hour = 7;
        day = 1;
        month = 1;
        year = 2000;
        yearCount = 1;
        timer = minuteInGame;
        daysInAMonthArray[1, 1] = year % 4==0 ? 29 : 28;
        UiManager.aMonthForAmountOfMinutes = aMonthForAmountOfMinutes;
    }
    void Update()
    {
        //ElapseTime();      
        DateCalculator();
    }
    void DateCalculator()
    {
        timer -= Time.deltaTime;
        if(timer<=0)
        {
            minute++;
            if (minute>aMonthForAmountOfMinutes)
            {
                minute = 0;
                month++;
                for (int i = 0; i < TimeUI.monthsText.Length; i++)
                {
                    if (month == daysInAMonthArray[i, 0])
                    {
                        TimeUI.currentMonth = TimeUI.monthsText[i];
                        UiManager.popUp.Enqueue(TimeUI.currentMonth);
                    }
                }
                //StartCoroutine(CountDown());
                UiManager.popUpMonth = true;
                if(month>12)
                {
                    month = 1;
                    yearCount++;
                }
            }
            timer = minutePerSecond;
            OnTimeChanged?.Invoke();
        }
    }
    void ElapseTime()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            minute++;
            if (minute >= 60)
            {
                minute = 0;
                hour++;
                if (hour >= 24)
                {
                    hour = 0;
                    day++;
                    if (day > daysInAMonthArray[month - 1, 1])
                    {
                        day = 1;
                        month++;
                        if (month > 12)
                        {
                            month = 1;
                            year++;
                            yearCount++;
                            daysInAMonthArray[1, 1] = year % 4 == 0 ? 29 : 28; ;
                        }
                    }
                }
            }
            timer = minuteInGame;
            OnTimeChanged?.Invoke();   
        }
    }
    //void ShowPopUp(string text)
    //{
    //    popUpText.transform.parent.gameObject.SetActive(true);
    //    popUpText.text = $"{text} arrived.";
       
    //}
    //IEnumerator CountDown()
    //{
    //    ShowPopUp(popUp.Dequeue());
    //    yield return new WaitForSeconds(popTextShowTime);
    //    popUpText.transform.parent.gameObject.SetActive(false);
    //    popUp.Clear();
    //    StopCoroutine(CountDown());
    //}
}
