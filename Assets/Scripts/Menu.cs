using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public Transform lefthand;
    public GameObject menu;

    private bool m_open = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (SteamvrInputModule.instance.shiHands[1] == null)
            return;

        if (SteamvrInputModule.instance.shiHands[1].MenuAction.GetLastStateDown(SteamvrInputModule.instance.shiHands[1].GetPose.inputSource))
        {
            m_open = !m_open;
            if (m_open)
            {
                menu.transform.localScale = Vector3.one * 0.001f;
                menu.transform.SetParent(lefthand);
                menu.transform.localPosition = new Vector3(0, 0, .2f);
                menu.transform.localRotation = Quaternion.Euler(90, 0, 0);
            }
            else
                menu.transform.localScale = Vector3.zero;
        }
    }
}
