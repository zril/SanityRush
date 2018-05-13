using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayStartLoopMusic : MonoBehaviour {
    public AudioClip Intro;
    public AudioClip Loop;
    //public GameObject AudioLoop;

	// Use this for initialization
	void Start ()
    {
        GetComponent<AudioSource>().PlayOneShot(Intro);
        StartCoroutine(StartDelay());

    }
	
	// Update is called once per frame
	void Update ()
    {
       
	}

    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(34.8f);
        GetComponent<AudioSource>().PlayOneShot(Loop);
        
       


    }
}
