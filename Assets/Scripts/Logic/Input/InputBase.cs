using UnityEngine;
using System.Collections;
using System;

public enum HandleType
{
    none,
    handleLeft,
    handleRight,
    hanelePush
}
public abstract class InputBase : IUpdate
{
    public delegate void HandleInput(HandleType handleType);

    public event HandleInput handleInput = null;

    protected abstract void OnUpdate();

    protected void Handle(HandleType handleType)
    {
        if(null != handleInput)
        {
            handleInput.Invoke(handleType);
        }
    }

    public void Update(float deltaTime)
    {
        OnUpdate();
    }
}
