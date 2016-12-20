using UnityEngine;
using System.Collections;
using UnityEditor;

public class CreateUIAtHierarchy {
    static GameObject selectedGameObject = null;

    [MenuItem("Windows/Create A New UI")]
    static void NewUI()
    {
        NewUIWindow.Init();
    }

    [InitializeOnLoadMethod]
    static void StartInitializeOnLoadMethod()
    {
        EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
    }

    static void OnHierarchyGUI(int instanceID, Rect selectionRect)
    {
        if (Event.current != null && selectionRect.Contains(Event.current.mousePosition)
            && Event.current.button == 1 && Event.current.type <= EventType.mouseUp)
        {
            selectedGameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            //这里可以判断selectedGameObject的条件
            if (selectedGameObject && selectedGameObject.transform.parent && selectedGameObject.transform.parent.name == "Canvas" && selectedGameObject.name == "UI")
            {
                Vector2 mousePosition = Event.current.mousePosition;

                EditorUtility.DisplayPopupMenu(new Rect(mousePosition.x, mousePosition.y, 0, 0), "Windows/", null);
                Event.current.Use();
            }
        }
    }
}
