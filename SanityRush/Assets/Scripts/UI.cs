using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour {

    public Image firstDrug;
    public Image secondDrug;

    public Slider drugBar;
    public Image drugTimer;
    public Color drugTimerColor1;
    public Color drugTimerColor2;

    public Text text;
    public Text cornerText;

    public Sprite[] pillSprites;

    private int cursorTimer;
    private float cursorOffset;

    private Player player;
    private float textTimer = 0;

    // Use this for initialization
    void Start () {
        cursorTimer = 0;
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject)
        {
            player = playerObject.GetComponent<Player>();
        }

        text.text = "Escape attempt number " + Global.tryNumber;

        var scenename = SceneManager.GetActiveScene().name;
        var floorname = scenename.Substring(6);

        cornerText.text = "Floor " + floorname;
    }

    // Update is called once per frame
    void Update () {
        // druglevel
        // drugLevel.text = Mathf.RoundToInt(player.DrugLevel).ToString();

        textTimer += Time.deltaTime;
        if (textTimer > 2)
        {
            text.enabled = false;
        }

        if (cursorTimer == 0)
        {
            var max = 50;
            float cursorMaxOffset = 0.02f * Mathf.Abs(player.DrugLevel - (max/2)) / (max/2);
            cursorOffset = Random.Range(-cursorMaxOffset, cursorMaxOffset);
            cursorTimer = Mathf.RoundToInt(Random.Range(0, 5));
            drugBar.value = cursorOffset + player.DrugLevel / max;
        }
        else
        {
            cursorTimer -= 1;
        }

        //drug timer
        // drugTimer.text = player.DrugTimer.ToString();
        drugTimer.fillAmount = player.DrugTimer / 7.0f;
        drugTimer.color = Color.Lerp(drugTimerColor1, drugTimerColor2, player.DrugTimer / 7.0f);

        // drug select
        // firstDrug.text = player.Drug1.ToString();
        // secondDrug.text = player.Drug2.ToString();
        if (pillSprites[(int)player.Drug1])
        {
            firstDrug.enabled = true;
            firstDrug.sprite = pillSprites[(int)player.Drug1];
        }
        else
        {
            firstDrug.enabled = false;
        }

        if (pillSprites[(int)player.Drug2])
        {
            secondDrug.enabled = true;
            secondDrug.sprite = pillSprites[(int)player.Drug2];
        }
        else
        {
            secondDrug.enabled = false;
        }
    }
}
