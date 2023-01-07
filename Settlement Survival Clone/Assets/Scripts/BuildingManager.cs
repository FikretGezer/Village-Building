using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class BuildingManager : MonoBehaviour
{
    [HideInInspector] public static bool isPlaceable;

    [SerializeField] float gridSize;
    [SerializeField] LayerMask ground;
    [SerializeField] GameObject[] spawnObject;
    [SerializeField] GameObject prefab;
    [SerializeField] Material mat_Placeable, mat_NotPlaceable, mat_PlacementOn;
    [SerializeField] GameObject buildingsMenu;
    [SerializeField] public static float planeSize { get; private set; }//if this is 200 plansize is 200*200
    [SerializeField] float planeSizeValue;
    [SerializeField] float restrictAmount;//Alaný her köþeden ne kadar sýnýrlayacaðýný belirtir.

    GameObject prefabCopy;
    Vector3 prefabCopyPos;
    Material matForPrefab;
    
    Dictionary<GameObject, Material> prefabsMatsOnTheScene = new Dictionary<GameObject, Material>();
    bool objPlacementActive;
    float rotateAmount;
    private void Awake()
    {
        planeSize = planeSizeValue;
    }
    private void Update()
    {        
        if(prefabCopy!=null)
        {
            MoveObject();
            RotatePrefab();
            ReleaseObject();
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Destroy(prefabCopy);
                objPlacementActive = false;
            }//Let go object
        }
        //if (Input.GetMouseButtonDown(1) && prefabCopy == null)
        //{
        //    prefabCopy = Instantiate(prefab, prefabCopyPos, Quaternion.identity);

        //    matForPrefab = prefabCopy.GetComponent<Renderer>().sharedMaterial;

        //    objPlacementActive = true;
        //}//Spawn object with right click
        PlacementMaterialChange();
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EnableChildMenu();
        }
    }
    void MoveObject()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, ground))
        {
            prefabCopyPos = new Vector3(
                RoundToNearPosition(hit.point.x),
                hit.point.y,
                RoundToNearPosition(hit.point.z)
                );
            prefabCopyPos = new Vector3(Mathf.Clamp(prefabCopyPos.x, restrictAmount, planeSize-restrictAmount), prefabCopyPos.y, Mathf.Clamp(prefabCopyPos.z, restrictAmount, planeSize-restrictAmount));
            prefabCopy.transform.position = prefabCopyPos;
            prefabCopy.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            ChangeObjectMaterial();
        }//Move object around
    }
    void ReleaseObject()
    {
        if (Input.GetMouseButtonDown(0) && isPlaceable)
        {
            prefabCopy.GetComponent<Renderer>().sharedMaterial = matForPrefab;
            prefabsMatsOnTheScene.Add(prefabCopy, prefabCopy.GetComponent<Renderer>().material);

            if (prefabCopy.transform.childCount > 0)
                prefabCopy.transform.GetChild(0).gameObject.SetActive(true);//If object has an particle effect actives it

            prefabCopy = null;
            objPlacementActive = false;

            AudioManager.Instance.AudioPlay(AudioManager.Instance.audioClips[0]);//Play a sound when a building placed.            
        }//Placed spawn object
    }
    void RotatePrefab()
    {
        if (Input.GetKeyDown(KeyCode.R))
            rotateAmount += 90;
        prefabCopy.transform.Rotate(Vector3.up, rotateAmount);
        //Rotate Object 90 degree
    }
    void PlacementMaterialChange()//Changes material of all objects white on the scene to see placeable object clear
    {
        Material mat=mat_PlacementOn;
        if (prefabsMatsOnTheScene != null)
        {
            foreach (GameObject go in prefabsMatsOnTheScene.Keys)
            {
                if (objPlacementActive)
                {
                    mat = mat_PlacementOn;
                    // go.transform.GetChild(0)?.gameObject.SetActive(false);
                    if (go.transform.childCount > 0)
                        go.transform.GetChild(0).gameObject.SetActive(false);
                }
                else
                {
                    mat = prefabsMatsOnTheScene[go];
                    //go.transform.GetChild(0)?.gameObject.SetActive(true);
                    if (go.transform.childCount > 0)
                        go.transform.GetChild(0).gameObject.SetActive(true);
                }

                go.GetComponent<Renderer>().material = mat;
            }
        }
    }
    void ChangeObjectMaterial()//Change object color if it can placeable or not
    {
        if (isPlaceable)
            prefabCopy.GetComponent<Renderer>().material = mat_Placeable;
        else
            prefabCopy.GetComponent<Renderer>().material = mat_NotPlaceable;
    }
    float RoundToNearPosition(float value)//Creates a grid system
    {
        float Difference = value % gridSize;
        value -= Difference;
        if (Difference > gridSize / 2)
            value += gridSize;
        return value;
    }
    public void ObjectSpawn(int index)//Spawn chosen object
    {   
        if(prefabCopy == null)
        {
            prefabCopy = Instantiate(spawnObject[index], prefabCopyPos, Quaternion.identity);
            matForPrefab = spawnObject[index].GetComponent<Renderer>().sharedMaterial;
            objPlacementActive = true;
            selectionOutline();
        } 
    }

    //public void EnableChildMenu(GameObject gO)
    //{
    //    GameObject menu = gO.transform.GetChild(0).gameObject;
    //    foreach (Transform menuChild in menu.transform)
    //    {
    //        if(!menuChild.GetComponent<Animator>().GetBool("buildingsOn"))
    //            menuChild.GetComponent<Animator>().SetBool("buildingsOn", true);
    //        else
    //            menuChild.GetComponent<Animator>().SetBool("buildingsOn", false);
    //    }
    //}

    public void EnableChildMenu()
    {
        foreach (Transform menuChild in buildingsMenu.transform)
        {
            if (!menuChild.GetComponent<Animator>().GetBool("buildingsOn"))
                menuChild.GetComponent<Animator>().SetBool("buildingsOn", true);
            else
                menuChild.GetComponent<Animator>().SetBool("buildingsOn", false);
        }
    }
    void selectionOutline()
    {
        if (!prefabCopy.GetComponent<Outline>()) prefabCopy.AddComponent<Outline>();
        else prefabCopy.GetComponent<Outline>().enabled = prefabCopy.GetComponent<Outline>().enabled ? false : true;
            
    }
}
