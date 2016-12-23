using UnityEngine;
using System.Collections;
using System;

public class StartGameUI : UIBase {
    public GameObject startGameBtn;

    protected override void OnLoad(object param = null)
    {
        UIManager.SetButtonClick(startGameBtn, OnStartGame);
        base.OnLoad(param);
    }

    protected override void OnRelease()
    {
        base.OnRelease();
    }

    private void OnStartGame(GameObject obj, int param1, int param2)
    {
        Game.Instance().CreateScene("GameScene", typeof(GameScene));
    }

}
