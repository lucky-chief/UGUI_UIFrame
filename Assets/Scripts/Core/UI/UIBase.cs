using UnityEngine;
using System.Collections;
using System;

public abstract class UIBase : MonoBehaviour
{
    public UINames selfName { get;  set; }

	private void OnEnable ()
    {
        OnLoadData();
        OnLoad();
	}

    private void Start()
    {
        OnOpenTween();
    }
	
	private void Update ()
    {
        OnUpdate(Time.deltaTime);
	}

    private void OnDestroy()
    {
    }

    private void OnDisable()
    {
        OnRelease();
    }

    public virtual void OnLoadData(params object[] allParams) { }
    protected virtual void OnLoad(object param = null) { }
    protected virtual void OnRelease() { }
    protected virtual void OnUpdate(float deltaTime) { }
    /// <summary>
    /// 需要给一个默认的UI打开动画，没实现
    /// </summary>
    protected virtual void OnOpenTween() { }
}
