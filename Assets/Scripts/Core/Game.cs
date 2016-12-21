using UnityEngine;
#if UNITY_5_3
using UnityEngine.SceneManagement;
#endif
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using FSM;

public class Game : MonoBehaviour {
    private SceneBase curScene ;
    public SceneBase CurScene{get { return curScene; }}
    public Camera camera2D { get; private set; }
    public Action loadComplete;
    private Dictionary<string, IUpdate> updateObjDic = new Dictionary<string, IUpdate>();
    private Transform container3D;
    public Transform Container3D
    {
        get { return container3D; }
    }

    void Start () {
        Singleton.GetInstance("TimeUtil");
        ModuleManager.Instance.Init();
        AutoRegisterUpdateObj();
        InitUIRoot();
	}

	void Update ()
    {
        if (null != curScene) curScene.OnUpdate(Time.deltaTime);
        List<IUpdate> updateList = new List<IUpdate>(updateObjDic.Values);
        for(int i = 0; i < updateList.Count; i++)
        {
            updateList[i].Update(Time.deltaTime);
        }
	}

    public static Game Instance()
    {
        return Singleton.GetInstance("Game") as Game;
    }

    public void BeginCoroutine(Func<IEnumerator> func)
    {
        StartCoroutine(func());
    }

    public void SetContainer3D(Transform container)
    {
        container3D = container;
    }

    private void InitUIRoot()
    {
        GameObject ui_Root = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/UI/Canvas"));
        uiRoot = ui_Root;
        UIManager.Instance.InitUIManager(uiRoot.transform.FindChild("UI"));
        DontDestroyOnLoad(uiRoot);
    }

    private void AutoRegisterUpdateObj()
    {
        var types = Assembly.GetAssembly(typeof(Game)).GetTypes();
        foreach (Type type in types)
        {
            object[] attrs = type.GetCustomAttributes(typeof(UpdateAttribute), false);
            foreach (object attr in attrs)
            {
                UpdateAttribute mdAttr = attr as UpdateAttribute;
                if (mdAttr.autoRegister) RegisterUpdateObj (mdAttr.name);
            }
        }
    }
    public void RegisterUpdateObj(string updateObjName)
    {
        IUpdate updateObj = Activator.CreateInstance(Type.GetType(updateObjName)) as IUpdate;
        if(!updateObjDic.ContainsKey(updateObjName))
        {
            updateObjDic.Add(updateObjName, updateObj);
        }
        else
        {
            Debug.Log(updateObjName + "已经注册过，请不要重复注册！");
            return;
        }
        Debug.Log("IUpdate: " + updateObjDic.Count);
    }

    public void UnRegisterUpdateObj<T>() where T : IUpdate
    {
        List<IUpdate> updateList = new List<IUpdate>(updateObjDic.Values);
        for (int i = 0; i < updateList.Count; i++)
        {
            if (updateList[i] is T)
            {
                updateObjDic.Remove(updateList[i].ToString());
                break;
            }
        }
    }

    public IUpdate GetUpdateObj<T>() where T : IUpdate
    {
        List<IUpdate> updateList = new List<IUpdate>(updateObjDic.Values);
        for (int i = 0; i < updateList.Count; i++)
        {
            if (updateList[i] is T)
            {
                return updateList[i];
            }
        }
        return default(T);
    }

    public void CreateScene(string sceneName,Type sceneType,Action loadComplete = null)
    {
        toLoadSceneData = new SceneData(sceneName, sceneType) ;
        this.loadComplete = loadComplete == null ? null : loadComplete ;
        BeginCoroutine(LoadScene);
    }

    private IEnumerator LoadScene()
    {
        AsyncOperation opt = null;
#if UNITY_5_3
        opt = SceneManager.LoadSceneAsync(toLoadSceneData.sceneName);
#else
        opt = Application.LoadLevelAsync(toLoadSceneData.sceneName);
#endif
        yield return opt;
        if(null != curScene) curScene.OnRelease();
        curScene = Activator.CreateInstance(toLoadSceneData.sceneType) as SceneBase;
        curScene.OnLoad();
        if(null != loadComplete)
        {
            loadComplete.Invoke();
        }
    }

    private SceneData toLoadSceneData;
    private GameObject uiRoot;
}
