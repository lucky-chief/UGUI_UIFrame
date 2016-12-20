public sealed class UIPath
{
    public readonly static UIPath Instance = new UIPath();
    private UIPath() { }

    public string GetUIPath(UINames UIName)
    {
        string path = "";
        switch(UIName)
        {
            case UINames.Snake_Room_SelectUI:
                path = "Prefabs/UI/Room";
                break;
            case UINames.RockerUI:
                path = "Prefabs/UI/Rocker";
                break;
            case UINames.LoginUI:
                path = "Prefabs/UI/LoginUI";
                break;
            case UINames.Draw2DPanelUI:
                path = "Prefabs/UI/2DDrawPanel";
                break;
        }
        return path;
    }
}
