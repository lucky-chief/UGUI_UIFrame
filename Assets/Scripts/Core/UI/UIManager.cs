using UnityEngine;
using System.Collections.Generic;

public delegate void ButtonClickHandler(GameObject obj, int param1,int param2);
/// <summary>
/// UI的资源预制都是存放在Resources下的
///如果你的不是，请自行实现UI预制资源的加载
/// </summary>
public class UIManager
{
    const string OPEN_UI = "OPEN_UI";
    const string CLOSE_UI = "CLOSE_UI";

    public static readonly UIManager Instance = new UIManager();
    private Camera uiCamera;

    private Dictionary<UINames, UIBase> m_AllOpenUI = new Dictionary<UINames, UIBase>();
    private Transform m_uiParent = null;
    private bool inited = false;
    private UIManager() { }

    public void InitUIManager(Transform uiParent)
    {
        m_uiParent = uiParent;
        uiCamera = uiParent.parent.FindChild("Camera").GetComponent<Camera>();
        inited = true;
    }

    public Camera UICamera
    {
        get { return uiCamera; }
    }

    public void OpenUI(UINames uiName, bool closeAll = false)
    {
        if (!inited) return;
        if(closeAll) CloseAllUI();
        GameObject ui = UIPool.Instance.SpawnUI(uiName);
        ui.SetActive(true);
        UIBase uiBase = ui.GetComponent<UIBase>();
        uiBase.selfName = uiName;
        ui.transform.SetParent(m_uiParent);
        ui.transform.localScale = Vector3.one;
        ui.transform.localPosition = Vector3.zero;
        RectTransform rectTrans = ui.GetComponent<RectTransform>();
        rectTrans.offsetMax = Vector2.zero;
        rectTrans.offsetMin = Vector2.zero;
        ui.transform.SetAsLastSibling();
        m_AllOpenUI.Add(uiName, uiBase);

        //发送UI打开事件
        Notification notify = new Notification(OPEN_UI);
        notify["uiName"] = uiName;
        notify.Send();
    }

    public void CloseUI(UINames uiName)
    {
        if (!inited) return;
        if(m_AllOpenUI.ContainsKey(uiName))
        {
            UIPool.Instance.DespawnUI(uiName);
            m_AllOpenUI.Remove(uiName);
            //发送UI关闭事件
            Notification notify = new Notification(CLOSE_UI);
            notify["uiName"] = uiName;
            notify.Send();
        }
    }

    public void CloseAllUI()
    {
        foreach(KeyValuePair<UINames,UIBase> ui in m_AllOpenUI)
        {
            UIPool.Instance.DespawnUI(ui.Key);
            //发送UI关闭事件
            Notification notify = new Notification(CLOSE_UI);
            notify["uiName"] = ui.Key;
            notify.Send();
        }
        m_AllOpenUI.Clear();
    }

    public UIBase GetUI(UINames uiName)
    {
        return m_AllOpenUI[uiName];
    }

    public void DetroyUI(UINames uiName)
    {
        UIPool.Instance.DespawnUI(uiName, 0);
    }

    public Transform GetUIRoot()
    {
        return m_uiParent;
    }

    public bool IsOpen(UINames uiName)
    {
        return m_AllOpenUI.ContainsKey(uiName);
    }

    public static void SetButtonClick(GameObject obj,ButtonClickHandler callFunc, int param1 = 0,int param2 = 0)
    {
        ButtonScript btnScript = obj.GetComponent<ButtonScript>();
        if (!btnScript)
        {
            btnScript = obj.AddComponent<ButtonScript>();
        }
        btnScript.SetData(obj, callFunc, param1,param2);
        btnScript.onClick += (GameObject obj1, ButtonClickHandler callFunc1, int param11, int param21) => {
            callFunc1(obj1, param11, param21);
        };
    }

}
