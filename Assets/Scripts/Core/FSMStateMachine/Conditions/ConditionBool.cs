using UnityEngine;
using System.Collections;
using FSM;
using System;

public class ConditionBool : FSMCondition
{
    public bool flag;

    public ConditionBool()
    {
    }

    public override bool OnValidate()
    {
        return flag;
    }
}
