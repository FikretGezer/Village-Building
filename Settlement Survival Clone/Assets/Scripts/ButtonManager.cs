using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] GameObject[] objectsMenu;
    [SerializeField] TMP_Text mainTownName;
    [SerializeField] Sprite[] playButtonsForSpeed;

    int[] dereceler = { 1, 3, 5 };
    int i = 0;
    float lastTimeScale;
    
    private void Update()
    {
        EnableMenuWithShortcuts(KeyCode.Alpha1, objectsMenu[0]);
    }
    void EnableMenuWithShortcuts(KeyCode keyCode, GameObject objectMenu)
    {
        if (Input.GetKeyDown(keyCode))
            BtnEnableObjectMenu(objectMenu);
    }
    public void BtnEnableResources(GameObject obj)
    {
        obj.SetActive(obj.activeSelf ? false : true);
    }
    public void BtnEnableObjectMenu(GameObject objectsMenu)
    {
        objectsMenu.transform.GetChild(0).GetComponent<Button>().Select();
        objectsMenu.SetActive(objectsMenu.activeSelf?false:true);
    }
    public void BtnLogOut()
    {
        Application.Quit();
    }
    public void BtnTownNameChanger(TMP_Text text)
    {
        mainTownName.text = text.text;
    }

    #region Speed Buttons
    public void BtnIncreaseSpeed()
    {
        if(Time.timeScale<10)
        {
            if(Time.timeScale!=0)
            {
                if (Time.timeScale + 5 <= 10)
                {
                    Time.timeScale += dereceler[i];
                    i++;
                }
                text.text = Time.timeScale.ToString()+"x";
            }        
        }
    }
    public void BtnDecreaseSpeed()
    {
        if(Time.timeScale>1)
        {
            if (Time.timeScale - 1 >= 1)
            {
                i--;
                Time.timeScale -= dereceler[i];              
            }
            text.text = Time.timeScale.ToString()+"x";
        }              
    }
    public void BtnStopSpeed(Image image)
    {
        if (Time.timeScale == 0)
        {
            image.sprite = playButtonsForSpeed[1];
            Time.timeScale = lastTimeScale;           
        }
           
        else
        {
            image.sprite = playButtonsForSpeed[0];
            lastTimeScale = Time.timeScale;
            Time.timeScale = 0;
            
        }
    }
    #endregion
}
