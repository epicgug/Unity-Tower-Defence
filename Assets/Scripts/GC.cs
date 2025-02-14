﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GC : MonoBehaviour {
	[System.Serializable]
	public class Wave {
		public GameObject goblin;
		public GameObject rockMonster;

		public float goblinSpawnInterval;
		public float rockMonsterSpawnInterval;
		
		public float numGoblins;
		public float numRockMonsters;
	}

	public static GC local;

	public Dictionary<string, float> towerCosts;
	public Dictionary<string, GameObject> towerObjects;
	public float startingGold;
	public float gold;

	public bool canPlaceTower;
	public float timeBetweenWaves;
	public bool gameStart;
	public int currentWave = 0;

	public Wave[] waves;
	public static Vector2[] nodes;
	public GameObject goblin, cannonTower, AoETower, mortarTower;
	public Dictionary<int, IUpgradable> upgradables;

	private int goblinsSpawned;
	private int rockMonstersSpawned;
	private float goblinLastSpawnTime;
	private float rockMonsterLastSpawnTime;
	private float lastSpawnTime;

	delegate void TowerPlacementDelegate();

	private ISelectable selected;


	public ISelectable Selected {
		get {
			return this.selected;
		}

		set {
			if(this.selected != null) {
				this.selected.Deselect ();
			}
			this.selected = value;
//			Debug.Log ("this is selected: " + this.selected);
			if (this.selected != null) {
				this.selected.Select ();
			} else {
//				UI.localUI.testInfoHash.text = "";
			}
		}
	}

	// Use this for initialization

	void waveManager() {
		if (currentWave < waves.Length) {
			float waveElapsedTime = Time.time - lastSpawnTime;
			float goblinTimeInterval = Time.time - goblinLastSpawnTime;
			float rockMonsterTimeInterval = Time.time - rockMonsterLastSpawnTime;
			float goblinSpawnInterval = waves[currentWave].goblinSpawnInterval;
			float rockMonsterSpawnInterval = waves[currentWave].rockMonsterSpawnInterval;

			if(((goblinTimeInterval > goblinSpawnInterval)
					&& (getTotalEnemiesSpawned() != 0 || waveElapsedTime > timeBetweenWaves))
					&& goblinsSpawned < waves[currentWave].numGoblins) {
				Instantiate(waves[currentWave].goblin);
				goblinsSpawned++;
				goblinLastSpawnTime = Time.time;
				lastSpawnTime = Time.time;
			}

			if(((rockMonsterTimeInterval > rockMonsterSpawnInterval)
					&& (getTotalEnemiesSpawned() != 0 || waveElapsedTime > timeBetweenWaves))
					&& rockMonstersSpawned < waves[currentWave].numRockMonsters) {
				Instantiate(waves[currentWave].rockMonster);
				rockMonstersSpawned++;
				rockMonsterLastSpawnTime = Time.time;
				lastSpawnTime = Time.time;
			}
			
			if (rockMonstersSpawned == waves[currentWave].numRockMonsters
					&& goblinsSpawned == waves[currentWave].numGoblins
					&& GameObject.FindGameObjectWithTag("Enemy") == null) {
				currentWave++;
				Debug.Log(currentWave);
				// gameManager.Gold = Mathf.RoundToInt(gameManager.Gold * 1.1f);
				goblinsSpawned = 0;
				rockMonstersSpawned = 0;
				lastSpawnTime = Time.time;
				goblinLastSpawnTime = Time.time;
				rockMonsterLastSpawnTime = Time.time;
				// StartCoroutine(waitForXSeconds(timeBetweenWaves));
  			}
		} else {
			//TODO: End game
		}
	}

	private int getTotalEnemiesSpawned() {
		return rockMonstersSpawned + goblinsSpawned;
	}

	IEnumerator waitForXSeconds(float seconds) {	
		yield return new WaitForSeconds(seconds);
	}

	void Awake () {
		GC.nodes = GameObject.Find ("Path").GetComponent<Path> ().nodes;
	}

	

	void Start () {
		towerCosts = new Dictionary<string, float> ();
		towerCosts.Add ("Cannon", 60);
		towerCosts.Add ("AoE", 80);
		towerCosts.Add ("Mortar", 100);
		towerObjects = new Dictionary<string, GameObject> ();
		towerObjects.Add ("Cannon", cannonTower);
		towerObjects.Add ("AoE", AoETower);
		towerObjects.Add ("Mortar", mortarTower);

		GC.local = this;
		lastSpawnTime = Time.time;
		goblinLastSpawnTime = Time.time;
		rockMonsterLastSpawnTime = Time.time;
		gold = startingGold;
		upgradables = new Dictionary<int, IUpgradable> ();
	}

	// Update is called once per frame
	void Update () {
		if(!gameStart) {
			createTowers ();
			waveManager();
			checkForSelection ();
		}
	}

	void makeGoblin() {
		Instantiate (goblin);
	}

	void holdTowerUnderMouse() {
		
	}

	void checkForSelection() {
		if(!UI.localUI.isMouseOver
			&& Input.GetMouseButtonDown(0)) {
			RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			foreach (RaycastHit2D hit in hits) {
				if (hit.collider.gameObject.tag == "Tower") {
					this.Selected = hit.collider.gameObject.GetComponent<Tower> ();
					UI.localUI.thingToDoNext = UI.ThingsToDoNext.NOTHING;
					return;
				} else if (hit.collider.gameObject.tag == "Enemy") {
					this.Selected = hit.collider.gameObject.GetComponent<Enemy> ();
					UI.localUI.thingToDoNext = UI.ThingsToDoNext.NOTHING;
					return;
				} else {
					if (UI.localUI.thingToDoNext == UI.ThingsToDoNext.PLACE_MORTAR_TARGET) {
						UI.localUI.thingToDoNext = UI.ThingsToDoNext.NOTHING;
						return;
					}
					if(this.Selected != null) {
						this.Selected = null;
					}
					UI.localUI.thingToDoNext = UI.ThingsToDoNext.NOTHING;
				}
			}
		}
	}

	void removeGold(int amount) {
		gold -= amount;
	}

	void addGold(int amount) {
		gold += amount;
	}

	void createTowers() {
		if(!UI.localUI.isMouseOver && Input.GetButtonDown("Fire1")) {
			if (UI.localUI.thingToDoNext == UI.ThingsToDoNext.PLACE_MORTAR_TARGET) {
				Mortar mortar = (Mortar)selected;
				mortar.ChangeTarget ();
				return;
			}
			// Physics.Raycast(transform.position, , )
//			Vector3 towerPosition = Input.mousePosition;
//			towerPosition.z = 0;
//			Camera.main.ScreenToWorldPoint (towerPosition);
//			Ray ray = Camera.main.ScreenPointToRay (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0));
//			RaycastHit hit;
//			if(Physics.Raycast(ray, out hit, 1000f)) {
//				if(hit.collider.gameObject.name == "Path Collision") {
//					Debug.Log ("Hit: " + hit.transform.position);
//					position.z = 0;
//					Instantiate (cannonTower, hit.transform.position, Quaternion.identity);			
//				} 
//			}
			RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			foreach(RaycastHit2D hit in hits) {
				if (hit.collider.gameObject.name != "Land Collision") {
					continue;
				}
				Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				Vector3 smoothPoint = Grid.local.remapPoint (mousePos);
				if (!Grid.local.placeTower (smoothPoint)) {
					continue;
				}

				if (UI.localUI.towerBeingPlaced == "None") {
					continue;
				}
				Tower newTower = Instantiate (towerObjects[UI.localUI.towerBeingPlaced], new Vector3 (smoothPoint.x, smoothPoint.y, 0), Quaternion.identity).GetComponent<Tower>();
				if(newTower.name.Contains("Mortar")) {
					Mortar newMortarTower = (Mortar)newTower;
					UI.localUI.thingToDoNext = UI.ThingsToDoNext.PLACE_MORTAR_TARGET;
					newTower.transform.SetParent(GC.local.transform);
				}
				upgradables.Add (newTower.GetHashCode (), newTower);
				gold -= towerCosts[UI.localUI.towerBeingPlaced];
				UI.localUI.PlacingTowerButtonDelegate = UI.localUI.noPlacing;
			}
			// if (Physics.Raycast (ray, out hit)) {
			// 	Debug.Log ("dkfd");
			// 	if(hit.collider.gameObject.name == "Path Collision") {
			// 		Debug.Log ("Hit: " + hit.transform.position);
			// 		Instantiate (cannonTower, hit.transform.position, Quaternion.identity);			
			// 	}
			// }


		}

	}

		
}
