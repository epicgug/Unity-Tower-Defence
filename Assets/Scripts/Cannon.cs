using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : Tower {

	public float turnRate;

	public Transform cannonMuzzle;
	public GameObject cannonProjectile;
	private Quaternion lookRotation;

	public int[] damageUpgrades;
	public int[] damageUpgradesCost;

//	public string UIInfo {
//		get {
//			return "Fire Rate: " +  string.Format("{0:0.00}", shootRate) + "\n"
//				+ "Range: " + string.Format("{0:0.00}", radius) + "\n"
//				+ "Damage: "+ damage;
//		}
//	} 

	public override string type {
		get {
			return "Cannon Tower";
		}
	}

	// Use this for initialization
	void Start () {
		radiusCollider.radius = radius;
		shootRateUpgradeChange = .05f;
		damageUpgradeChange = 3;
		rangeUpgradeChange = .5f;
		shootRateUpgradeCost = 20;
		damageUpgradeCost = 20;
		rangeUpgradeCost = 20;
		damage = 5;
		damageInfo = new DamageInfo (damage, armorPen);
		// Debug.Log(AssetDatabase.GetAssetPath);
	}

	// Update is called once per frame
	void Update () {
		shotTimer += Time.deltaTime;


		if(currentCollisions.Count != 0) {
//			if(lookRotation != null)
			if(Time.deltaTime > 0)
				cannonMuzzle.rotation = Quaternion.Lerp (cannonMuzzle.rotation, lookRotation, Time.deltaTime * turnRate);
		}

		if (targetEnemy != null) {
			AimPosition (targetEnemy.transform.position);	
		}
		calculateFarthestEnemy ();
		checkShoot ();
	}

//	public override void Select() {
//		spriteRenderer.color = Color.red;
//		//		UI.localUI.testInfoHash.text = this.GetHashCode () + "";
//	}
//
//	public override void Deselect() {
//		spriteRenderer.color = Color.white;
//	}

	public override void upgradeDamage() {
		if (GC.local.gold >= damageUpgradeCost) {
			GC.local.gold -= damageUpgradeCost;
			damage += 1;
		}
	}

	public override void upgradeShotSpeed() {
		if(shootRate < shootRateUpgradeChange) {
			Debug.Log ("fix");
		} else {
			if (GC.local.gold >= shootRateUpgradeCost) {
				shootRate -= shootRateUpgradeChange;
				GC.local.gold -= shootRateUpgradeCost;
			}
		}
	}

	public override void upgradeRange() {
		radius += 0.5f;
		GC.local.gold -= rangeUpgradeCost;
		radiusCollider.radius = radius;
	}

	//	void OnCollisionEnter2D(Collision2D col) {
	//		Debug.Log ("enter");
	//		currentCollisions.Add (col.gameObject.GetComponent<Enemy>());
	//	}
	//
//	void OnTriggerEnter2D(Collider2D col) {
//		//Debug.Log ("entered");
//		if(col.gameObject.GetComponent<Enemy>() != null)
//			currentCollisions.Add (col.gameObject.GetComponent<Enemy>());
//	}
//
//	void OnTriggerExit2D (Collider2D col) {
//		currentCollisions.Remove (col.gameObject.GetComponent<Enemy>());
//		if(currentCollisions.Count == 0) {
//			currentCollisions.Clear ();
//		}
//		currentCollisions.TrimExcess ();
//	}

	protected override void checkShoot() {
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

	protected override void attack() {
		shotTimer = 0f;
		GameObject bullet = (GameObject)Instantiate (cannonProjectile, cannonMuzzle.position, cannonMuzzle.rotation);
		if (bullet != null) {
			bullet.SendMessage ("setDamageInfo", damageInfo);
			bullet.SendMessage ("ReceiveEndPoint", targetEnemy); 
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
