using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

    public Text drugLevel;
    public Text drugTimer;
    public Text firstDrug;
    public Text secondDrug;

    public Slider drugBar;

    public Player player;

    private int cursorTimer;
    private float cursorOffset;

	// Use this for initialization
	void Start () {
        cursorTimer = 0;
		
	}
	
	// Update is called once per frame
	void Update () {
        // druglevel
        drugLevel.GetComponent<Text>().text = Mathf.RoundToInt(player.DrugLevel).ToString();

        if (cursorTimer == 0)
        {
            float cursorMaxOffset = 0.02f * Mathf.Abs(player.DrugLevel - 50) / 50f;
            cursorOffset = Random.Range(-cursorMaxOffset, cursorMaxOffset);
            cursorTimer = Mathf.RoundToInt(Random.Range(0, 5));
            drugBar.value = cursorOffset + player.DrugLevel / 100f;
        }
        else
        {
            cursorTimer -= 1;
        }

        //drug timer
        drugTimer.GetComponent<Text>().text = player.DrugTimer.ToString();

        // drug select
        firstDrug.GetComponent<Text>().text = player.Drug1.ToString();
        secondDrug.GetComponent<Text>().text = player.Drug2.ToString();
    }
}
