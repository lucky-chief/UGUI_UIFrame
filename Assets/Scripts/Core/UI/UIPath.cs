public sealed class UIPath
{
    public readonly static UIPath Instance = new UIPath();
    private UIPath() { }

    public string GetUIPath(UINames UIName)
    {
        string path = "";
        switch(UIName)
        {
                    case UINames.Canvas:
            return "Prefabs/UI/Canvas";
        case UINames.GameUI:
            return "Prefabs/UI/GameUI";
        case UINames.StartGameUI:
            return "Prefabs/UI/StartGameUI";

        }
        return path;
    }
}
