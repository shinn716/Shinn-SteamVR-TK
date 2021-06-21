// Athor: John Tsai
// Base on ShiHand.
// Modify from https://www.youtube.com/watch?v=ryfUXr5yvKw

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class SteamvrInputModule : VR_InputModule
{
    public static SteamvrInputModule instance;

    public ShiHand[] shiHands;

    protected override void Awake()
    {
        instance = this;
        shiHands = FindObjectsOfType<ShiHand>();
    }

    public override void Process()
    {
        base.Process();

        foreach (var i in shiHands)
        {
            if (i.TriggerAction.GetStateDown(i.GetPose.inputSource))
                Press();

            if (i.TriggerAction.GetStateUp(i.GetPose.inputSource))
                Release();
        }
    }
}
