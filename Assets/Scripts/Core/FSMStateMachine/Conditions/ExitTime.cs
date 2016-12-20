using UnityEngine;
using System.Collections;
using FSM;
using System;

public class ExitTime : FSMCondition
{
    public float time;

    private float enterTime;

    public ExitTime() { }

    public override void OnEnter()
    {
        enterTime = Time.time;
        base.OnEnter();
    }

    public override bool OnValidate()
    {
        return Time.time - enterTime >= time;
    }
}
