// Author: John Tsai
// Modify from https://www.youtube.com/watch?v=ryfUXr5yvKw

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class ShiInteractable : MonoBehaviour
{
    public enum Type
    {
        Grabbable,
        Triggerable
    }
    
    public Type type = Type.Grabbable;
    public UnityEvent unityEvent;

    public ShiHand m_ActiveHand { get; set; } = null;

    private GameObject m_hightlightGo = null;
    private bool m_hightlight = false;

    public virtual void Action()
    {
        //print("Action");
    }

    public void ApplyTransform(Transform hand)
    {
        if (type.Equals(Type.Grabbable))
        {
            if (m_hightlightGo != null)
                DisableHightLight();
            
            transform.SetParent(hand);
            unityEvent.Invoke();
        }
        else if (type.Equals(Type.Triggerable))
        {
            if (m_hightlightGo != null)
                DisableHightLight();
            
            unityEvent.Invoke();
        }
    }

    public void Release()
    {
        if (type.Equals(Type.Grabbable))
            transform.SetParent(null);
    }

    public void EnableHightLight()
    {
        if (m_hightlight)
            return;

        m_hightlight = true;
        m_hightlightGo = Instantiate(gameObject);
        m_hightlightGo.name = $"HightLight_{gameObject.name}(Clone)";
        m_hightlightGo.GetComponent<Renderer>().receiveShadows = false;
        m_hightlightGo.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        m_hightlightGo.GetComponent<Renderer>().lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
        m_hightlightGo.GetComponent<Renderer>().reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
        m_hightlightGo.GetComponent<Renderer>().allowOcclusionWhenDynamic = false;

        m_hightlightGo.GetComponent<Renderer>().material = Resources.Load<Material>("SteamVR_HoverHighlight");
        //m_hightlightGo.transform.SetPositionAndRotation(transform.position, transform.rotation);

        m_hightlightGo.transform.SetParent(transform);
        m_hightlightGo.transform.localPosition = Vector3.zero;
        m_hightlightGo.transform.localRotation = Quaternion.identity;

        Destroy(m_hightlightGo.GetComponent<ShiInteractable>());
        Destroy(m_hightlightGo.GetComponent<Collider>());
        Destroy(m_hightlightGo.GetComponent<Rigidbody>());
    }
    
    public void DisableHightLight()
    {
        Destroy(m_hightlightGo);
        m_hightlightGo = null;
        m_hightlight = false;
    }

    public void StartEvent()
    {
        unityEvent.Invoke();
    }
}
