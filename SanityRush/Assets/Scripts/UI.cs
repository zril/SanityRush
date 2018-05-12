using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

    public Text drugLevel;
    public Text drugTimer;
    public Text firstDrug;
    public Text secondDrug;

    public RectTransform drugBar;
    public RectTransform drugCursor;

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
            float cursorMaxOffset = 10 * Mathf.Abs(player.DrugLevel - 50) / 50f;
            cursorOffset = Random.Range(-cursorMaxOffset, cursorMaxOffset);
            cursorTimer = Mathf.RoundToInt(Random.Range(0, 5));
        }
        else
        {
            cursorTimer -= 1;
        }

        float barWidth = drugBar.rect.width / 2;
        Rect newRect = drugCursor.rect;
        drugCursor.localPosition = new Vector3(cursorOffset + barWidth * (-50 + player.DrugLevel) / 50f, 0, 0);

        //drug timer
        drugTimer.GetComponent<Text>().text = player.DrugTimer.ToString();

        // drug select
        firstDrug.GetComponent<Text>().text = player.Drug1.ToString();
        secondDrug.GetComponent<Text>().text = player.Drug2.ToString();
    }
}
