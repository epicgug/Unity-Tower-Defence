using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

	public Text waveText;
	public GC GC;
	public Button cannonButton;
	public bool isMouseOver;
	public static UI localUI;
	public Text testInfoHash;
	public Text goldText;
	public GameObject cannonSprite;
	public bool placingCannon;

	private GameObject temporaryTower;

	public delegate void placeTowerDelegate();

	public placeTowerDelegate PlacingTowerButtonDelegate;
	// Use this for initialization
	void Start () {
		GC = GameObject.Find ("GameController").GetComponent<GC> ();
		cannonButton.onClick.AddListener (TaskOnClick);
		isMouseOver = false;
		UI.localUI = this;
	}

	public void noPlacing () {
		GameObject.DestroyObject (temporaryTower);
		placingCannon = false;
	}

	void cannonTowerPlacing() {
		temporaryTower.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, -4);
	}

	/*
	 * delegate MathDelegate(float x);
	 * 
	 * MathDelegate myDel;
	 * 
	 * float f(x)
	 * 	return 2*x;
	 * 
	 * float g(x)
	 * 	return 2+x;
	 * 
	 * myDel = f;
	 * 
	 * print myDel(3);
	 * equals 6
	 * 
	*/

	
	// Update is called once per frame
	void Update () {
		updateText ();
		if(PlacingTowerButtonDelegate != null)
			PlacingTowerButtonDelegate ();
	}

	void TaskOnClick(){
		Debug.Log ("You have clicked the button!");
		temporaryTower = Instantiate (cannonSprite, Input.mousePosition, Quaternion.identity);
		PlacingTowerButtonDelegate = cannonTowerPlacing;
		placingCannon = true;
	}

	void updateText() {
		waveText.text = "Wave: " + (GC.currentWave + 1);
		goldText.text = "Gold: " + GC.gold;
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
