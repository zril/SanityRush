using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(GameObject.Find("MusicDrogue"));
            Global.CurrentLevel = 1;
            Global.tryNumber = 1;
            SceneManager.LoadScene(0);
        }
    }
}
