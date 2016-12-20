using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public delegate void OnClickButtonScript(GameObject obj, ButtonClickHandler handler, int param1,int param2);

public class ButtonScript : MonoBehaviour, IPointerClickHandler
{
    public event OnClickButtonScript onClick;

    private GameObject obj;
    private ButtonClickHandler handler;
    private int param1;
    private int param2;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (onClick != null)
        {
            onClick(obj, handler, param1,param2);
        }
    }

    public void SetData(GameObject obj, ButtonClickHandler handler, int param1, int param2)
    {
        this.obj = obj;
        this.handler = handler;
        this.param1 = param1;
        this.param2 = param2;
    }
}
