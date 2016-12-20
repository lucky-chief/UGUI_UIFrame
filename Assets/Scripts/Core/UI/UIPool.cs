using UnityEngine;
using System.Collections.Generic;

public class UIPool
{
    public static readonly UIPool Instance = new UIPool();
    private Dictionary<UINames, UIBase> m_UIMap = new Dictionary<UINames, UIBase>();

    private UIPool() { }

    public GameObject SpawnUI(UINames uiName)
    {
        if (m_UIMap.ContainsKey(uiName))
        {
            return m_UIMap[uiName].gameObject;
        }
        else
        {
            string uiPath = UIPath.Instance.GetUIPath(uiName);
            if (string.IsNullOrEmpty(uiPath))
            {
                Debug.LogError("没有定义名为【" + uiName.ToString() + "】的UI路径");
                return null;
            }
            else
            {
                GameObject ui = Resources.Load<GameObject>(uiPath);
                if (null == ui)
                {
                    Debug.LogError("您要实例化名为【" + uiName.ToString() + "】的UI失败，请检查路径【" + uiPath + "】是否正确");
                    return null;
                }
                ui = GameObject.Instantiate(ui);
                return ui;
            }
        }
    }

    public void DespawnUI(UINames uiNames, float destroyTime = 10.0f)
    {
        TimeUtil timeUtil = Singleton.GetInstance("TimeUtil") as TimeUtil;
        UIBase ui = UIManager.Instance.GetUI(uiNames);
        timeUtil.AddTimeCountDown(destroyTime, () => {
            if (null == ui || null == ui.gameObject || ui.gameObject.activeSelf) return;
            m_UIMap.Remove(ui.selfName);
            GameObject.Destroy(ui.gameObject);
        });
        ui.gameObject.SetActive(false);
        if(!m_UIMap.ContainsKey(ui.selfName))
        {
            m_UIMap.Add(ui.selfName, ui);
        }
    }

    public void RemoveUI(UINames uiName)
    {
        m_UIMap.Remove(uiName);
    }
}
