using System;
using UnityEngine;
using FSM;

public class ActionOpenUI : FSMAction
{
    public string toOpenUIName;
    public bool closeAll;

    public ActionOpenUI() { }
    public override void OnEnter()
    {
        UINames openUIName = (UINames)Enum.Parse(typeof(UINames), toOpenUIName);
        if (UIManager.Instance.IsOpen(openUIName))    return;
        UIManager.Instance.OpenUI(openUIName, closeAll);
        base.OnEnter();
    }
}
