using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMovement : MonoBehaviour
{
    Vector3 pos;
    [Header("Camera Speed Parameters")]
    [SerializeField] float camMoveSpeed = 5f;
    [SerializeField] float camZoomSpeed = 10f;
    [SerializeField] float camRotateSpeed = 60f;
    
    float targetZoom;
    [Header("Camera Zoom Parameters")]
    [SerializeField] float min=4.6f;
    [SerializeField] float max=14f;
    [SerializeField] float zoomFactor = 5f;

    [SerializeField] int FPSTarget = 60;

    float hor, ver, scrollData;
    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = FPSTarget;
        transform.localPosition = new Vector3(BuildingManager.planeSize / 2,transform.localPosition.y, BuildingManager.planeSize / 2);
        pos = transform.localPosition;
        targetZoom = pos.y;
    }
    private void Update()
    {
        hor = Input.GetAxisRaw("Horizontal");
        ver = Input.GetAxisRaw("Vertical");
        scrollData = Input.GetAxis("Mouse ScrollWheel");

        targetZoom -= scrollData * zoomFactor;
        targetZoom = Mathf.Clamp(targetZoom, min, max);

        pos += transform.right * hor * camMoveSpeed *Time.unscaledDeltaTime/** Time.deltaTime*/;
        pos += transform.forward * ver * camMoveSpeed * Time.unscaledDeltaTime /** Time.deltaTime*/;
        pos.y = Mathf.Lerp(transform.localPosition.y, targetZoom, camZoomSpeed *Time.unscaledDeltaTime/** Time.deltaTime*/);

        transform.localPosition = pos;

        if (Input.GetKey(KeyCode.E))
            transform.Rotate(new Vector3(0, 1, 0) * camRotateSpeed *Time.unscaledDeltaTime/** Time.deltaTime*/, Space.World);
        if (Input.GetKey(KeyCode.Q))
            transform.Rotate(new Vector3(0, -1, 0) * camRotateSpeed *Time.unscaledDeltaTime/** Time.deltaTime*/, Space.World);
    }
}
