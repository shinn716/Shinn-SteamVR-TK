using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScreenFadeObject", menuName = "ScreenFade/Fadeblack")]
public class ScreenFadeObject : ScriptableObject
{
    public GameObject loadingUI;
    public Material materialFadeBlack;
}
