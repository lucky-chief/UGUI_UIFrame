using UnityEngine;
using System.Collections;

public class GameScene : SceneBase {

    public override void OnLoad()
    {
        UIManager.Instance.CloseAllUI();
        UIManager.Instance.OpenUI(UINames.GameUI);
        base.OnLoad();
    }
}
