using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OmgIsItGonnaWork : MonoBehaviour
{
    [SerializeField] Material mat_PlacementOn;

    Material originalMaterial;

    public bool imPlaced = false;

    Collider[] checkAround;

    List<GameObject> collectableObjectUnderPlaceable = new List<GameObject>();

    [SerializeField] float timeParameterForLerp=1f;

    GameObject effectPrefab;
    private void Awake()
    {
        originalMaterial = GetComponent<Renderer>().sharedMaterial;
        originalMaterial.SetFloat("_Clip", 1);
        effectPrefab = transform.GetChild(0).gameObject;
        
    }
    private void Start()
    {
        
    }
    public float current = 0;
    private void Update()
    {
        if (imPlaced)
        {
            effectPrefab.SetActive(false);
            checkAround = Physics.OverlapSphere(transform.position, 2f);
            if (checkAround.Length > 0)
            {
                foreach (var item in checkAround)
                {
                    if (item.tag == "rock" || item.tag == "tree")
                    {
                        if(!collectableObjectUnderPlaceable.Contains(item.gameObject))
                            collectableObjectUnderPlaceable.Add(item.gameObject);
                    }
                }                              
            }

            for (int i = 0; i < collectableObjectUnderPlaceable.Count; i++)
            {
                if (!collectableObjectUnderPlaceable[i].activeSelf)
                    collectableObjectUnderPlaceable.Remove(collectableObjectUnderPlaceable[i]);
            }
            if (collectableObjectUnderPlaceable.Count > 0)
            {
                Debug.Log("there is elements");                
                GetComponent<Renderer>().sharedMaterial = mat_PlacementOn;
            }
            else
            {
                Debug.Log("there is no elements");
                GetComponent<Renderer>().sharedMaterial = originalMaterial;
                originalMaterial.SetFloat("_Clip", 0f);
                current = Mathf.MoveTowards(current, 1, timeParameterForLerp);
                originalMaterial.SetFloat("_Clip", current);
                if(current>=1)
                {
                    effectPrefab.SetActive(true);
                    imPlaced = false;
                    this.enabled = false;
                }
            }            
        }
    }
}
