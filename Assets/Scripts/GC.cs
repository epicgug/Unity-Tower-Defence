using UnityEngine;
using System.Collections;

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

	public float money;
	public float cannonTowerCost;

	public bool canPlaceTower;
	public float timeBetweenWaves;
	public bool gameStart;
	public int currentWave = 0;

	public Wave[] waves;
	public static Vector2[] nodes;
	public GameObject goblin, cannonTower;

	private int goblinsSpawned;
	private int rockMonstersSpawned;
	private float goblinLastSpawnTime;
	private float rockMonsterLastSpawnTime;
	private float lastSpawnTime;

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
			Debug.Log (this.selected);
			this.selected.Select ();
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
		GC.local = this;
		lastSpawnTime = Time.time;
		goblinLastSpawnTime = Time.time;
		rockMonsterLastSpawnTime = Time.time;
	}

	// Update is called once per frame
	void Update () {
		if(!gameStart) {
			createTowers ();
			waveManager();
		}
	}

	void makeGoblin() {
		Instantiate (goblin);
	}

	void createTowers() {
		if(!UI.localUI.isMouseOver && Input.GetButtonDown("Fire1")) {

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
			Grid grid = GameObject.Find ("Grid").GetComponent<Grid> ();
			RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
//			Debug.Log (Camera.main.ScreenToWorldPoint (Input.mousePosition));
			foreach(RaycastHit2D hit in hits) {
				if((hit.collider.gameObject.name == "Land Collision")) {
					Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
					Vector3 smoothPoint = grid.remapPoint (mousePos);
					if (grid.placeTower (smoothPoint)) {
						Instantiate (cannonTower, new Vector3 (smoothPoint.x, smoothPoint.y, 0), Quaternion.identity);
					}
				}
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
