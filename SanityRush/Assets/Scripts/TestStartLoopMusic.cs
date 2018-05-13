using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStartLoopMusic : MonoBehaviour {
    public AudioClip Intro;
   //public AudioClip Loop;
    public GameObject AudioLoop;

	// Use this for initialization
	void Start ()
    {
        GetComponent<AudioSource>().PlayOneShot(Intro);
        AudioLoop.SetActive(false);
        StartCoroutine(StartDelay());

    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(34.8f);
        //GetComponent<AudioSource>().PlayOneShot(Loop);
        AudioLoop.SetActive(true);


    }
}
