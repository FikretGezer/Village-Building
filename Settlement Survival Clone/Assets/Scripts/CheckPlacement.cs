using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlacement : MonoBehaviour
{
    [SerializeField] Material mat_PlacementOn;
    [SerializeField] float timeParameterForLerp = 0.015f;
   

    List<GameObject> collectableObjectUnderPlaceable = new List<GameObject>();

    Material originalMaterial;
    GameObject bacaDumanieffectPrefab;

    public bool imPlaced = false;

    Collider[] checkAround;

    public float current = 1f;

    BuildingManager buildingManager;

    Animator buildingAnim;

    Renderer thisRenderer;

    private void Awake()
    {
        buildingManager = FindObjectOfType<BuildingManager>();

        buildingAnim = GetComponent<Animator>();
        thisRenderer = GetComponent<Renderer>();
        originalMaterial = thisRenderer.material;
        bacaDumanieffectPrefab = transform.GetChild(0).gameObject;
        current = 0f;
    }
    private void Update()
    {
        if (imPlaced)
        {
            bacaDumanieffectPrefab.SetActive(false);
            checkAround = Physics.OverlapSphere(transform.position, 2f);
            if (checkAround.Length > 0)
            {
                foreach (var item in checkAround)
                {
                    if (item.tag == "rock" || item.tag == "tree")
                    {                        
                        if (!collectableObjectUnderPlaceable.Contains(item.gameObject))
                            collectableObjectUnderPlaceable.Add(item.gameObject);
                    }
                }
            }

            for (int i = 0; i < collectableObjectUnderPlaceable.Count; i++)
            {
                if (!collectableObjectUnderPlaceable[i].activeSelf)
                {
                    if (!SpawnNatureElements.DisabledGameObjects.Contains(collectableObjectUnderPlaceable[i]))
                    {
                        SpawnNatureElements.DisabledGameObjects.Add(collectableObjectUnderPlaceable[i]);
                    }

                    collectableObjectUnderPlaceable.Remove(collectableObjectUnderPlaceable[i]);                    
                }
            }


            if (collectableObjectUnderPlaceable.Count > 0)
            {
                GetComponent<Renderer>().sharedMaterial = mat_PlacementOn;
            }
            else
            {
                current = Mathf.MoveTowards(current, 1, timeParameterForLerp * Time.deltaTime);
                originalMaterial.SetFloat("_Clip", current);
                thisRenderer.material = originalMaterial;

                if(thisRenderer.sharedMaterial.name==originalMaterial.name)
                {
                    if (thisRenderer.sharedMaterial.GetFloat("_Clip") >= 1f)
                    {
                        buildingManager.placementSmoke.gameObject.transform.position = this.gameObject.transform.position;
                        buildingAnim.SetTrigger("houseBig");
                        buildingManager.placementSmoke.Play();
                        bacaDumanieffectPrefab.SetActive(true);
                        imPlaced = false;
                        this.enabled = false;
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "zemin")
        {
            BuildingManager.isPlaceable = true;
        }
        if (other.gameObject.tag == "placeable" || other.gameObject.name == "NotPlaceable")
        {
            BuildingManager.isPlaceable = false;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "placeable" || other.gameObject.name == "NotPlaceable")
        {
            BuildingManager.isPlaceable = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "placeable" || other.gameObject.name == "NotPlaceable")
        {
            BuildingManager.isPlaceable = true;
        }
    }
}

