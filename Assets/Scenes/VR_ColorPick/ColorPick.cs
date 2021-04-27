using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPick : MonoBehaviour
{
    public Transform TransformUIParent;
    public Image ImageColorBoad;
    public Image ImageColorPanel;
    public Transform thumb;
    
    void Update()
    {
        if(Input.GetMouseButton(0))
            SetThumbPosition(Standalone_Controller.instance.sprDot.transform.position);
    }

    private void SetThumbPosition(Vector3 point)
    {
        thumb.position = point;
        GetColor(point);
    }

    private void GetColor(Vector2 pos)
    {
        var px = Map(pos.x - TransformUIParent.position.x, -.2f, .2f, 0, 730);
        var py = Map(pos.y - TransformUIParent.position.y, -.1f, .3f, 0, 730);

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
