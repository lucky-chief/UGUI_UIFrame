using UnityEngine;
using System.Collections;
using System;

[Update("KeyboardInput", true)]
public class KeyboardInput : InputBase
{
    protected override void OnUpdate()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            Handle(HandleType.handleLeft);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            Handle(HandleType.handleRight);
        }
        else if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            Handle(HandleType.hanelePush);
        }
    }
}
