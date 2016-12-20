using UnityEngine;
using System.Collections;
using System;

public abstract class SceneBase : ILoad
{
    public virtual void OnLoad() { }
    public virtual void OnRelease() { }
    public virtual void OnUpdate(float deltaTime) { }
}
