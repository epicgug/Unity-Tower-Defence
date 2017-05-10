using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : Tower {


	public float shootRate;
	public float radius;
	public float damage;
	public float turnRate;

	public Transform cannonMuzzle;
	public GameObject cannonProjectile;
	public CircleCollider2D radiusCollider;
	public SpriteRenderer spriteRenderer;

	private List<Enemy> currentCollisions = new List<Enemy>();
	private float shotTimer;
	private Quaternion lookRotation;
	public Enemy targetEnemy;

	public int[] damageUpgrades;
	public int[] damageUpgradesCost;
	public float shootRateUpgradeChange = .05f;
	public int damageUpgradeChange = 3;
	public float rangeUpgradeChange = .5f;
	public float shootRateUpgradeCost = 20;
	public float damageUpgradeCost = 20;
	public float rangeUpgradeCost = 20;

	public string UIInfo {
		get {
			return "Fire Rate: " +  string.Format("{0:0.00}", shootRate) + "\n"
				+ "Range: " + string.Format("{0:0.00}", radius) + "\n"
				+ "Damage: "+ damage;
		}
	} 

	public string type {
		get {
			return "Cannon Tower";
		}
	}

	// Use this for initialization
	void Start () {

		damage = 5;
		// Debug.Log(AssetDatabase.GetAssetPath);
	}

	// Update is called once per frame
	void Update () {
		shotTimer += Time.deltaTime;
		radiusCollider.radius = radius;

		if(currentCollisions.Count != 0) {
			if(lookRotation != null)
			if(Time.deltaTime > 0)
				cannonMuzzle.rotation = Quaternion.Lerp (cannonMuzzle.rotation, lookRotation, Time.deltaTime * turnRate);
		}

		if (targetEnemy != null) {
			AimPosition (targetEnemy.transform.position);	
		}
		calculateFarthestEnemy ();
		checkShoot ();
	}

	public void Select() {
		spriteRenderer.color = Color.red;
		//		UI.localUI.testInfoHash.text = this.GetHashCode () + "";
	}

	public void Deselect() {
		spriteRenderer.color = Color.white;
	}

	public void upgradeDamage() {
		damage += 1;
		GC.local.gold -= damageUpgradeCost;
	}

	public void upgradeShotSpeed() {
		if(shootRate < shootRateUpgradeChange) {
			Debug.Log ("fix");
		} else {
			shootRate -= shootRateUpgradeChange;
			GC.local.gold -= shootRateUpgradeCost;
		}
	}

	public void upgradeRange() {
		radius += 0.5f;
		GC.local.gold -= rangeUpgradeCost;
	}

	//	void OnCollisionEnter2D(Collision2D col) {
	//		Debug.Log ("enter");
	//		currentCollisions.Add (col.gameObject.GetComponent<Enemy>());
	//	}
	//
	void OnTriggerEnter2D(Collider2D col) {
		//Debug.Log ("entered");
		currentCollisions.Add (col.gameObject.GetComponent<Enemy>());
	}

	void OnTriggerExit2D (Collider2D col) {
		currentCollisions.Remove (col.gameObject.GetComponent<Enemy>());
		if(currentCollisions.Count == 0) {
			currentCollisions.Clear ();
		}
		currentCollisions.TrimExcess ();

	}

	void checkShoot() {
		if(shotTimer >= shootRate && targetEnemy != null) {
			attack ();
		}
	}

	void calculateFarthestEnemy() {
		Enemy farthestTarget = null;
		int highestIndex = 0;
		float farthestT = 0;
		Enemy farthestEnemy = null;
		for(int i = 0; i < currentCollisions.Count; i++) {
			if (currentCollisions [i] != null) {
				if (currentCollisions [i].pathNodeIndex >= highestIndex) {
					highestIndex = currentCollisions [i].pathNodeIndex;
					farthestTarget = (currentCollisions [i]);
				}
			}
		}
		for(int i = 0; i < currentCollisions.Count; i++) {
			Enemy collider = currentCollisions [i];
			if (collider == null)
				continue;
			if (collider.pathNodeIndex != highestIndex)
				continue;
			if (collider.t > farthestT) {
				farthestT = collider.t;
				farthestEnemy = collider;
			}
		}
		targetEnemy = farthestEnemy;

	}

	void attack() {
		shotTimer = 0f;
		GameObject bullet = (GameObject)Instantiate (cannonProjectile, cannonMuzzle.position, cannonMuzzle.rotation);
		if (bullet != null) {
			bullet.SendMessage ("ReceiveEndPoint", targetEnemy);
			bullet.SendMessage ("setDamage", damage);
		}
	}

	void AimPosition(Vector3 target) {
		//		Vector3 aimPoint = new Vector3 (target.x, target.y, 0) - transform.position;
		//		lookRotation = Quaternion.LookRotation (aimPoint);
		Vector3 dir = targetEnemy.transform.position - transform.position;
		float angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) + 90;
		lookRotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}



}
