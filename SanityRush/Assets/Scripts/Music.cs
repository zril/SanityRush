using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour {

    private float timer;
    private bool introfinished = false;

    // Use this for initialization
    void Start()
    {
        timer = 0;
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 36f && !introfinished)
        {
            GameObject.FindGameObjectWithTag("Blue").GetComponent<AudioSource>().Play();
            GameObject.FindGameObjectWithTag("Yellow").GetComponent<AudioSource>().Play();
            GameObject.FindGameObjectWithTag("Black").GetComponent<AudioSource>().Play();
            GameObject.FindGameObjectWithTag("Red").GetComponent<AudioSource>().Play();
            GameObject.FindGameObjectWithTag("Glitch").GetComponent<AudioSource>().Play();

            GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>().Play();

            introfinished = true;
        }
    }

}
