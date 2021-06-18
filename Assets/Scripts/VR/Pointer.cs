// Athor: John Tsai
// Base on 
// Modify from https://www.youtube.com/watch?v=ryfUXr5yvKw

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pointer : MonoBehaviour
{
    public Camera raycastCam;
    public GameObject m_Dot;
    public float m_DefaultLength = 5f;

    private VR_InputModule inputModule = null;
    private LineRenderer lineRenderer = null;

    private void Start()
    {
        inputModule = FindObjectOfType<VR_InputModule>();
        lineRenderer = GetComponent<LineRenderer>();
    }
    
    private void Update()
    {
        UpdateLine();
    }
    
    private void UpdateLine()
    {
        PointerEventData pointerEventData = inputModule.Data;
        if (pointerEventData.pointerCurrentRaycast.gameObject == null)
        {
            if (m_Dot.activeSelf != false)
                EnableRaycaster(false);
            return;
        }

        if (m_Dot.activeSelf != true)
            EnableRaycaster(true);

        float targetLength = pointerEventData.pointerCurrentRaycast.distance == 0 ? m_DefaultLength : pointerEventData.pointerCurrentRaycast.distance;
        
        Vector3 endPosition = transform.position + (transform.forward * targetLength);
        
        m_Dot.transform.LookAt(raycastCam.transform);
        m_Dot.transform.position = endPosition;

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, endPosition);
    }

    private void EnableRaycaster(bool enable)
    {
        m_Dot.SetActive(enable);
        lineRenderer.enabled = enable;
    }
}
