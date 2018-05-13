using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    private Dictionary<string, AudioSource> loops;

    // Use this for initialization
    void Start () {
        loops = new Dictionary<string, AudioSource>();

        GameObject walkSoundObject = GameObject.FindGameObjectWithTag("WalkSound");
        AudioSource walkSound = walkSoundObject.GetComponent<AudioSource>();
        walkSound.enabled = false;
        loops.Add("Walk", walkSound);
    }

    public void playLoop(string loopName)
    {
        loops[loopName].enabled = true;
    }

    public void stopLoop(string loopName)
    {
        loops[loopName].enabled = false;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
