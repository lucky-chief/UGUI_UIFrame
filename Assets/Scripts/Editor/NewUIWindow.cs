using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Reflection;

public class NewUIWindow : EditorWindow {
    public static GameObject UIParent;
    static Vector2 windowSize = new Vector2(300, 300);
    const string UI_PREFAB_ROOT = "Prefabs/UI/";

	public static void Init()
    {
        EditorWindow wind = EditorWindow.GetWindowWithRect<NewUIWindow>(new Rect((Screen.width - windowSize.x) * 0.5f, (Screen.height - windowSize.y) * 0.5f, windowSize.x, windowSize.y), true, "创建新UI", true);
        wind.Show();
    }

    string uiName = "";
    GameObject uiGameObject = null;
    bool waitForScriptsFresh;
    float waitScriptTime = 8.0f;//不是秒，只是达到延时的效果

    void OnGUI()
    {
        GUI.Label(new Rect(5, 22, 50, 20), "新UI名称:");
        uiName = GUI.TextField(new Rect(80, 20, 200, 20), uiName);
        if(GUI.Button(new Rect(120, 60, 50, 30), "创建"))
        {
            if(string.IsNullOrEmpty(uiName))
            {
                Debug.LogError("UI名称不能为空！");
                return;
            }
            Create_UI_Script();
        }
        if(GUI.Button(new Rect(10,60,50,30),"生成脚本"))
        {
            Create_UINames_Script();
        }
        GUI.TextArea(new Rect(5, 110, 295, 150), "单击 创建 按钮，可以完成一下事情：\n1、生成一个UI预制,存放路径：Resources/Prefabs/UI,没有则创建；\n2、生成对应的UI脚本，脚本名与填写的UI名称形同，并挂在刚刚生成的UI预制上，存放路径：Scripts/Logic/UI；\n3、在UINames的脚本中添加对应的ui类型，即为填写的UI名称；\n4、在UIPath脚本中生成对应的路径代码。");
    }

    void Update()
    {
        //创建完脚本之后需要等待Unity识别新添加的脚本，否则会导致从程序集里获取不到
        //该脚本，从而无法挂载到创建的UI上。
        if(waitForScriptsFresh)
        {
            if(waitScriptTime > 0)
            {
                waitScriptTime -= 0.02f;
            }
            else
            {
                waitForScriptsFresh = false;
                Create_UI_GameObject();
                Create_UI_Prefab();
                Create_UINames_Script();
                Create_UIPath_Script();
            }
        }
    }

    void Create_UINames_Script()
    {
        string[] files = Directory.GetFiles(Application.dataPath + "/Resources/Prefabs/UI/");
        string uiNamestemplatePath = Application.dataPath + "/Scripts/Editor/TemplateUINames.txt";
        string uiPathtemplatePath = Application.dataPath + "/Scripts/Editor/TemplateUIPath.txt";
        string uiNamesCs = File.ReadAllText(uiNamestemplatePath);
        string uiPathCs = File.ReadAllText(uiPathtemplatePath);
        string enumBodies = "";
        string pathBodies = "";
        foreach (string file in files)
        {
            if (!file.EndsWith(".meta"))
            {
                string[] directoryRootNames = file.Split('/');
                string fileName = directoryRootNames[directoryRootNames.Length - 1].Split('.')[0];
                Debug.Log(fileName);
                enumBodies += "    " + fileName + ",\r\n";
                pathBodies += "        case UINames." + fileName + ":\r\n            return \"" + UI_PREFAB_ROOT + fileName + "\";\r\n";   
            }
        }
        uiNamesCs = uiNamesCs.Replace("$UI_NAMES$", enumBodies);
        uiPathCs = uiPathCs.Replace("$CASE_PLACE$", pathBodies);

        string path = Application.dataPath + "/Scripts/Core/UI/UINames.cs";
        byte[] byteArray = System.Text.Encoding.Default.GetBytes(uiNamesCs);
        FileStream stream = File.Create(path);
        stream.Write(byteArray, 0, uiNamesCs.Length);
        stream.Close();

        path = Application.dataPath + "/Scripts/Core/UI/UIPath.cs";
        FileStream stream2 = File.Create(path);
        byteArray = System.Text.Encoding.Default.GetBytes(uiPathCs);
        stream2.Write(byteArray, 0, uiPathCs.Length);
        stream2.Close();
        AssetDatabase.Refresh();
    }

    void Create_UIPath_Script()
    {

    }

    void Create_UI_Script()
    {
        CheckUIScriptPath();
        string path = Application.dataPath + "/Scripts/Logic/UI/" + uiName + ".cs";
        if (File.Exists(path ))
        {
            File.Delete(path);
        }
        string templatePath = Application.dataPath + "/Scripts/Editor/TemplateUI.txt";
        string cs = File.ReadAllText(templatePath);
        cs = cs.Replace("$CLASS_NAME$", uiName);

        byte[] byteArray = System.Text.Encoding.Default.GetBytes(cs);
        FileStream stream = File.Create(path);
        stream.Write(byteArray, 0,cs.Length);
        stream.Close();
        AssetDatabase.Refresh();
        waitForScriptsFresh = true;
    }

    void CheckUIScriptPath()
    {
        string path = Application.dataPath + "/Scripts/Logic/UI";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    void Create_UI_Prefab()
    {
        string path = "Assets/Resources/Prefabs/UI/" + uiName + ".prefab";
        PrefabUtility.CreatePrefab(path, uiGameObject,ReplacePrefabOptions.ReplaceNameBased);
    }

    void Create_UI_GameObject()
    {
        GameObject newUI = new GameObject();
        RectTransform rectTrans = newUI.AddComponent<RectTransform>();
        newUI.transform.SetParent(GameObject.Find("Canvas/UI").transform);
        newUI.transform.localPosition = Vector3.zero;
        newUI.transform.localScale = Vector3.one;
        rectTrans.anchorMax = Vector2.one;
        rectTrans.anchorMin = Vector2.zero;
        newUI.name = uiName;
        System.Type t = Assembly.GetAssembly(typeof(Game)).GetType(uiName,true);
        newUI.AddComponent(t);
        uiGameObject = newUI;
    }
}
