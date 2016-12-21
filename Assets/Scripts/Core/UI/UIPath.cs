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
        case UINames.TestUI:
            return "Prefabs/UI/TestUI";

        }
        return path;
    }
}
