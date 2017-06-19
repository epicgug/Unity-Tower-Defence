using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

	public GC GC;
	public static UI localUI;

	public Button cannonButton;
	public Button AoEButton;
	public Button mortarButton;

	public Button damageUpgradeButton;
	public Button rangeUpgradeButton;
	public Button shotSpeedUpgradeButton;
	public Button changeMortarTarget;

	public Text waveText;
	public Text testInfoHash;
	public Text goldText;
	public Text nextWave;

	public GameObject cannonSprite;
	public GameObject AoESprite;
	public GameObject mortarSprite;

	public bool isMouseOver;
	public bool placingCannon;
	public bool placingAoE;
	public bool placingMortar;
	public string towerBeingPlaced;

	public GameObject upgradePanel;
	public GameObject towerPanel;

	private GameObject temporaryTower;

	public ThingsToDoNext thingToDoNext;

	public delegate void placeTowerDelegate();

	public enum ThingsToDoNext {
		NOTHING=0,
		PLACE_CANON=1,
		PLACE_AOE=2,
		PLACE_MORTAR=3,
		PLACE_MORTAR_TARGET=4
	}

	public placeTowerDelegate PlacingTowerButtonDelegate;
	// Use this for initialization
	void Start () {
		GC = GameObject.Find ("GameController").GetComponent<GC> ();
		cannonButton.onClick.AddListener (CannonButtonClick);
		AoEButton.onClick.AddListener (AoEButtonClick);
		mortarButton.onClick.AddListener (MortarButtonClick);
		damageUpgradeButton.onClick.AddListener (upgradeDamageButtonClick);
		rangeUpgradeButton.onClick.AddListener (upgradeRangeButtonClick);
		shotSpeedUpgradeButton.onClick.AddListener (upgradeShotSpeedButtonClick);
		changeMortarTarget.onClick.AddListener (mortarTargetButton);
		isMouseOver = false;
		UI.localUI = this;
		changeMortarTarget.gameObject.SetActive (false);
		thingToDoNext = ThingsToDoNext.NOTHING;
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

	void MortarTowerPlacing() {
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

	void mortarTargetButton() {
		thingToDoNext = ThingsToDoNext.PLACE_MORTAR_TARGET;
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

	void MortarButtonClick() {
		if(temporaryTower != null) {
			GameObject.DestroyObject (temporaryTower);
		}
		temporaryTower = Instantiate (mortarSprite, Input.mousePosition, Quaternion.identity);
		PlacingTowerButtonDelegate = MortarTowerPlacing;
		towerBeingPlaced = "Mortar";
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
			if(GC.local.Selected.type.Contains("Mortar Tower")) {
				changeMortarTarget.gameObject.SetActive (true);
			}
		} else {
			towerPanel.gameObject.SetActive (true);
			upgradePanel.gameObject.SetActive (false);
			testInfoHash.gameObject.SetActive (false);
			changeMortarTarget.gameObject.SetActive (false);
		}
		updateText ();
		if (PlacingTowerButtonDelegate != null)
			PlacingTowerButtonDelegate ();

		if(thingToDoNext == ThingsToDoNext.PLACE_MORTAR_TARGET) {
			
		}
	}

	void updateText() {
		waveText.text = "Wave: " + (GC.currentWave + 1);
		goldText.text = "Gold: " + GC.gold;
		nextWave.text = "Next Wave: " + "\n" + "Goblins: " + GC.local.waves [GC.local.currentWave + 1].numGoblins + "\n "+ "Rock Monsters: " + GC.local.waves [GC.local.currentWave + 1].numRockMonsters;
	}


	public void mouseIsOver() {
		isMouseOver = true;
	}

	public void mouseNotOver() {
		isMouseOver = false;
	}

	public void updateButtons() {
		
	}

	IEnumerator WaitForKeyUp(KeyCode keyCode)
	{
		while (!Input.GetKeyUp(keyCode))
			yield return null;
	}
}
