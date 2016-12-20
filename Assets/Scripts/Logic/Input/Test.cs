using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {
    public KeyboardInput kbInput;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.B))
        {
            kbInput = Game.Instance().GetUpdateObj<KeyboardInput>() as KeyboardInput;
            kbInput.handleInput += handleIp;
        }
	}

    void handleIp(HandleType type)
    {
        Debug.Log("========: " + type.ToString());
    }
}
