using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeMaterials : MonoBehaviour
{
    [SerializeField] GameObject housePrefab;
    [SerializeField] Material a, b,baseMat;
    GameObject refPrefab;
    private void Start()
    {
       
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            refPrefab = Instantiate(housePrefab, new Vector3(5.3f, 0, 9.13f), Quaternion.identity);
            baseMat = refPrefab.GetComponent<Renderer>().material;
        }
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            refPrefab.GetComponent<Renderer>().material = a;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            refPrefab.GetComponent<Renderer>().material = b;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            refPrefab.GetComponent<Renderer>().material = baseMat;
        }
    }
}
