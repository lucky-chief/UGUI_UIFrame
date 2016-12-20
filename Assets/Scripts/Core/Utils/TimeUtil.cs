using UnityEngine;
using System.Collections.Generic;

public class TimeCountDownData
{
    public System.Action action;
    public float endTime;
    public float seconds;
    public bool loop;
}
public class TimeUtil : MonoBehaviour
{
    private List<TimeCountDownData> m_timeCountDownList = new List<TimeCountDownData>();

    void Start()
    {

    }

    void Update()
    {
        for (int i = 0; i < m_timeCountDownList.Count; i++)
        {
            TimeCountDownData item = m_timeCountDownList[i];
            float nowTime = Time.time;
            if (nowTime >= item.endTime)
            {
                item.action.Invoke();
                if (item.loop)
                {
                    item.endTime = nowTime + item.seconds;
                    continue;
                }
                m_timeCountDownList.RemoveAt(i--);
            }
        }
    }

    public void AddTimeCountDown(float seconds, System.Action action, bool loop = false)
    {
        for (int i = 0; i < m_timeCountDownList.Count; i++)
        {
            TimeCountDownData item = m_timeCountDownList[i];
            if (item.action == action) return;
        }
        TimeCountDownData data = new TimeCountDownData();
        data.action = action;
        data.endTime = Time.time + seconds;
        data.loop = loop;
        data.seconds = seconds;
        m_timeCountDownList.Add(data);

    }

    public void RemoveTimeCountDown(System.Action action)
    {
        TimeCountDownData data = null;
        for (int i = 0; i < m_timeCountDownList.Count; i++)
        {
            TimeCountDownData item = m_timeCountDownList[i];
            if (item.action == action)
            {
                data = item;
                break;
            }
        }
        m_timeCountDownList.Remove(data);
    }
}
