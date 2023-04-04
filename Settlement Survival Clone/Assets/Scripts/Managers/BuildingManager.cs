using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    [SerializeField] float planeSizeValue;
    [SerializeField] float restrictAmount;
    public ParticleSystem placementSmoke;


    public static Action OnObjectPlaced = delegate { };

    public static float planeSize { get; private set; }//if this is 200 plansize is 200*200
    public static float restrictValue { get; private set; }//Alaný her köþeden ne kadar sýnýrlayacaðýný belirtir.

    public static List<GameObject> allPlacedObject = new List<GameObject>();

    GameObject prefabCopy;
    Vector3 prefabCopyPos;
    Material matForPrefab;
    
    Dictionary<GameObject, Material> prefabsMatsOnTheScene = new Dictionary<GameObject, Material>();
    List<GameObject> objectPool = new List<GameObject>();

    bool isThereAnObject;
    bool objPlacementActive;
    float rotateAmount;

    private void Awake()
    {
        placementSmoke.Stop();

        BuildingPricesForEachBuilding();

        allPlacedObject.AddRange(GameObject.FindGameObjectsWithTag("placeable"));

        for (int i = 0; i < allPlacedObject.Count; i++)
        {
            prefabsMatsOnTheScene.Add(allPlacedObject[i], allPlacedObject[i].GetComponent<Renderer>().material);
        }        

        planeSize = planeSizeValue;
        restrictValue = restrictAmount;
    }
    BuildingPrices basicHouse;
    BuildingPrices secondHouse;
    List<BuildingPrices> buildingAndPrices = new List<BuildingPrices>();
    void BuildingPricesForEachBuilding()
    {
        basicHouse = new BuildingPrices(0, 350, 0);
        secondHouse = new BuildingPrices(1, 250, 250);

        buildingAndPrices.Add(basicHouse);
        buildingAndPrices.Add(secondHouse);
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
                objectPool.Add(prefabCopy);
                prefabCopy.SetActive(false);
                prefabCopy = null;
                objPlacementActive = false;
            }//Cancel placement for selected object
        }
        PlacementMaterialChange();
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
    Collider[] checkAroundSpawnObject;
    void ReleaseObject()
    {
        if (Input.GetMouseButtonDown(0) && isPlaceable)
        {
            if(prefabCopy.GetComponent<CheckPlacement>()!=null)
                prefabCopy.GetComponent<CheckPlacement>().imPlaced = true;

            prefabCopy.GetComponent<Renderer>().sharedMaterial = matForPrefab;
            prefabsMatsOnTheScene.Add(prefabCopy, prefabCopy.GetComponent<Renderer>().material);

            allPlacedObject.Add(prefabCopy);

            checkAroundSpawnObject = Physics.OverlapSphere(prefabCopy.transform.position, 2f);
            if (checkAroundSpawnObject.Length >= 0)
            {
                foreach (var item in checkAroundSpawnObject)
                {
                    if (item.tag == "tree" || item.tag == "rock")
                    {
                        BoxSelectionScript.SelectObjectUnderBuilding(item.gameObject);
                    }
                }
            }

            //if (prefabCopy.transform.childCount > 0)
            //    prefabCopy.transform.GetChild(0).gameObject.SetActive(true);//If object has an particle effect actives it
            foreach (var item in buildingAndPrices)
            {
                if (currentIndex == item.indexOfObject)
                {
                    UiManager.woodCount -= item.woodPrice;
                    UiManager.rockCount -= item.rockPrice;
                }
            }

            prefabCopy = null;
            objPlacementActive = false;

            OnObjectPlaced();//Play a sound or do something when a building placed.            
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
    void selectionOutline()
    {
        if (!prefabCopy.GetComponent<Outline>()) prefabCopy.AddComponent<Outline>();
        else prefabCopy.GetComponent<Outline>().enabled = prefabCopy.GetComponent<Outline>().enabled ? false : true;
            
    }
    float RoundToNearPosition(float value)//Creates a grid system
    {
        float Difference = value % gridSize;
        value -= Difference;
        if (Difference > gridSize / 2)
            value += gridSize;
        return value;
    }
    int currentIndex;
    public void ObjectSpawn(int index)//Spawn chosen object
    {
        if (prefabCopy == null)
        {
            foreach (var item in buildingAndPrices)
            {
                if(index == item.indexOfObject && UiManager.woodCount >= item.woodPrice && UiManager.rockCount >= item.rockPrice)
                {
                    currentIndex = index;

                    foreach (var objFromPool in objectPool)
                    {
                        if (objFromPool.name == spawnObject[index].name)
                        {
                            objFromPool.SetActive(true);
                            prefabCopy = objFromPool;
                            objectPool.Remove(objFromPool);
                            isThereAnObject = true;
                            break;
                        }
                    }
                    if (!isThereAnObject)
                    {
                        prefabCopy = Instantiate(spawnObject[index], prefabCopyPos, Quaternion.identity);
                        prefabCopy.name = spawnObject[index].name;
                        selectionOutline();
                    }
                    matForPrefab = spawnObject[index].GetComponent<Renderer>().sharedMaterial;
                    objPlacementActive = true;
                    isThereAnObject = false;
                }
            }
            //if ((UiManager.woodCount >= basicHouse.woodPrice && UiManager.rockCount >= basicHouse.rockPrice && basicHouse.indexOfObject == index) ||
            //    (UiManager.woodCount >= secondHouse.woodPrice && UiManager.rockCount >= secondHouse.rockPrice && secondHouse.indexOfObject == index))
            //{
            //    currentIndex = index;

            //    foreach (var objFromPool in objectPool)
            //    {
            //        if (objFromPool.name == spawnObject[index].name)
            //        {
            //            objFromPool.SetActive(true);
            //            prefabCopy = objFromPool;
            //            objectPool.Remove(objFromPool);
            //            isThereAnObject = true;
            //            break;
            //        }
            //    }
            //    if (!isThereAnObject)
            //    {
            //        prefabCopy = Instantiate(spawnObject[index], prefabCopyPos, Quaternion.identity);
            //        prefabCopy.name = spawnObject[index].name;
            //        selectionOutline();
            //    }
            //    matForPrefab = spawnObject[index].GetComponent<Renderer>().sharedMaterial;
            //    objPlacementActive = true;
            //    isThereAnObject = false;
            //}
        } 
    }
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
}

public struct BuildingPrices
{
    public int indexOfObject;
    public int woodPrice;
    public int rockPrice;

    public BuildingPrices(int indexOfObject, int woodPrice, int rockPrice)
    {
        this.indexOfObject = indexOfObject;
        this.woodPrice = woodPrice;
        this.rockPrice = rockPrice;
    }
}
