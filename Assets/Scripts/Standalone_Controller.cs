using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Standalone_Controller : MonoBehaviour
{
    public static Standalone_Controller instance;

    public GameObject sprDot;

    private bool m_ColliderTrigger = false;
    private GameObject m_target;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        RayHit();
    }
    
    private void RayHit()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Transform objectHit = hit.transform;
            sprDot.transform.position = hit.point;
            
            if (m_ColliderTrigger && m_target != null && Input.GetMouseButtonDown(0))
                m_target.SendMessage("StartEvent", SendMessageOptions.DontRequireReceiver);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Interactable"))
            return;

        if (!m_ColliderTrigger)
        {
            m_ColliderTrigger = true;
            other.SendMessage("EnableHightLight", SendMessageOptions.DontRequireReceiver);
            m_target = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Interactable"))
            return;

        if (m_ColliderTrigger)
        {
            m_ColliderTrigger = false;
            other.SendMessage("DisableHightLight", SendMessageOptions.DontRequireReceiver);
            m_target = null;
        }
    }


}
