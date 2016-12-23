using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Singleton.GetInstance("Game");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
