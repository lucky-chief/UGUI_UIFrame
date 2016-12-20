using UnityEngine;
using System.Collections.Generic;
using System;
using System.Reflection;

public class ModuleManager
{
    public static readonly ModuleManager Instance = new ModuleManager();

    private List<ModuleBase> m_ModuleList = new List<ModuleBase>();

    private ModuleManager()
    {
    }

    public void Init()
    {
        AutoRegisterModule();
    }

    private void AutoRegisterModule()
    {
        var types = Assembly.GetAssembly(typeof(ModuleManager)).GetTypes();
        foreach (Type type in types)
        {
            object[] attrs = type.GetCustomAttributes(typeof(ModuleAttribute), false);
            foreach (object attr in attrs)
            {
                ModuleAttribute mdAttr = attr as ModuleAttribute;
                if (mdAttr.autoRegister) Register(mdAttr.name);
            }
        }
    }

    private void Register(string moduleName)
    {
        ModuleBase module = Activator.CreateInstance(Type.GetType(moduleName)) as ModuleBase;
        ModuleBase moduleFind = m_ModuleList.Find((ModuleBase findModule) =>
        {
            if (findModule.GetType().Name == moduleName) return true;
            else return false;
        });
        if (null != moduleFind)
        {
            Debug.Log(moduleFind.GetType().ToString() + "已经注册过，请不要重复注册！");
            return;
        }
        module.OnLoad();
        m_ModuleList.Add(module);
        Debug.Log("m_ModuleList: " + m_ModuleList.Count);
    }

    public void Register<T>() where T : ModuleBase, new()
    {
        ModuleBase moduleFind = m_ModuleList.Find((ModuleBase findModule) =>
        {
            if (findModule is T) return true;
            else return false;
        });
        if (null != moduleFind)
        {
            Debug.Log(moduleFind.GetType().ToString() + "已经注册过，请不要重复注册！");
            return;
        }
        T module = new T();
        module.OnLoad();
        m_ModuleList.Add(module);
    }

    public void UnRegister<T>() where T : ModuleBase
    {
        T module = GetModule<T>();
        module.OnRelease();
        m_ModuleList.Remove(module);
    }

    public T GetModule<T>() where T : ModuleBase
    {
        ModuleBase moduleFind = m_ModuleList.Find((ModuleBase findModule) =>
        {
            if (findModule is T) return true;
            else return false;
        });
        return moduleFind as T;
    }
}
