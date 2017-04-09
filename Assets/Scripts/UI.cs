using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

	public GC GC;
	public static UI localUI;
	public Button cannonButton;
	public Button upgradeButton;
	public bool isMouseOver;
	public Text waveText;
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
		upgradeButton.onClick.AddListener (upgradeButtonClick);
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

	void upgradeButtonClick() {
		GC.local.Selected.upgrade ();
	}

	void TaskOnClick(){
		Debug.Log ("You have clicked the button!");
		temporaryTower = Instantiate (cannonSprite, Input.mousePosition, Quaternion.identity);
		PlacingTowerButtonDelegate = cannonTowerPlacing;
		placingCannon = true;

	}
	
	void Update () {
		if (GC.local.Selected != null) {
			testInfoHash.text = GC.local.Selected.UIInfo;
			testInfoHash.gameObject.SetActive(true);
			upgradeButton.gameObject.SetActive (true);
			cannonButton.gameObject.SetActive(false);
		} else {
			cannonButton.gameObject.SetActive(true);
			upgradeButton.gameObject.SetActive (false);
			testInfoHash.gameObject.SetActive(false);
		}
		updateText ();
		if(PlacingTowerButtonDelegate != null)
			PlacingTowerButtonDelegate ();
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
