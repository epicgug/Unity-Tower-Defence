using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

	public Text waveText;
	public GC GC;
	public Button button;
	public bool isMouseOver;
	public static UI localUI;
	public Text testInfoHash;

	// Use this for initialization
	void Start () {
		GC = GameObject.Find ("GameController").GetComponent<GC> ();
		button.onClick.AddListener (TaskOnClick);
		isMouseOver = false;
		UI.localUI = this;
	}
	
	// Update is called once per frame
	void Update () {
		updateText ();
	}

	void TaskOnClick(){
		Debug.Log ("You have clicked the button!");
	}

	void updateText() {
		waveText.text = "Wave: " + (GC.currentWave + 1);
	}

	public void mouseIsOver() {
		isMouseOver = true;
	}

	public void mouseNotOver() {
		isMouseOver = false;
	}

	public void updateButtons() {
		
	}

}
