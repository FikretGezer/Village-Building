using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlopeAngleScript : MonoBehaviour
{
    public Transform rearRayPos;
    public Transform frontRayPos;
    public LayerMask layerMask;

    float surfaceAngle;
    bool upHill;
    bool flatSurface;

    private void Update()
    {
        rearRayPos.rotation = Quaternion.Euler(-gameObject.transform.rotation.x, 0, 0);
        frontRayPos.rotation = Quaternion.Euler(-gameObject.transform.rotation.x, 0, 0);

        RaycastHit rearHit;
        if(Physics.Raycast(rearRayPos.position,rearRayPos.TransformDirection(-Vector3.up),out rearHit,layerMask))
        {
            surfaceAngle = Vector3.Angle(rearHit.normal, Vector3.up);
            Debug.Log(surfaceAngle);
            //if (surfaceAngle > 0 )
            //    Debug.Log("Yokuþ");
            //else if(surfaceAngle<=0 && rearHit.collider.transform.gameObject.layer == 10)
            //    Debug.Log("Düz zemin");
        }
        else
        {
            upHill = false;
            Debug.LogWarning("DownHill");
        }
        RaycastHit frontHit;
        Vector3 frontRayStartPos = new Vector3(frontRayPos.position.x, rearRayPos.position.y, frontRayPos.position.z);
        if (Physics.Raycast(frontRayStartPos, frontRayPos.TransformDirection(-Vector3.up), out frontHit, layerMask)) ;
        else
        {
            upHill = true;
            Debug.LogWarning("UpHill");
        }
        if(frontHit.distance<rearHit.distance)
        {
            upHill = true;
            Debug.LogWarning("UpHill");
        }
        else if (frontHit.distance > rearHit.distance)
        {
            upHill = false;
            Debug.LogWarning("DownHill");
        }
        else if (frontHit.distance == rearHit.distance)
        {
            flatSurface = true;
            Debug.LogWarning("Flat Surface");
        }
        else
        {
            flatSurface = false;
        }
        if(flatSurface)
        {
            GetComponent<Renderer>().material.color = new Color32(255, 0, 0, 120);
        }
        else
            GetComponent<Renderer>().material.color = new Color32(0, 0, 255, 120);
    }
}
