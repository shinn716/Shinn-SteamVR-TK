using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class BtDrag : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{
    public bool boolDraggable = false;
    public Image imageBg;
    public Transform m_transformMainObj;

    [Space]
    public List<Transform> m_items = new List<Transform>();

    private Transform m_itemGroup;
    private GameObject m_goEmpty;
    private int m_currentIndex;
    private int m_targetIndex;

    private void Start()
    {
        m_itemGroup = m_transformMainObj.parent;
        GetComponent<Image>().enabled = boolDraggable;
    }

    public void OnPointerEnter(PointerEventData evd)
    {
        if (imageBg != null)
            imageBg.enabled = true;
    }
    public void OnPointerExit(PointerEventData evd)
    {
        if (imageBg != null)
            imageBg.enabled = false;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!boolDraggable)
            return;

        m_items.Clear();
        m_currentIndex = m_transformMainObj.GetSiblingIndex();

        m_goEmpty = new GameObject();
        m_goEmpty.gameObject.name = "temp";
        m_goEmpty.transform.parent = m_transformMainObj.parent;
        m_goEmpty.AddComponent<RectTransform>().sizeDelta = m_transformMainObj.GetComponent<RectTransform>().sizeDelta;
        m_goEmpty.transform.localPosition = Vector3.zero;
        m_goEmpty.transform.localScale = Vector3.one;
        m_goEmpty.transform.SetSiblingIndex(m_currentIndex);
        
        foreach (Transform i in m_transformMainObj.parent)
        {
            if (i.name.Equals("temp") ||
                !i.name.Equals(m_transformMainObj.name)
                )
                m_items.Add(i);
        }
        m_transformMainObj.SetParent(m_transformMainObj.parent.parent);
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (!boolDraggable)
            return;
        m_transformMainObj.GetComponent<RectTransform>().position = Standalone_Controller.instance.sprDot.transform.position;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!boolDraggable)
            return;

        //StartCoroutine(Process());

        var target = GetClosestObject(m_items.ToArray(), Standalone_Controller.instance.sprDot.transform.position);
        m_targetIndex = target.GetSiblingIndex();

        m_transformMainObj.SetParent(m_itemGroup);
        m_transformMainObj.SetSiblingIndex(m_targetIndex);
        m_transformMainObj.GetComponent<RectTransform>().localPosition = Vector3.Scale(m_transformMainObj.GetComponent<RectTransform>().localPosition, new Vector3(1, 1, 0));

        target.SetSiblingIndex(m_currentIndex);
        Destroy(m_goEmpty);
    }

    private IEnumerator Process()
    {
        yield return null;
        var target = GetClosestObject(m_items.ToArray(), Standalone_Controller.instance.sprDot.transform.position);
        m_targetIndex = target.GetSiblingIndex();

        m_transformMainObj.SetParent(m_itemGroup);
        m_transformMainObj.SetSiblingIndex(m_targetIndex);
        m_transformMainObj.GetComponent<RectTransform>().localPosition = Vector3.Scale(m_transformMainObj.GetComponent<RectTransform>().localPosition, new Vector3(1, 1, 0));

        target.SetSiblingIndex(m_currentIndex);

        yield return null;
        Destroy(m_goEmpty);

    }

    private Transform GetClosestObject(Transform[] objs, Vector3 target)
    {
        float[] array = new float[objs.Length];

        for(int i=0; i<array.Length; i++)
            array[i] = Vector3.Distance(objs[i].position, target);

        int getMinIndex = System.Array.IndexOf(array, Mathf.Min(array));
        return objs[getMinIndex];
    }
}