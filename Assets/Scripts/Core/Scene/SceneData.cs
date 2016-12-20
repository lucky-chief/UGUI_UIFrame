using System;
using System.Collections.Generic;

public class SceneData
{
    public string sceneName;
    public Type sceneType;
    public SceneData(string sceneName,Type sceneType)
    {
        this.sceneName = sceneName;
        this.sceneType = sceneType;
    }
}
