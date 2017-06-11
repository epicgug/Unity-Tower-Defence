using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

	public GC GC;
	public static UI localUI;
	public Button cannonButton;
	public Button AoEButton;
	public Button damageUpgradeButton;
	public Button rangeUpgradeButton;
	public Button shotSpeedUpgradeButton;
	public bool isMouseOver;
	public Text waveText;
	public Text testInfoHash;
	public Text goldText;
	public GameObject cannonSprite;
	public GameObject AoESprite;
	public bool placingCannon;
	public bool placingAoE;
	public string towerBeingPlaced;

	public GameObject upgradePanel;
	public GameObject towerPanel;

	private GameObject temporaryTower;

	public delegate void placeTowerDelegate();

	public placeTowerDelegate PlacingTowerButtonDelegate;
	// Use this for initialization
	void Start () {
		GC = GameObject.Find ("GameController").GetComponent<GC> ();
		cannonButton.onClick.AddListener (CannonButtonClick);
		AoEButton.onClick.AddListener (AoEButtonClick);
		damageUpgradeButton.onClick.AddListener (upgradeDamageButtonClick);
		rangeUpgradeButton.onClick.AddListener (upgradeRangeButtonClick);
		shotSpeedUpgradeButton.onClick.AddListener (upgradeShotSpeedButtonClick);
		isMouseOver = false;
		UI.localUI = this;
	}

	public void noPlacing () {
		GameObject.DestroyObject (temporaryTower);
		towerBeingPlaced = "None";
	}

	void cannonTowerPlacing() {
		temporaryTower.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, -4);
	}

	void AoETowerPlacing() {
		temporaryTower.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, -4);
	}

	void upgradeDamageButtonClick() {
		if(GC.local.upgradables.ContainsKey(GC.local.Selected.GetHashCode())) 
			GC.local.upgradables[GC.local.Selected.GetHashCode()].upgradeDamage();
	}

	void upgradeShotSpeedButtonClick() {
		if(GC.local.upgradables.ContainsKey(GC.local.Selected.GetHashCode())) 
			GC.local.upgradables[GC.local.Selected.GetHashCode()].upgradeShotSpeed();
	}

	void upgradeRangeButtonClick() {
		if(GC.local.upgradables.ContainsKey(GC.local.Selected.GetHashCode())) 
			GC.local.upgradables[GC.local.Selected.GetHashCode()].upgradeRange();		
	}

	void CannonButtonClick(){
		if(temporaryTower != null) {
			GameObject.DestroyObject (temporaryTower);
		}
		temporaryTower = Instantiate (cannonSprite, Input.mousePosition, Quaternion.identity);
		PlacingTowerButtonDelegate = cannonTowerPlacing;
		towerBeingPlaced = "Cannon";
	}

	void AoEButtonClick() {
		if(temporaryTower != null) {
			GameObject.DestroyObject (temporaryTower);
		}
		temporaryTower = Instantiate (AoESprite, Input.mousePosition, Quaternion.identity);
		PlacingTowerButtonDelegate = AoETowerPlacing;
		towerBeingPlaced = "AoE";
	}
	
	void Update () {
		if (GC.local.Selected != null) {
			testInfoHash.text = GC.local.Selected.UIInfo;
			testInfoHash.gameObject.SetActive (true);
			towerPanel.gameObject.SetActive (false);
			if (GC.local.Selected.type.Contains("Tower")) {
				upgradePanel.gameObject.SetActive (true);
			}
			else {
				upgradePanel.gameObject.SetActive (!true);
			}
		} else {
			towerPanel.gameObject.SetActive (true);
			upgradePanel.gameObject.SetActive (false);
			testInfoHash.gameObject.SetActive (false);
		}
		updateText ();
		if (PlacingTowerButtonDelegate != null)
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
