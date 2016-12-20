using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// 心跳属性
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class UpdateAttribute : Attribute
{
    public string name;
    public bool autoRegister;

    public UpdateAttribute(string name,bool autoRegister)
    {
        this.name = name;
        this.autoRegister = autoRegister;
    }
}
