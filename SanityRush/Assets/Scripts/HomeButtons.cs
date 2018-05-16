using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeButtons : MonoBehaviour {

    public GameObject startButton;
    public GameObject quitButton;
    public GameObject buttonCursor;

    private GameObject currentButton;
    private Vector3 originalCursorPosition;

	// Use this for initialization
	void Start () {
        currentButton = startButton;
        originalCursorPosition = buttonCursor.transform.position;
	}

    private void StartGame()
    {
        Debug.Log("start game");
        SceneManager.LoadScene(Global.CurrentLevel);
    }

    private void DoQuit()
    {
        Debug.Log("quitting");
        Application.Quit();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Vertical"))
        {
            if (currentButton == startButton)
            {
                currentButton = quitButton;
            }
            else
            {
                currentButton = startButton;
            }

        }

        Vector3 cursorPosition = buttonCursor.transform.position;
        cursorPosition.x = originalCursorPosition.x + 30 * Mathf.Sin(Mathf.PI * 2 * Time.fixedTime);
        cursorPosition.y = currentButton.transform.position.y;
        buttonCursor.transform.position = cursorPosition;

        if (Input.GetButtonDown("Submit"))
        {
            if (currentButton == startButton)
            {
                StartGame();
            }
            else
            {
                DoQuit();
            }
        }
    }
}
