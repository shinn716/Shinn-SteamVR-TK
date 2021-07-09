// John

using UnityEngine;
using System.Collections;
using DG.Tweening;

public class ScreenFade : MonoBehaviour
{
    private GameObject m_go;
    private Mesh m_mesh;
    private Material m_materialInstance;
    private int m_materialFadeID;
    private float m_value;
    private float alpha = 1;

    public ScreenFade(string fadeblackObjUrlFromResources, float talpha = .7f)
    {
        alpha = talpha;
        var screenFadeObject = Resources.Load<ScreenFadeObject>(fadeblackObjUrlFromResources);
        var mat = screenFadeObject.materialFadeBlack;

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

        if (screenFadeObject.loadingUI == null)
            return;

        var hint = Instantiate(screenFadeObject.loadingUI);
        hint.transform.SetParent(m_go.transform);
        hint.transform.localPosition = Vector3.zero;
        hint.transform.localRotation = Quaternion.identity;
        hint.transform.localScale = Vector3.one * 0.0003f;
    }

    public void FadeIn(float during_second, Ease ease = Ease.OutSine)
    {
        DOTween.To(() => m_value, x => m_value = x, alpha, during_second).SetEase(ease).OnUpdate(() =>
        m_materialInstance.SetFloat(m_materialFadeID, m_value)
        );
    }

    public void FadeOut(float during_second, Ease ease = Ease.OutSine)
    {
        DOTween.To(() => m_value, x => m_value = x, 0, during_second).SetEase(ease).OnUpdate(() =>
        m_materialInstance.SetFloat(m_materialFadeID, m_value)
        );
    }

    public void Dispose()
    {
        Destroy(m_go);
    }
}