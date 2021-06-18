// Author: John Tsai
// Modify from https://www.youtube.com/watch?v=ryfUXr5yvKw

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class ShiHand : MonoBehaviour
{
    public SteamVR_Behaviour_Pose GetPose { get; private set; } = null;
    public Hand GetHand { get; private set; } = null;

    public SteamVR_Action_Boolean TriggerAction = null;
    public SteamVR_Action_Boolean MenuAction = null;
    public SteamVR_Action_Boolean GrabAction = null;
    public SteamVR_Action_Boolean TeleportAction = null;
    public SteamVR_Action_Boolean SnapTurnLeftAction = null;
    public SteamVR_Action_Boolean SnapTurnRightAction = null;
    
    public bool EnableControllerHints = true;

    private ShiInteractable m_CurrentInteractable = null;
    private List<ShiInteractable> m_CurrentInteractables = new List<ShiInteractable>();
    private bool m_ColliderTrigger = false;
    
    private void Awake()
    {
        GetPose = GetComponent<SteamVR_Behaviour_Pose>();
        GetHand = GetComponent<Hand>();
    }
    
    private void FixedUpdate()
    {
        // Trigger
        if (TriggerAction.GetLastStateDown(GetPose.inputSource))
        {
            if (m_CurrentInteractable != null)
            {
                m_CurrentInteractable.Action();
                return;
            }
            Pickup();
        }

        if (TriggerAction.GetLastStateUp(GetPose.inputSource))
        {
            if (m_CurrentInteractable == null)
                return;            
            Drop();
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (!other.gameObject.CompareTag("Interactable"))
            return;

        if (!m_ColliderTrigger)
        {
            m_ColliderTrigger = true;
            other.gameObject.GetComponent<ShiInteractable>().EnableHightLight();
            m_CurrentInteractables.Add(other.gameObject.GetComponent<ShiInteractable>());

            if (EnableControllerHints)
                ControllerButtonHints.ShowTextHint(GetComponent<Hand>(), TriggerAction, "Trigger");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Interactable"))
            return;

        if (m_ColliderTrigger)
        {
            m_ColliderTrigger = false;
            other.gameObject.GetComponent<ShiInteractable>().DisableHightLight();
            m_CurrentInteractables.Remove(other.gameObject.GetComponent<ShiInteractable>());

            if (EnableControllerHints)
                ControllerButtonHints.HideTextHint(GetComponent<Hand>(), TriggerAction);
        }
    }

    private void Pickup()
    {
        m_CurrentInteractable = GetNearestInteractable();

        if (!m_CurrentInteractable)
            return;

        if (m_CurrentInteractable.m_ActiveHand)
            m_CurrentInteractable.m_ActiveHand.Drop();

        m_CurrentInteractable.ApplyTransform(transform);
        m_CurrentInteractable.m_ActiveHand = this;

        // TODO:
        // 物件包含 hand physics 功能
        //if (!m_CurrentInteractable.GetComponent<Rigidbody>().useGravity)
        //    return;
        //m_CurrentInteractable.GetComponent<Rigidbody>().isKinematic = true;
    }

    private void Drop()
    {
        if (!m_CurrentInteractable)
            return;

        // TODO:
        // 物件包含 hand physics 功能
        //m_CurrentInteractable.GetComponent<Rigidbody>().isKinematic = false;
        //m_CurrentInteractable.GetComponent<Rigidbody>().useGravity = true;

        m_CurrentInteractable.Release();
        m_CurrentInteractable.m_ActiveHand = null;
        m_CurrentInteractable = null;
    }

    private ShiInteractable GetNearestInteractable()
    {
        ShiInteractable nearest = null;
        float minDistance = float.MaxValue;
        float distance = .0f;

        foreach (ShiInteractable interactable in m_CurrentInteractables)
        {
            distance = (interactable.transform.position - transform.position).sqrMagnitude;

            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = interactable;
            }
        }
        return nearest;
    }
}
 