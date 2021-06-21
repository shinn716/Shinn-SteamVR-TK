using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ColorPick : MonoBehaviour
{
    public Transform TransformUIParent;
    public Image ImageColorBoad;
    public Image ImageColorPanel;
    public Transform thumb;

    private Vector3 thumbOriginPos;

    private void Start()
    {
        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Drag;
        entry.callback.AddListener((data) => { OnDragDelegate((PointerEventData)data); });
        trigger.triggers.Add(entry);

        thumbOriginPos = thumb.position;
    }

    private void OnDragDelegate(PointerEventData data)
    {
        // Standalone
        if (UnityEngine.XR.XRSettings.loadedDeviceName.Equals("None") || UnityEngine.XR.XRSettings.loadedDeviceName.Equals(""))
        {
            if (Input.GetMouseButton(0))
                SetThumbPosition(Standalone_Controller.instance.sprDot.transform.position);
        }
        // SteamVR
        else
        {
            if (data.pointerCurrentRaycast.gameObject == null)
                return;

            if (!data.pointerCurrentRaycast.gameObject.name.Equals("ColorPanel"))
                return;

            float distance = Vector3.Distance(data.pointerCurrentRaycast.worldPosition, thumbOriginPos);
            
            if (distance < .2f)
                SetThumbPosition(data.pointerCurrentRaycast.worldPosition);
        }
    }
    private void SetThumbPosition(Vector3 point)
    {
        thumb.position = point;
        thumb.localPosition = new Vector3(thumb.localPosition.x, thumb.localPosition.y, 0);
        GetColor(thumb.GetComponent<RectTransform>().anchoredPosition);
    }

    private void GetColor(Vector2 pos)
    {
        var px = Map(pos.x - TransformUIParent.position.x, -250f, 250f, 0, 730);
        var py = Map(pos.y - TransformUIParent.position.y, -250f, 250f, 0, 730);

        Rect rect = ImageColorPanel.GetComponent<RectTransform>().rect;
        Color imageColor = ImageColorPanel.sprite.texture.GetPixel(
            Mathf.FloorToInt(px),
            Mathf.FloorToInt(py) + 50
            );
        
        ImageColorBoad.color = new Color(imageColor.r, imageColor.g, imageColor.b);
    }

    private float Map(float v, float a, float b, float x, float y)
    {
        return (v == a) ? x : (v - a) * (y - x) / (b - a) + x;
    }
}
