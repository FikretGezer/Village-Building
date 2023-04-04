using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiManager : MonoBehaviour
{
    [SerializeField] Image img;
    [SerializeField] TextMeshProUGUI speedText;

    [SerializeField] TMP_Text popUpNewTech;
    [SerializeField] TMP_Text popUpMonthText;

    [SerializeField] TMP_Text woodCountText;
    [SerializeField] TMP_Text rockCountText;

    [SerializeField] TMP_Text peopleCount;

    public static int woodCount, rockCount;

    public static bool popUpMonth;
    bool newTechArrived;
    public static Queue<string> popUp = new Queue<string>();
    public static float aMonthForAmountOfMinutes;
    public static Action<int> OnResourceUpdate = delegate { };
    public static string NameOfCountedObject="";

    float popTextShowTime = 50f;//10'un katlar�n� yapzmam�n sebebi 10x h�zda 5,10,.. sn s�rs�n diye.
    float perSecondInGame = 1f;
    float fillAmount;

    private void OnEnable()
    {
        OnResourceUpdate += UpdateResourceCount;
    }
    private void OnDisable()
    {
        OnResourceUpdate -= UpdateResourceCount;
    }

    void Start()
    {
        woodCount = rockCount = 1000;
        img.fillAmount = 0;
        fillAmount = 1 / (3 * aMonthForAmountOfMinutes);
    }

    void Update()
    {
        peopleCount.text = (GameObject.FindGameObjectsWithTag("Player").Length).ToString()+"/0/0";
        woodCountText.text = woodCount.ToString();
        rockCountText.text = rockCount.ToString();

        NewTechUpdate();
        if(popUpMonth)
        {
            StartCoroutine(CountDown(popUpMonthText));
            popUpMonth = false;
        }
        if(newTechArrived)
        {
            StartCoroutine(CountDown(popUpNewTech));
            newTechArrived = false;
        }
    }
    void NewTechUpdate()
    {
        perSecondInGame -= Time.deltaTime;
        if (perSecondInGame <= 0)
        {            
            img.fillAmount += fillAmount;
            if (img.fillAmount >= 1f)
            {
                img.fillAmount = 0f;
                popUp.Enqueue("New Tech");
                newTechArrived = true;
            }
                
            perSecondInGame = 1f;
        }
        //img.fillAmount += (0.00003f*timeMultiplier)*Time.deltaTime;//bir ayda 43200 dakika oldu�u i�in fillAmount alabilece�i maks. de�er olan 1'i 43200'e b�l�nce bu de�er ��k�yor.2 de yar�m saniye 1 dk ge�ti�i i�in
        //speedText.text = ((int)Time.timeScale).ToString() + "x";
    }
    void ShowPopUp(string text,TMP_Text tmp)
    {
        tmp.transform.parent.gameObject.SetActive(true);
        tmp.text = $"{text} arrived.";
    }
    IEnumerator CountDown(TMP_Text tmp)
    {
        ShowPopUp(popUp.Dequeue(),tmp);
        yield return new WaitForSeconds(popTextShowTime);
        tmp.transform.parent.gameObject.SetActive(false);
        StopCoroutine(CountDown(tmp));
    }
    private void UpdateResourceCount(int objectCount)
    {
        if(NameOfCountedObject==nameof(woodCount))
        {
            woodCount += objectCount;
            //woodCountText.text = woodCount.ToString();
        }
        if (NameOfCountedObject == nameof(rockCount))
        {
            rockCount += objectCount;
            //rockCountText.text = rockCount.ToString();
        }
    }
}
/*
 * Butonlar de�i�ecek.
 * - A�a� ve maden se�me ayr� olacak. Simgeleri zaten var.
 * - Bir de toplu olarak se�ilenleri kald�rma olacak yani sondaki k�rm�z� buton simgesi de�i�ecek.
 * - Obje se�ildi�inde se�ilen objenin materiali daha se�kin olsun diye alphas� d���k d�z bir renge d�n��t�r�lebilir.
 * 
 */
