using UnityEngine;
using System.Collections;
using FSM;
using System;
/// <summary>
/// 通过事件来触发条件
/// </summary>
public class TriggeredByNotification : FSMCondition
{
    public string notificationName;
    bool triggered = false;

    public TriggeredByNotification() { }

    public override void OnEnter()
    {
        Messager.Instance.AddNotification(notificationName, OnTrigger);
        base.OnEnter();
    }

    public override void OnExit()
    {
        Messager.Instance.RemoveNotification(notificationName, OnTrigger);
        triggered = false;
        base.OnExit();
    }

    public override bool OnValidate()
    {
        return triggered;
    }

    private void OnTrigger(Notification notify)
    {
        triggered = true;
    }
}
