using System.Collections;

public abstract class ModuleBase : ILoad
{
    public abstract void OnLoad();
    public abstract void OnRelease();
}
