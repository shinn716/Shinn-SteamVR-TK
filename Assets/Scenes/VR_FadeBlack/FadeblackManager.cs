using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeblackManager : MonoBehaviour
{
    ScreenFade screenFade;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        screenFade = new ScreenFade();
        screenFade.FadeIn(1);
        yield return new WaitForSeconds(1);
        screenFade.FadeOut(1);
        yield return new WaitForSeconds(1);
        screenFade.Dispose();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
