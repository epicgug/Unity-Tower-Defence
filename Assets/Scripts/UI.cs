using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

	public Text waveText;
	public GC GC;

	// Use this for initialization
	void Start () {
		GC = GameObject.Find ("GameController").GetComponent<GC> ();
	}
	
	// Update is called once per frame
	void Update () {
		updateText ();
	}

	void updateText() {
		waveText.text = "Wave: " + (GC.currentWave + 1);
	}
}
