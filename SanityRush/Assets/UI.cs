using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        var player = GameObject.FindGameObjectWithTag("Player");
        var canvas = FindObjectOfType<Canvas>();

        // druglevel
        var druglevel = canvas.transform.Find("DrugLevel").gameObject;
        druglevel.GetComponent<Text>().text = Mathf.RoundToInt(player.GetComponent<Player>().DrugLevel).ToString();

        //drug timer
        var drugTimer = canvas.transform.Find("DrugTimer").gameObject;
        drugTimer.GetComponent<Text>().text = player.GetComponent<Player>().DrugTimer.ToString();

        // drug select
        var drug1 = canvas.transform.Find("Drug1").gameObject;
        drug1.GetComponent<Text>().text = player.GetComponent<Player>().Drug1.ToString();

        var drug2 = canvas.transform.Find("Drug2").gameObject;
        drug2.GetComponent<Text>().text = player.GetComponent<Player>().Drug2.ToString();
    }
}
