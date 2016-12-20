using System.Collections.Generic;

public class Notification
{
    private string m_NotificationName;
    private Dictionary<string, object> m_attributes = new Dictionary<string, object>();

    public Notification(string notificationName,object sender = null)
    {
        m_NotificationName = notificationName;
        this["sender"] = sender;
    }

    public void Send()
    {
        Messager.Instance.Excute(m_NotificationName, this);
    }

    public object this[string key]
    {
        set
        {
            m_attributes[key] = value;
        }
        get
        {
            if(m_attributes.ContainsKey(key))
            {
                return m_attributes[key];
            }
            return null;
        }
    }
	
}
