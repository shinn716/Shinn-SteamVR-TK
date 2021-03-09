using UnityEngine;
using System.Collections;
using DG.Tweening;

public class ScreenFade
{
    private GameObject m_go;
    private Mesh m_mesh;
    private Material m_materialInstance;
    private int m_materialFadeID;
    private float m_value;

    public ScreenFade()
    {
        var mat = Resources.Load<Material>("Materials/FadeBlack");
        m_mesh = new Mesh();
        Vector3[] verts = new Vector3[]
        {
            new Vector3(-1, -1, 0),
            new Vector3(-1, 1, 0),
            new Vector3(1, 1, 0),
            new Vector3(1, -1, 0)
        };
        int[] elts = new int[] { 0, 1, 2, 0, 2, 3 };
        m_mesh.vertices = verts;
        m_mesh.triangles = elts;
        m_mesh.RecalculateBounds();

        m_go = new GameObject("fadeback", typeof(MeshFilter), typeof(MeshRenderer));
        m_materialInstance = new Material(mat);

        m_go.GetComponent<MeshFilter>().mesh = m_mesh;
        m_go.GetComponent<MeshRenderer>().material = m_materialInstance;

        m_go.transform.SetParent(Camera.main.transform);
        m_go.transform.localPosition = Vector3.forward * .4f;
        m_go.transform.localRotation = Quaternion.identity;
        m_go.transform.localScale = Vector3.one;

        m_materialFadeID = Shader.PropertyToID("_Fade");
        m_materialInstance.SetFloat(m_materialFadeID, 0);
    }

    public void FadeIn(float during)
    {
        DOTween.To(() => m_value, x => m_value = x, 1, during).SetEase(Ease.OutSine).OnUpdate(() =>
        m_materialInstance.SetFloat(m_materialFadeID, m_value)
        );
    }

    public void FadeOut(float during)
    {
        DOTween.To(() => m_value, x => m_value = x, 0, during).SetEase(Ease.OutSine).OnUpdate(() =>
        m_materialInstance.SetFloat(m_materialFadeID, m_value)
        );
    }

    public void Dispose()
    {
        GameObject.Destroy(m_go);
    }
}