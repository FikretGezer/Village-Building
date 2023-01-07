using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public static Action OnTimeChanged;
    public static int year { get; private set; }
    public static int month { get; private set; }
    public static int day { get; private set; }
    public static int hour { get; private set; }
    public static int minute { get; private set; }

    [SerializeField] float minuteInGame = .001f;
    float timer;

    int[,] daysInAMonthArray = {{1, 31}, {2, 28}, {3, 31}, {4, 30}, {5, 31}, {6, 30}, {7, 31}, {8, 31}, {9, 30}, {10, 31}, {11, 30}, {12, 31}};
    void Awake()
    {
        minute = 0;
        hour = 7;
        day = 1;
        month = 1;
        year = 2000;
        timer = minuteInGame;
        daysInAMonthArray[1, 1] = year % 4==0 ? 29 : 28;
    }

    // Update is called once per frame
    void Update()
    {
        ElapseTime();
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
                            daysInAMonthArray[1, 1] = year % 4 == 0 ? 29 : 28; ;
                        }
                    }
                }
            }
            timer = minuteInGame;
            OnTimeChanged?.Invoke();
        }
    }
}
