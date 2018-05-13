﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeButtons : MonoBehaviour {

    public string nextSceneName;

	// Use this for initialization
	void Start () {
		
	}

    public void StartGame()
    {
        Debug.Log("start game");
        SceneManager.LoadScene(nextSceneName);
    }

    public void DoQuit()
    {
        Debug.Log("quitting");
        Application.Quit();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
