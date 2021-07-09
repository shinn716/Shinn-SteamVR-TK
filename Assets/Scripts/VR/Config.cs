using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config : MonoBehaviour
{
    public KeyCode openkey = KeyCode.BackQuote;
    public bool enablePlayerDebugMode = false;

    public bool toggleDisableHMD { get; set; } = false;
    public bool toggleEnableFPS { get; set; } = false;

    private uint panelWidth = 300;
    private bool m_open = false;
    private string toggleEnableVRTitle = string.Empty;

    private GUIStyle guiStyle = new GUIStyle(); //create a new variable

    private float m_showFps = 0;
    private float nextActionTime = 0;

    private enum Toggle1Status
    {
        MODE_VR,
        MODE_STANDALONE
    }

    private void Start()
    {
        LoadData();
        DisableHMD(toggleDisableHMD);

        Valve.VR.InteractionSystem.Player.instance.allowToggleTo2D = enablePlayerDebugMode;
    }

    private void Update()
    {
        if (Input.GetKeyDown(openkey))
        {
            m_open = !m_open;

            if (!m_open)
                SaveData();
        }
    }

    private void OnGUI()
    {

        if (toggleEnableFPS)
        {
            if (Time.unscaledTime > nextActionTime)
            {
                nextActionTime = Time.unscaledTime + 1f;
                m_showFps = Mathf.Ceil(1 / Time.unscaledDeltaTime);
            }
            Init_FPSGuiStyle();
            GUI.Label(new Rect(Screen.width - 65, 0, 60, 20), m_showFps.ToString(), guiStyle);
        }

        if (!m_open)
            return;

        GUI.Box(new Rect(0, 0, panelWidth, Screen.height), "CONFIG PANEL");
        GUI.Label(new Rect(50, 20, 300, 20), Application.productName + " " + Application.version);

        if (GUI.Button(new Rect(panelWidth - 20, 0, 20, 20), "x"))
        {
            m_open = false;
            SaveData();
        }

        // Mode
        toggleDisableHMD = GUI.Toggle(new Rect(panelWidth / 2 - 75, 40, 300, 20), toggleDisableHMD, toggleEnableVRTitle);
        DisableHMD(toggleDisableHMD);

        // FPS
        toggleEnableFPS = GUI.Toggle(new Rect(panelWidth / 2 - 75, 60, 300, 20), toggleEnableFPS, "Show FPS");
    }


    void DisableHMD(bool disable)
    {
        if (disable)
        {
            var player = Valve.VR.InteractionSystem.Player.instance;
            player.SendMessage("ActivateRig", player.rig2DFallback);
            toggleEnableVRTitle = Toggle1Status.MODE_STANDALONE.ToString();

            var snapTurn = FindObjectOfType<Valve.VR.InteractionSystem.SnapTurn>();
            snapTurn.enabled = false;
        }
        else
        {
            var player = Valve.VR.InteractionSystem.Player.instance;
            player.SendMessage("ActivateRig", player.rigSteamVR);
            toggleEnableVRTitle = Toggle1Status.MODE_VR.ToString();

            var snapTurn = FindObjectOfType<Valve.VR.InteractionSystem.SnapTurn>();
            snapTurn.enabled = true;
        }
    }

    void Init_FPSGuiStyle()
    {
        guiStyle.fontSize = 20;
        guiStyle.alignment = TextAnchor.MiddleRight;
        guiStyle.normal.textColor = Color.white;
    }

    void LoadData()
    {
        bool.TryParse(PlayerPrefs.GetString("toggleDisableHMD"), out bool flag1);
        bool.TryParse(PlayerPrefs.GetString("toggleEnableFPS"), out bool flag2);

        toggleDisableHMD = flag1;
        toggleEnableFPS = flag2;

        print("====Load prefs===");
        print("toggleDisableHMD " + toggleDisableHMD);
        print("toggleEnableFPS " + toggleEnableFPS);
    }

    void SaveData()
    {
        Debug.Log("SaveData");
        PlayerPrefs.SetString("toggleDisableHMD", toggleDisableHMD.ToString());
        PlayerPrefs.SetString("toggleEnableFPS", toggleEnableFPS.ToString());
    }
}
