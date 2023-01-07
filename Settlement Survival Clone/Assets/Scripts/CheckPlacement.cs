using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlacement : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "zemin")
        {
            BuildingManager.isPlaceable = true;
        }        
        if (other.gameObject.tag == "placeable" || other.gameObject.name== "NotPlaceable")
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
